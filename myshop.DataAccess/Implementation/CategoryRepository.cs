using myshop.Entities.Models;
using myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var CategoryInDb = _context.Categories.FirstOrDefault(x=>x.Id == category.Id);
            if (CategoryInDb != null)
            {
                CategoryInDb.Name = category.Name;
                CategoryInDb.Description = category.Description;          
                CategoryInDb.CreatedTime = DateTime.Now;
            }
        }
    }
}
