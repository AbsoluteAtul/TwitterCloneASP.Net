using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using twitterProject.Data;
using twitterProject.Models;

namespace twitterProject.Controllers
{
    public class FollowsController : Controller
    {
        private readonly TwitterContext _context;

        public FollowsController(TwitterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create([Bind("UserID, FollowingID")] Follow follow)
        {
            try
            {
                ModelState.Remove(nameof(follow.Following));
                bool flag = _context.Follow.Any(e => e.FollowingID == follow.FollowingID && e.UserID == follow.UserID);

                if (flag == false)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(follow);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Users");
                    }

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Delete", new { UserID = follow.UserID, FollowingID = follow.FollowingID });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Tweets");
            }
        }

        public async Task<IActionResult> Delete(int UserID, int FollowingID)
        {
            var follow = _context.Follow
                .Include(f => f.Following)
                .FirstOrDefault<Follow>(m => m.UserID == UserID && m.FollowingID == FollowingID);

            if (follow == null)
            {
                return NotFound();
            }

            _context.Follow.Remove(follow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Users");
        }

        private bool FollowExists(int id)
        {
            return _context.Follow.Any(e => e.FollowingID == id);
        }
    }
}