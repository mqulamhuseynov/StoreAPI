using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() 
        {
         string salam = "Hello, World!";
            return Ok(salam);
        }
    }
}
