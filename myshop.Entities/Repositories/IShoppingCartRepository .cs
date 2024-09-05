using myshop.Entities.Models;

namespace myshop.Entities.Repositories
{
    public interface IShoppingCartRepository:IGenericRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart shoppingcart, int count);
        int DecreaseCount(ShoppingCart shoppingcart, int count);
      
    }
}
