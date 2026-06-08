using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class CategoryController : Controller
{
    private readonly ICatalogService _catalog;

    public CategoryController(ICatalogService catalog)
    {
        _catalog = catalog;
    }

    [Route("Category/{slug}")]
    public async Task<IActionResult> Index(string slug, int page = 1)
    {
        var category = await _catalog.GetCategoryBySlugAsync(slug);
        if (category is null)
            return NotFound();

        var model = new ProductListViewModel
        {
            Products = await _catalog.GetProductsAsync(slug, null, page, 12),
            Categories = await _catalog.GetActiveCategoriesAsync(),
            CurrentCategory = category,
            CategorySlug = slug
        };
        return View("~/Views/Product/Index.cshtml", model);
    }
}
