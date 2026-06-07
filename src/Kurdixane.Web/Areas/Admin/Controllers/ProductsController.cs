using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class ProductsController : AdminBaseController
{
    private readonly ICatalogService _catalog;

    public ProductsController(ICatalogService catalog) => _catalog = catalog;

    public async Task<IActionResult> Index()
    {
        var products = await _catalog.GetAllProductsAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCategories();
        return View(new Product { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid)
        {
            await PopulateCategories(model.CategoryId);
            return View(model);
        }
        await _catalog.SaveProductAsync(model);
        TempData["Success"] = "Ürün eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _catalog.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        await PopulateCategories(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product model)
    {
        NormalizeSlug(model);
        if (!ModelState.IsValid)
        {
            await PopulateCategories(model.CategoryId);
            return View(model);
        }
        await _catalog.SaveProductAsync(model);
        TempData["Success"] = "Ürün güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    private void NormalizeSlug(Product model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.Generate(model.Name);
            ModelState.Remove(nameof(Product.Slug));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _catalog.DeleteProductAsync(id);
        TempData["Success"] = "Ürün silindi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategories(int? selected = null)
    {
        var cats = await _catalog.GetAllCategoriesAsync();
        ViewBag.Categories = new SelectList(cats, "Id", "Name", selected);
    }
}
