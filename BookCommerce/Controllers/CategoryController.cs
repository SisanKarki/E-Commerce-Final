using Book.DataAccess.Data;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookCommerce.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;
		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			List<Category> CatList = _db.Categories.ToList();
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
				_db.Categories.Add(category);
				_db.SaveChanges();
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
			var CategoryToDisplay = _db.Categories.Find(id);
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
				_db.Categories.Update(cat);
				_db.SaveChanges();
				TempData["success"] = "Category Updated Successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult DeleteCategory(int? id)
		{
			Category category = _db.Categories.Find(id);
			if(category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost,ActionName("DeleteCategory")]
		public IActionResult DeleteCategoryPOST(int? id)
		{
			Category? CategoryToDelete = _db.Categories.Find(id);
			if (CategoryToDelete == null)
			{
				return NotFound();
			}
			_db.Categories.Remove(CategoryToDelete);
			_db.SaveChanges();
			TempData["success"] = "Category Deleted Successfully";
			return RedirectToAction("Index");
		}

		


	}

	
}
