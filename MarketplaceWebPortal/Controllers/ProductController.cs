using MarketplaceWebPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceWebPortal.Controllers
{
    //[Authorize(Policy = "Authenticated")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> results = _context.Products;
            return View(results);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchViewModel viewModel)
        {
            IEnumerable<Product> results = _context.Products.Where(u => u.Name.Contains(viewModel.Keyword) &&
                u.Type == viewModel.Type);
            return View("SearchResult", results);
        }


        public IActionResult Compare([FromQuery] string list)
        {
            List<int> idList = list.Split(",").Select(Int32.Parse).ToList();
            Product[] results = _context.Products.Where(u => idList.Contains(u.Id)).ToArray();
            return View(results);
        }

        //[HttpPost]
        //public IActionResult Compare()
        //{
        //    Console.WriteLine("Compare function called");
        //    return RedirectToAction("CompareResult", ProductList);
        //}

        public IActionResult Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _context.Products.FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(product.File.FileName);
                string fileName = product.FileName + fileInfo.Extension;
                Console.WriteLine(fileName);
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    product.File.CopyTo(stream);
                }
                product.FileName = Path.Combine("/Files/", fileName);

                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["success"] = "Product created successfully";
            }


            return RedirectToAction("Index");
        }
    }
}
