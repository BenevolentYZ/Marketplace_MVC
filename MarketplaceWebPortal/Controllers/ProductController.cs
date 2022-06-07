using MarketplaceWebPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceWebPortal.Controllers
{
    //[Authorize(Policy = "Authenticated")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Product> _compareList;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            _compareList = new List<Product>();
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

        public IActionResult Compare()
        {
            Product[] results = _context.Products.Where(u => u.Type == "Electrical").ToArray();
            return View(results);
        }

        public IActionResult UploadImage()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Upload(SingleFileModel model)
        {
           
            //if (ModelState.IsValid)
            {
               
                model.IsResponse = true;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.File.FileName);
                string fileName = model.FileName + fileInfo.Extension;
                
                string fileNameWithPath = Path.Combine(path, fileName);
                
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                model.IsSuccess = true;
                TempData["Success"] = "File upload successfully";
            }
            return View("UploadImage", model);
        }
    }
}
