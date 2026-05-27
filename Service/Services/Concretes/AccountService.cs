using Domain.Commons;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.AccountDTOs;
using Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Concretes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task CreateRole(CreateRoleDTO dto)
        {
            IdentityRole identityRole = new() { Name = dto.Name };
            await _roleManager.CreateAsync(identityRole);
        }

        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)
        };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> Login(LoginDTO dto)
        {
            AppUser appUser = await _userManager.FindByNameAsync(dto.UsernameOrEmail);

            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(dto.UsernameOrEmail);
            }

            if (appUser == null)
            {
                throw new Exception("Username or password is wrong");
            }

            var checkPassword = await _userManager.CheckPasswordAsync(appUser, dto.Password);

            if (!checkPassword)
            {
                throw new Exception("Username or password is wrong");
            }

            var roles = await _userManager.GetRolesAsync(appUser);
            return GenerateJwtToken(appUser.UserName, roles.ToList());
        }

        public async Task<ApiResponse> Register(RegisterDTO dto)
        {
            AppUser appUser = new() 
            {
            Name = dto.Name,
            Surname = dto.Surname,
            Email = dto.Email,
            UserName = dto.Username
            };

          IdentityResult identityResult = await _userManager.CreateAsync(appUser, dto.Password);

            ApiResponse apiResponse = new();

            if (!identityResult.Succeeded) 
            {
                apiResponse.Errors = identityResult.Errors.Select(e => e.Description).ToList();
                apiResponse.Message = "o deile mans tezden ele";
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            return apiResponse;
        }
    }
}
