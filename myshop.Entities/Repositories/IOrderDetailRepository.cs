using myshop.Entities.Models;

namespace myshop.Entities.Repositories
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
