
using myshop.DataAccess;

namespace myshop.Web.Areas.Admin.Controllers
{
    
    [Area("Admin")]
   // [Authorize(Roles =SD.AdminRole)]
    public class UsersController(ApplicationDbContext _context) : Controller
    {

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;

            return View(_context.ApplicationUsers.Where(x=>x.Id != userid).ToList());
        }

        public IActionResult LockUnlock(string? id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Users", new {area = "Admin"});
        }
    }
}
