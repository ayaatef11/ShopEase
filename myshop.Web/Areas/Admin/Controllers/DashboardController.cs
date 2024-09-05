

namespace myshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles =SD.AdminRole)]
    public class DashboardController(IUnitOfWork _unitOfWork) : Controller
    {

        public IActionResult Display()
        {
            ViewBag.Orders  = _unitOfWork.OrderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _unitOfWork.OrderHeader.GetAll(x=>x.OrderStatus == SD.Approve).Count();
            ViewBag.Users = _unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.Products = _unitOfWork.Product.GetAll().Count();
            return View();
        }
    }
}
