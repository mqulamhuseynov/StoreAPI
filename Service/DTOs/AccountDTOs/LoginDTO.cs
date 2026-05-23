using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
