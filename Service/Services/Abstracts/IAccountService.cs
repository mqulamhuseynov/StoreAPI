using Domain.Commons;
using Service.DTOs.AccountDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstracts
{
    public interface IAccountService
    {
        Task<ApiResponse> Register(RegisterDTO dto);
        Task CreateRole(CreateRoleDTO dto);
        Task<string?> Login(LoginDTO dto);
    }
}
