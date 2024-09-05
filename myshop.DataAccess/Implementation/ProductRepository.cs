
namespace MyShop.DataAccess.Implementation
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductInDb = _context.Products.FirstOrDefault(x=>x.Id == product.Id);
            if (ProductInDb != null)
            {
                ProductInDb.Name = product.Name;
                ProductInDb.Description = product.Description;      
                ProductInDb.Price = product.Price;
                ProductInDb.Img = product.Img;
                ProductInDb.CategoryId = product.CategoryId;

            }
        }
    }
}
