using Microsoft.AspNetCore.Mvc;
using Service.DTOs.ProductDTOs;
using Service.Services.Abstracts;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllProduct());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO request) 
        {
         await _productService.Create(request);
            return Ok();
        }
    }
}
