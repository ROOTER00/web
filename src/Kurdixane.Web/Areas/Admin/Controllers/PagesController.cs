using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class PagesController : AdminBaseController
{
    private readonly IContentService _content;

    public PagesController(IContentService content) => _content = content;

    public async Task<IActionResult> Index() => View(await _content.GetAllPagesAsync());

    public IActionResult Create() => View(new Page { IsActive = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Page model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) return View(model);
        await _content.SavePageAsync(model);
        TempData["Success"] = "Sayfa eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var page = await _content.GetPageByIdAsync(id);
        if (page == null) return NotFound();
        return View(page);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Page model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) return View(model);
        await _content.SavePageAsync(model);
        TempData["Success"] = "Sayfa güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _content.DeletePageAsync(id);
        TempData["Success"] = "Sayfa silindi.";
        return RedirectToAction(nameof(Index));
    }

    private void NormalizeSlug(Page model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.Generate(model.Title);
            ModelState.Remove(nameof(Page.Slug));
        }
    }
}
