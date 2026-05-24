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
        public async Task<IActionResult> Create([FromForm] CreateProductDTO request) 
        {
         await _productService.Create(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDTO request)
        {
            await _productService.Update(id, request);
            return Ok();
        }
    }
}
