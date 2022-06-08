using MarketplaceWebPortal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketplaceWebPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get /User
        public IActionResult Index()
        {
            return View("Login");
        }

        // Post /Login
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            // Server side validation
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _context.Users.Where(u => u.Email == viewModel.Email).FirstOrDefault();
            if (user == null)
            {
                TempData["Error"] = "You are not registered";
            }
            else if (user.Password != viewModel.Password)
            {
                TempData["Error"] = "Incorrect Password";
            }
            else 
            {
                TempData["Success"] = "Login Success";
                var claims = new List<Claim> {
                    new Claim("Authenticated", "true")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            }
            return RedirectToAction("Index", "Product");
        }
        // Get /User/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // POST /User/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel viewModel)
        {
            // Server side validation
            // is it necessary to apply more strict validation policy?
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // map SignUpViewModel to User
            User user = new User();
            user.Email = viewModel.Email;
            user.Password = viewModel.Password;
            user.Username = viewModel.Username;
            // Save it to db
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Register Success";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            TempData["Success"] = "Logout Success";
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index");
        }
    }
}
