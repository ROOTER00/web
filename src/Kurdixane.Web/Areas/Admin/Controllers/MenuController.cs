using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class MenuController : AdminBaseController
{
    private readonly IContentService _content;

    public MenuController(IContentService content) => _content = content;

    public async Task<IActionResult> Index() => View(await _content.GetAllMenuItemsAsync());

    public IActionResult Create() => View(new MenuItem { IsActive = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItem model)
    {
        if (!ModelState.IsValid) return View(model);
        await _content.SaveMenuItemAsync(model);
        TempData["Success"] = "Menü öğesi eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _content.GetMenuItemByIdAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MenuItem model)
    {
        if (!ModelState.IsValid) return View(model);
        await _content.SaveMenuItemAsync(model);
        TempData["Success"] = "Menü öğesi güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _content.DeleteMenuItemAsync(id);
        TempData["Success"] = "Menü öğesi silindi.";
        return RedirectToAction(nameof(Index));
    }
}
