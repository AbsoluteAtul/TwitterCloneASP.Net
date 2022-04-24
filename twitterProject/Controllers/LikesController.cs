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
    public class LikesController : Controller
    {
        private readonly TwitterContext _context;

        public LikesController(TwitterContext context)
        {
            _context = context;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var twitterContext = _context.Likes.Include(l => l.Tweet).Include(l => l.User);
            return View(await twitterContext.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Tweet)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.TweetID == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public IActionResult Create()
        {
            ViewData["TweetID"] = new SelectList(_context.Tweets, "Id", "Description");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public async Task<IActionResult> Create([Bind("TweetID,UserID")] Like like)
        {
            try
            {
                //Removing the properties that are not needed while creating a like
                //Validating the Model State
                //Another big issue we faced using .NET 6
                ModelState.Remove(nameof(like.Tweet));
                ModelState.Remove(nameof(like.User));

                bool likeExists = _context.Likes.Any(e => e.UserID == like.UserID && e.TweetID == like.TweetID);

                //If like does not exist create one.
                if (likeExists == false)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(like);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Tweets");
                    }

                    return RedirectToAction("Index", "Tweets");
                }
                //Delete the like if it exists.
                else
                {
                    return RedirectToAction("Delete", new { UserID = like.UserID, TweetID = like.TweetID });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Tweets");
            }
        }

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int TweetId, int UserId)
        {
            var like = await _context.Likes
                .Include(l => l.Tweet)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.TweetID == TweetId && m.UserID == UserId);

            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Tweets");
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.TweetID == id);
        }
    }
}