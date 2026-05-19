using Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Abstracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Create(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByGuid(Guid id);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
