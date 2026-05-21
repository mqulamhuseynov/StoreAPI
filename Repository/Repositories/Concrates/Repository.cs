using Domain.Commons;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Concrates
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _entities;
        public Repository(AppDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            if(id == null) throw new ArgumentNullException(nameof(id)); 

            var entity = await _entities.FindAsync(id);
            
            if(entity == null) throw new NullReferenceException($"Entity with id {id} not found.");

            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll() =>  await _entities.ToListAsync();

        public async Task<T> GetByGuid(Guid id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var entity = await _entities.FindAsync(id);

            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return entity;
        }
      
        public async Task Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _entities.Update(entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        
    }
}
