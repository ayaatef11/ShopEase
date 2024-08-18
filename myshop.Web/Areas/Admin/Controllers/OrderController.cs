using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using Stripe;

namespace myshop.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles =SD.AdminRole)]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitofwork;

		[BindProperty]
		public OrderVM OrderVM { get; set; }

		public OrderController(IUnitOfWork unitOfWork)
        {
			_unitofwork = unitOfWork;
        }
        public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetData()
		{
			IEnumerable<OrderHeader> orderHeaders;
			orderHeaders = _unitofwork.OrderHeader.GetAll(Includeword: "ApplicationUser");
			return Json(new { data = orderHeaders });
		}

		public IActionResult Details(int orderid)
		{
			OrderVM orderVM = new OrderVM()
			{
				OrderHeader = _unitofwork.OrderHeader.GetFirstorDefault(u=>u.Id == orderid,Includeword:"ApplicationUser"),
				OrderDetails = _unitofwork.OrderDetail.GetAll(x=>x.OrderHeaderId==orderid,Includeword:"Product")
			};

			return View(orderVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetails()
		{
			var orderfromdb = _unitofwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
			orderfromdb.Name = OrderVM.OrderHeader.Name;
			orderfromdb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderfromdb.Address = OrderVM.OrderHeader.Address;
			orderfromdb.City = OrderVM.OrderHeader.City;

			if (OrderVM.OrderHeader.Carrier != null)
			{
				orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
			}

			if (OrderVM.OrderHeader.TrakcingNumber != null)
			{
				orderfromdb.TrakcingNumber = OrderVM.OrderHeader.TrakcingNumber;
			}

			_unitofwork.OrderHeader.Update(orderfromdb);
			_unitofwork.Complete();
			TempData["Update"] = "Item has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = orderfromdb.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
			_unitofwork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.Proccessing, null);
			_unitofwork.Complete();
			
			TempData["Update"] = "Order Status has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var orderfromdb = _unitofwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
			orderfromdb.TrakcingNumber = OrderVM.OrderHeader.TrakcingNumber;
			orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
			orderfromdb.OrderStatus = SD.Shipped;
			orderfromdb.ShippingDate = DateTime.Now;

			_unitofwork.OrderHeader.Update(orderfromdb);
			_unitofwork.Complete();

			TempData["Update"] = "Order has Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderfromdb = _unitofwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderVM.OrderHeader.Id);
			if (orderfromdb.PaymentStatus == SD.Approve)
			{
				var option = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderfromdb.PaymentIntentId
				};

				var service = new RefundService();
				Refund refund = service.Create(option);

				_unitofwork.OrderHeader.UpdateStatus(orderfromdb.Id, SD.Cancelled, SD.Refund);
			}
			else
			{
				_unitofwork.OrderHeader.UpdateStatus(orderfromdb.Id, SD.Cancelled, SD.Cancelled);
			}
			_unitofwork.Complete();

			TempData["Update"] = "Order has Cancelled Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
		}
	}
}
