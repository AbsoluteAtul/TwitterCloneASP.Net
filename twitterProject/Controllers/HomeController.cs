using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using twitterProject.Data;
using twitterProject.Models;

namespace twitterProject.Controllers
{
    public class HomeController : Controller
    {
        public User loggedUser = new User();

        private readonly TwitterContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(TwitterContext context, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["Check"] != null)
            {
                return RedirectToAction("Index", "Tweets");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            string email = formCollection["Email"];

            if (formCollection is null)
            {
                throw new ArgumentNullException(nameof(formCollection));
            }

            var user = _context.Users.FirstOrDefault<User>(m => m.Email == email);

            //Email does not exist
            if (user == null)
            {
                ViewBag.message = "User does not exist!";
                return View();
            }
            else
            {
                //Checking Password
                if (user.Password.Equals(formCollection["password"].ToString().Trim()))
                {
                    Response.Cookies.Append("Check", user.FirstName);
                    Response.Cookies.Append("Id", user.Id.ToString());
                    loggedUser = user;
                    return RedirectToAction("Index", "Tweets");
                }
                else
                {
                    ViewBag.message = "Email and password does not match";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("Check");
            Response.Cookies.Delete("Id");

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}