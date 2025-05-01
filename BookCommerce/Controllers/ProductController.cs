using System.Collections.Generic;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult AddProduct() {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
            );
            ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new ProductVM()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };
            return View(productVM); }

        [HttpPost]
        public IActionResult AddProduct(ProductVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
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

        public IActionResult RemoveProduct(int? id)
        {
            var ProductToDelete = _unitOfWork.Product.Get(u=>u.Id == id);
            return View(ProductToDelete);
        }
        [HttpPost,ActionName("RemoveProduct")]
        public IActionResult RemoveProductPOST(int? id)
        {
            var ProductToDelete = _unitOfWork.Product.Get(u=> u.Id == id);
            if(ProductToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(ProductToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
