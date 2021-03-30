using Microsoft.AspNetCore.Mvc;
using Blogs.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        // this controller depends on the BloggingRepository
        private BloggingContext _bloggingContext;
        public HomeController(BloggingContext db) => _bloggingContext = db;

        public IActionResult Index() => View(_bloggingContext.Blogs.OrderBy(b => b.Name));
        public IActionResult BlogDetail(int id) => View(new PostViewModel
        {
            blog = _bloggingContext.Blogs.FirstOrDefault(b => b.BlogId == id),
            Posts = _bloggingContext.Posts.Where(p => p.BlogId == id)
        });
        [Authorize]
        public IActionResult AddBlog() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBlog(Blog model)
        {
            if (ModelState.IsValid)
            {
                if (_bloggingContext.Blogs.Any(b => b.Name == model.Name))
                {
                    ModelState.AddModelError("", "Name must be unique");
                }
                else
                {
                    _bloggingContext.AddBlog(model);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public IActionResult DeleteBlog(int id)
        {
            _bloggingContext.DeleteBlog(_bloggingContext.Blogs.FirstOrDefault(b => b.BlogId == id));
            return RedirectToAction("Index");
        }
         public IActionResult AddPost(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(int id, Post post)
        {
            post.BlogId = id;
            if (ModelState.IsValid)
            {
                _bloggingContext.AddPost(post);
                return RedirectToAction("BlogDetail", new { id = id });
            }
            @ViewBag.BlogId = id;
            return View();
        }
        public IActionResult DeletePost(int id)
        {
            Post post = _bloggingContext.Posts.FirstOrDefault(p => p.PostId == id);
            int BlogId = post.BlogId;
            _bloggingContext.DeletePost(post);
            return RedirectToAction("BlogDetail", new { id = BlogId });
        }
    }
}
