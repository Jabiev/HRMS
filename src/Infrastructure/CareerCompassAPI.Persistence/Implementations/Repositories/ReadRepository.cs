using CareerCompassAPI.Application.Abstraction.Repositories;
using CareerCompassAPI.Domain.Entities.Common;
using CareerCompassAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CareerCompassAPI.Persistence.Implementations.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity, new()
    {
        private readonly CareerCompassDbContext _context;

        public ReadRepository(CareerCompassDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool isTracking = true, params string[] includes)
        {
            var query = Table.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return isTracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAllByExpression(Expression<Func<T, bool>> expression, int take, int skip, bool isTracking = true, params string[] includes)
        {
            var query = Table.Where(expression).Skip(skip).Take(take).AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return isTracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAllFilteredOrderedBy(Expression<Func<T, bool>> expression, int take, int skip, Expression<Func<T, bool>> orderExpression, bool isOrdered = true, bool isTracking = true, params string[] includes)
        {
            var query = Table.Where(expression).AsQueryable();
            query = isOrdered ? query.OrderBy(orderExpression) : query.OrderByDescending(orderExpression);
            query = query.Skip(skip).Take(take);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return isTracking ? query : query.AsNoTracking();
        }

        public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = true)
        {
            var query = isTracking ? Table : Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            return await Table.FindAsync(Id);
        }
    }
}
