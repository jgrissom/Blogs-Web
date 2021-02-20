using Microsoft.AspNetCore.Mvc;
using Blogs.Models;
using System.Linq;

namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        // this controller depends on the BloggingRepository
        private BloggingContext _bloggingContext;
        public HomeController(BloggingContext db) => _bloggingContext = db;

        public IActionResult Index() => View(_bloggingContext.Blogs.OrderBy(b => b.Name));
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
    }
}
