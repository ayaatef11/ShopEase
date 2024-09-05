using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace myshop.Entities.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }=new();

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }=new();

        public decimal Price { get; set; }

        public int Count { get; set; }


    }
}
