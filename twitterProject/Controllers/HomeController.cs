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
        //Log messages
        private readonly ILogger<HomeController> _logger;
        //Request and response information
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(TwitterContext context, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //Check if the user is logged in, then redirect him to tweets page.
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

        //We call this function to check if the user info matches an account in the database, then log in or stop accordingly.
        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            string email = formCollection["Email"];

            if (formCollection is null)
            {
                throw new ArgumentNullException(nameof(formCollection));
            }

            var user = _context.Users.FirstOrDefault<User>(m => m.Email == email);

            if (user == null)
            {
                ViewBag.message = "User does not exist!";
                return View();
            }
            else
            {
                if (user.Password.Equals(formCollection["password"].ToString().Trim()))
                {
                    //I tried to set the expiry time for a cookie but could not figure that out.
                    //This is one of the difficulties that we faced and could not figure out.
                    // Response.Cookies("Check") = 12;
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

        //Destroy Cookies
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