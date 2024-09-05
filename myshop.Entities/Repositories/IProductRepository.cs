using myshop.Entities.Models;

namespace myshop.Entities.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
