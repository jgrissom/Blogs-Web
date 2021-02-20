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
                _bloggingContext.AddBlog(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
