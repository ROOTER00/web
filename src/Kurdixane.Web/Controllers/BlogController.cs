using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class BlogController : Controller
{
    private readonly IContentService _content;

    public BlogController(IContentService content)
    {
        _content = content;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var model = new BlogListViewModel
        {
            Posts = await _content.GetBlogPostsAsync(page, 9),
            RecentPosts = await _content.GetRecentBlogPostsAsync(5)
        };
        return View(model);
    }

    [Route("blog/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var post = await _content.GetBlogPostBySlugAsync(slug);
        if (post is null)
            return NotFound();
        ViewBag.RecentPosts = await _content.GetRecentBlogPostsAsync(5);
        return View(post);
    }
}
