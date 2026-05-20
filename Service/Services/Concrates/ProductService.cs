using Domain.Entities;
using Repository.Repositories.Abstracts;
using Service.DTOs.ProductDTOs;
using Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Concrates
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task Create(CreateProductDTO dto)
        {
            Product product = new Product
            {
             Name = dto.Name,
             Price = dto.Price,
             Description = dto.Description
            };

         await _repository.Create(product);
            
        }

        public async Task<IEnumerable<GetAllProductDTO>> GetAllProduct()
        {
            var products = await _repository.GetAll();

            var mapProducts = products.Select(p => new GetAllProductDTO
            {
                Id = p.Id, 
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });
            return mapProducts;
        }
    }
}
