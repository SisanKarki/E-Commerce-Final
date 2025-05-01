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
        private readonly IWebHostEnvironment _env;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> product = _unitOfWork.Product.GetAll().ToList();

            return View(product);
        }

        public IActionResult Upsert(int? id) { //update+Insert
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()

            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u=>u.Id== id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _env.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\image\product\" + fileName;
                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
				obj.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(obj);
			}
            
        }

        //public IActionResult UpdateProduct(int? id)
        //{
        //    if(id==null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var cat = _unitOfWork.Product.Get(u => u.Id == id);
        //    return View(cat);
        //}

        //[HttpPost]
        //public IActionResult UpdateProduct(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

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
