using Microsoft.EntityFrameworkCore;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public abstract class BaseService<T, TSearch, TEntity> : IService<T, TSearch> where T : class where TSearch : class where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<T>> GetAsync(TSearch search)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            var list = await query.ToListAsync();

            return list.Select(MapToResponse).ToList();
        }

        protected abstract T MapToResponse(TEntity entity);

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity != null ? MapToResponse(entity) : null;
        }
    }
}



 
