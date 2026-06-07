using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class PageController : Controller
{
    private readonly IContentService _content;

    public PageController(IContentService content)
    {
        _content = content;
    }

    [Route("Page/{slug}")]
    [Route("sayfa/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var page = await _content.GetPageBySlugAsync(slug);
        if (page is null)
            return NotFound();
        return View(page);
    }
}
