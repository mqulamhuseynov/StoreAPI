using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.AccountDTOs;
using Service.Services.Abstracts;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request) => Ok(await _accountService.Register(request));

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO request)
        {
            await _accountService.CreateRole(request);
            return Ok("Ugurla yaradildi qagaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO request) => Ok(await _accountService.Login(request));

        [Authorize]
        [HttpGet("secret-data")]
        public IActionResult GetSecretData()
        {
            return Ok("Halaldır mans, token saat kimi işləyir və sən bu gizli datanı görə bildin!");
        }
    }
}
