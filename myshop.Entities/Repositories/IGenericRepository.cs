
using System.Linq.Expressions;

namespace myshop.Entities.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null,string? Includeword = null);
        T GetFirstorDefault(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null);
        void Add(T entity);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
