using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons
{
    public class ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public string Message { get; set; } = "Success";
    }
}
