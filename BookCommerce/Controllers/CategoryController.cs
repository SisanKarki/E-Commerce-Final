using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookCommerce.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
	
		public CategoryController(IUnitOfWork db)
		{
			_unitOfWork = db;
		}
		public IActionResult Index()
		{
			List<Category> CatList = _unitOfWork.Category.GetAll().ToList();
			return View(CatList);
		}

		public IActionResult AddCategory()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AddCategory(Category category)
		{
			if (ModelState.IsValid)
			{
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
				TempData["success"] = "Category Created Successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult UpdateCategory(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var CategoryToDisplay = _unitOfWork.Category.Get(u=>u.Id == id);
			if (CategoryToDisplay == null)
			{
				return NotFound();
			}
			return View(CategoryToDisplay);
		}

		[HttpPost]
		public IActionResult UpdateCategory(Category cat)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Update(cat);
                _unitOfWork.Save();
				TempData["success"] = "Category Updated Successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult DeleteCategory(int? id)
		{
			Category category = _unitOfWork.Category.Get(u=>u.Id == id);
			if(category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost,ActionName("DeleteCategory")]
		public IActionResult DeleteCategoryPOST(int? id)
		{
			Category? CategoryToDelete = _unitOfWork.Category.Get(u=>u.Id==id);
			if (CategoryToDelete == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(CategoryToDelete);
            _unitOfWork.Save();
			TempData["success"] = "Category Deleted Successfully";
			return RedirectToAction("Index");
		}

		


	}

	
}
