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

        //Action to create a follow. Instead of calling the view, we just call the action to create a follow.
        [HttpGet]
        public async Task<IActionResult> Create([Bind("UserID, FollowingID")] Follow follow)
        {
            try
            {
                //Check if the suer is already following another user
                ModelState.Remove(nameof(follow.Following));
                bool isFollowing = _context.Follow.Any(e => e.FollowingID == follow.FollowingID && e.UserID == follow.UserID);

                //If not, then follow him/her
                if (isFollowing == false)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(follow);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Users");
                    }

                    return RedirectToAction("Index", "Users");
                }
                //Otherwise delete the follow. or Simply unfollow anothr user.
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

        //Action to delete a follow. Here also instead of calling the view, we directly call the action to unfollow a user.
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