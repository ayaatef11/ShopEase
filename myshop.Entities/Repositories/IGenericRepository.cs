using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // _Context.Categories.Include("Products").ToList();
        // _Context.Categories.Where(x=>x.Id == id).ToList();
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null,string? Includeword = null);

        // _Context.Categories.Include("Products").ToSingleOrDefault();
        // _Context.Categories.Where(x=>x.Id == id).ToSingleOrDefault();
        T GetFirstorDefault(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null);

        //_Context.Categories.Add(category);
        void Add(T entity);

        //_Context.Categories.Remove(category);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
