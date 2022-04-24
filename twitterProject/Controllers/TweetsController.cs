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
            //If user is not logged in, redirect to login page.
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Lists to store all the tweets and followings.
            List<Tweet> allTweets = new List<Tweet>();
            List<int> followingList = new List<int>();

            // Only show my tweets and tweets of the users I follow.
            var user = await _context.Users
                .Include(t => t.Tweets)
                .Include(t => t.Follows)
                .FirstOrDefaultAsync(m => m.Id == Int32.Parse(Request.Cookies["Id"]));

            var followings = _context.Follow
                .Where<Follow>(m => m.UserID == user.Id);

            followingList.Add(user.Id);

            foreach (Follow f in followings)
            {
                followingList.Add(f.FollowingID);
            }

            var followingTweets = _context.Tweets
                .Include(t => t.User)
                .Include(t => t.Likes)
                .Where<Tweet>(m => followingList.Contains(m.UserId));

            //Creating a list of all the tweets.
            allTweets = followingTweets.ToList<Tweet>();
            //Order the list by decending order so that latest one comes on top.
            List<Tweet> SortedList = allTweets.OrderByDescending(t => t.CreatedDate).ToList();

            //Returning the list to the view.
            return View(SortedList);
        }

        // GET: Tweets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Request.Cookies["Check"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                //Again validating Model state by removing the properties that are not needed.
                ModelState.Remove(nameof(tweet.Likes));
                ModelState.Remove(nameof(tweet.UserId));
                ModelState.Remove(nameof(tweet.User));
                ModelState.Remove(nameof(tweet.CreatedDate));
                //Fetching user Id from cookies.
                tweet.UserId = Int32.Parse(Request.Cookies["Id"]);
                //Setting current Date and Time.
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