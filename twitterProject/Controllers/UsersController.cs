#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using twitterProject.Data;
using twitterProject.Models;

namespace twitterProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly TwitterContext _context;
        //Log messages
        private readonly ILogger<HomeController> _logger;
        //Request and response information
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(TwitterContext context, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await _context.Users
                .Include(u => u.Follows)
                .ToListAsync());
        }

        // GET:
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Password,ImageUrl")] User user)
        {
            try
            {
                // Ignore tweet, like and follow property in order to validate the model
                ModelState.Remove(nameof(user.Tweets));
                ModelState.Remove(nameof(user.Likes));
                ModelState.Remove(nameof(user.Follows));

                if (ModelState.IsValid)
                {
                    //Before creating a new user, check if the email already exists.
                    //If exists cancel operating and show error
                    if (EmailExists(user.Email))
                    {
                        ViewBag.message = "Email already in Use!";
                        return View();
                    }
                    //Otherwise create a new user.
                    else
                    {
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //If the user is not logged in, redirect him to login page.
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Password,ImageUrl")] User user)
        {
            // Ignore tweet, like and follow property in order to validate the model
            ModelState.Remove(nameof(user.Tweets));
            ModelState.Remove(nameof(user.Likes));
            ModelState.Remove(nameof(user.Follows));
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Response.Cookies.Append("Check", user.FirstName);
                return RedirectToAction("Index", "Tweets");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }
    }
}
