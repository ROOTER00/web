using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class SlidersController : AdminBaseController
{
    private readonly IContentService _content;

    public SlidersController(IContentService content) => _content = content;

    public async Task<IActionResult> Index() => View(await _content.GetAllSlidersAsync());

    public IActionResult Create() => View(new Slider { IsActive = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Slider model)
    {
        if (!ModelState.IsValid) return View(model);
        await _content.SaveSliderAsync(model);
        TempData["Success"] = "Slider eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var s = await _content.GetSliderByIdAsync(id);
        if (s == null) return NotFound();
        return View(s);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Slider model)
    {
        if (!ModelState.IsValid) return View(model);
        await _content.SaveSliderAsync(model);
        TempData["Success"] = "Slider güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _content.DeleteSliderAsync(id);
        TempData["Success"] = "Slider silindi.";
        return RedirectToAction(nameof(Index));
    }
}
