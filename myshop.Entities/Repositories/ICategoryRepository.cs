using myshop.Entities.Models;

namespace myshop.Entities.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
