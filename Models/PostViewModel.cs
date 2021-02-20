using System.Collections.Generic;

namespace Blogs.Models
{
    public class PostViewModel
    {
        public Blog blog { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
