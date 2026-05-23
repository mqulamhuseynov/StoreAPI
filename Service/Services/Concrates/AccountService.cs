using Domain.Commons;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.DTOs.AccountDTOs;
using Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Concrates
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateRole(CreateRoleDTO dto)
        {
            IdentityRole identityRole = new() { Name = dto.Name };
            await _roleManager.CreateAsync(identityRole);
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
