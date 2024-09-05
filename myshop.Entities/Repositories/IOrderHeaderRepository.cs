using myshop.Entities.Models;

namespace myshop.Entities.Repositories
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus);
    }
}
