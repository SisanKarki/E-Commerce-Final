using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookCommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> product = _unitOfWork.Product.GetAll().ToList();
            return View(product);
        }

        public IActionResult AddProduct() { return View(); }

        [HttpPost]
        public IActionResult AddProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult UpdateProduct(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var cat = _unitOfWork.Product.Get(u => u.Id == id);
            return View(cat);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
