using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookCommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.ShoppingCart.GetAll("Product")
                .Where(u => u.ApplicationUserId == userId);
            
            foreach(var cart in cartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            }

            return View(cartItems);
        }

        public IActionResult AddToCart(int productId, int count = 1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == productId);

            if (cartFromDb == null)
            {
                // Create new cart item
                ShoppingCart cart = new()
                {
                    ProductId = productId,
                    Count = count,
                    ApplicationUserId = userId
                };
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                // Update existing cart item
                cartFromDb.Count += count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb != null)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateCart(int cartId, int count)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb != null)
            {
                cartFromDb.Count = count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Cart item not found" });
        }

        private double GetPriceBasedOnQuantity(int quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else if (quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }
    }
} 