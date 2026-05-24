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
        private readonly IFileService _fileService;

        public ProductService(IRepository<Product> repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task Create(CreateProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            //dtodan gelen melumatlari zad zad elluy entitye
            Product product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description
            };
            //photo null deyilse file service ile upload edib url-ni entitye yaziriq
            if (dto.Photo != null)
            {
                using (var stream = dto.Photo.OpenReadStream())
                {
                    product.ImageUrl = await _fileService.UploadFile(stream, dto.Photo.FileName, "products");
                }
            }

            await _repository.Create(product);

        }

        public async Task Delete(Guid id)
        {
             await _repository.Delete(id);
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

        public async Task Update(Guid id, UpdateProductDTO dto)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var product = await _repository.GetByGuid(id);

            if (product == null) throw new InvalidOperationException("Product not found.");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;

            await _repository.Update(product);
        }
    }
}
