using System.Diagnostics;
using Book.DataAccess.Repository.IRepository;
using Book.Models;

using Microsoft.AspNetCore.Mvc;

namespace BookCommerce.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitofWork;

		public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitofWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> ProductList = _unitofWork.Product.GetAll(includeProperties:"Category");
			return View(ProductList);
		}


        public IActionResult Details(int productId)
        {

			Product product = _unitofWork.Product.Get(u => u.Id == productId, includeProperties: "Category");
			return View(product);
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
