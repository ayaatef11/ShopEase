using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IShoppingCartRepository ShoppingCart { get; }

        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }

        IApplicationUserRepository ApplicationUser { get; }
        int Complete();
    }
}
