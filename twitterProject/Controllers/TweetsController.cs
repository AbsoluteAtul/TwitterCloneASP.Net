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
    public class TweetsController : Controller
    {
        private readonly TwitterContext _context;

        public TweetsController(TwitterContext context)
        {
            _context = context;
        }

        // GET: Tweets
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var twitterContext = _context.Tweets.Include(t => t.User);
            return View(await twitterContext.ToListAsync());
        }

        // GET: Tweets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }

            return View(tweet);
        }

        // GET: Tweets/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Tweets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Tweet tweet)
        {
            try
            {
                ModelState.Remove(nameof(tweet.Likes));
                ModelState.Remove(nameof(tweet.UserId));
                ModelState.Remove(nameof(tweet.User));
                ModelState.Remove(nameof(tweet.CreatedDate));

                tweet.UserId = Int32.Parse(Request.Cookies["Id"]);
                tweet.CreatedDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    _context.Add(tweet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(tweet);
        }

        //public async Task<IActionResult> Create([Bind("Id,Description")] Tweet tweet)
        //{
        //    try
        //    {
        //        // ignore like property
        //        ModelState.Remove(nameof(tweet.Likes));

        //        tweet.UserId = Int32.Parse(Request.Cookies["Id"]);
        //        tweet.CreatedDate = DateTime.Now;

        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(tweet);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", tweet.UserId);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    return View(tweet);
        //}

        // GET: Tweets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", tweet.UserId);
            return View(tweet);
        }

        // POST: Tweets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Description,CreatedDate")] Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tweet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TweetExists(tweet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", tweet.UserId);
            return View(tweet);
        }

        // GET: Tweets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }

            return View(tweet);
        }

        // POST: Tweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tweet = await _context.Tweets.FindAsync(id);
            _context.Tweets.Remove(tweet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TweetExists(int id)
        {
            return _context.Tweets.Any(e => e.Id == id);
        }
    }
}