using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts.Data
{
    public partial interface IRepository<T> : IDisposable where T : class
    {

        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        Task Insert(T entity);

        Task Insert(IEnumerable<T> entities);

        Task Update(T entity);

        Task Update(IEnumerable<T> entities);

        Task Delete(T entity);

        Task Delete(IEnumerable<T> entities);

        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, Task<IOrderedQueryable<T>>> orderBy = null,
            string includeProperties = "");

        IEnumerable<T> GetPagedElements<S>(int pageIndex, int pageCount,
            Expression<Func<T, S>> orderByExpression, bool ascending,
            Expression<Func<T, bool>> filter = null, string includeProperties = "");

        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }

        Task CancelChanges(T entity);
    }
}
