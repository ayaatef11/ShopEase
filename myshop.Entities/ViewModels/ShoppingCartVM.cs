using myshop.Entities.Models;

namespace myshop.Entities.ViewModels
{
	public class ShoppingCartVM
	{
        public IEnumerable<ShoppingCart> CartsList { get; set; }= new List<ShoppingCart>();
        public OrderHeader OrderHeader { get; set; } = new OrderHeader();
    }
}
