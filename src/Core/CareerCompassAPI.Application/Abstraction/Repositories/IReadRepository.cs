using CareerCompassAPI.Domain.Entities.Common;
using System.Linq.Expressions;

namespace CareerCompassAPI.Application.Abstraction.Repositories
{
    public interface IReadRepository<T>:IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool isTracking = true, params string[] includes);
        IQueryable<T> GetAllByExpression(Expression<Func<T, bool>> expression,
                                         int take,
                                         int skip,
                                         bool isTracking = true,
                                         params string[] includes);
        IQueryable<T> GetAllFilteredOrderedBy(Expression<Func<T, bool>> expression,
                                              int take,
                                              int skip,
                                              Expression<Func<T, bool>> orderExpression,
                                              bool isOrdered = true,
                                              bool isTracking = true);
        Task<T> GetByIdAsync(Guid Id);
        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = true);
    }
}
