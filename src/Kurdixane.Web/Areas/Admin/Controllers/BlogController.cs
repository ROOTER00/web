using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class BlogController : AdminBaseController
{
    private readonly IContentService _content;

    public BlogController(IContentService content) => _content = content;

    public async Task<IActionResult> Index() => View(await _content.GetAllBlogPostsAsync());

    public IActionResult Create() => View(new BlogPost { IsPublished = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPost model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) return View(model);
        await _content.SaveBlogPostAsync(model);
        TempData["Success"] = "Yazı eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var post = await _content.GetBlogPostByIdAsync(id);
        if (post == null) return NotFound();
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BlogPost model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) return View(model);
        await _content.SaveBlogPostAsync(model);
        TempData["Success"] = "Yazı güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _content.DeleteBlogPostAsync(id);
        TempData["Success"] = "Yazı silindi.";
        return RedirectToAction(nameof(Index));
    }

    private void NormalizeSlug(BlogPost model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.Generate(model.Title);
            ModelState.Remove(nameof(BlogPost.Slug));
        }
    }
}
