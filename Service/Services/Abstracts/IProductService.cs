using Service.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstracts
{
    public interface IProductService 
    {
        Task Create(CreateProductDTO dto);
        Task<IEnumerable<GetAllProductDTO>> GetAllProduct();
        Task Update(Guid id, UpdateProductDTO dto);
        Task Delete(Guid id);
    }
}
