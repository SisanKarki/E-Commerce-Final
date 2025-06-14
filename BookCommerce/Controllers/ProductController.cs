﻿using System.Collections.Generic;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookCommerce.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
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
            List<Product> product = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

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
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)){
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (obj.Product.Id == 0) {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
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

        //public IActionResult RemoveProduct(int? id)
        //{
        //    var ProductToDelete = _unitOfWork.Product.Get(u=>u.Id == id);
        //    return View(ProductToDelete);
        //}
        //[HttpPost,ActionName("RemoveProduct")]
        //public IActionResult RemoveProductPOST(int? id)
        //{
        //    var ProductToDelete = _unitOfWork.Product.Get(u=> u.Id == id);
        //    if(ProductToDelete == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(ProductToDelete);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> product = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = product });

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.Get(u=>id == u.Id);
            if(id== null || ProductToBeDeleted == null)
            {
                return Json(new {success= false,message="Error while deleting"});
            }
            if (!string.IsNullOrEmpty(ProductToBeDeleted.ImageUrl))
            {
                var oldImagePath = Path.Combine(_env.WebRootPath, ProductToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
