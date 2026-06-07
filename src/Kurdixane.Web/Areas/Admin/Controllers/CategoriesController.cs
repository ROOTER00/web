using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class CategoriesController : AdminBaseController
{
    private readonly ICatalogService _catalog;

    public CategoriesController(ICatalogService catalog) => _catalog = catalog;

    public async Task<IActionResult> Index() => View(await _catalog.GetAllCategoriesAsync());

    public async Task<IActionResult> Create()
    {
        await PopulateParents();
        return View(new Category { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) { await PopulateParents(model.ParentId); return View(model); }
        await _catalog.SaveCategoryAsync(model);
        TempData["Success"] = "Kategori eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cat = await _catalog.GetCategoryByIdAsync(id);
        if (cat == null) return NotFound();
        await PopulateParents(cat.ParentId, cat.Id);
        return View(cat);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid) { await PopulateParents(model.ParentId, model.Id); return View(model); }
        await _catalog.SaveCategoryAsync(model);
        TempData["Success"] = "Kategori güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _catalog.DeleteCategoryAsync(id);
        TempData["Success"] = "Kategori silindi.";
        return RedirectToAction(nameof(Index));
    }

    private void NormalizeSlug(Category model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.Generate(model.Name);
            ModelState.Remove(nameof(Category.Slug));
        }
    }

    private async Task PopulateParents(int? selected = null, int? excludeId = null)
    {
        var cats = await _catalog.GetAllCategoriesAsync();
        if (excludeId.HasValue) cats = cats.FindAll(c => c.Id != excludeId.Value);
        ViewBag.Parents = new SelectList(cats, "Id", "Name", selected);
    }
}
