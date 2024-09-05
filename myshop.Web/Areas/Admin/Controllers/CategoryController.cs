

namespace myshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(IUnitOfWork _unitOfWork) : Controller
    {
         public IActionResult ViewCategories()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);

            return View(categoryIndb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);

            return View(categoryIndb);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                NotFound();
            }
            _unitOfWork.Category.Remove(categoryIndb);
            _unitOfWork.Complete();
            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
