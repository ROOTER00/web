using System.Diagnostics;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICatalogService _catalog;
    private readonly IContentService _content;

    public HomeController(ICatalogService catalog, IContentService content)
    {
        _catalog = catalog;
        _content = content;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Sliders = await _content.GetActiveSlidersAsync(),
            Categories = await _catalog.GetActiveCategoriesAsync(),
            FeaturedProducts = await _catalog.GetFeaturedProductsAsync(8),
            LatestProducts = await _catalog.GetLatestProductsAsync(8),
            RecentPosts = await _content.GetRecentBlogPostsAsync(3)
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
