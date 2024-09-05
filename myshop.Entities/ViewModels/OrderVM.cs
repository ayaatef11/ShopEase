using myshop.Entities.Models;

namespace myshop.Entities.ViewModels
{
	public class OrderVM
	{
        public OrderHeader OrderHeader { get; set; }=new OrderHeader();

        public IEnumerable<OrderDetail> OrderDetails { get; set; }=Enumerable.Empty<OrderDetail>();
    }
}
