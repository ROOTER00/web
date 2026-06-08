using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class ProductController : Controller
{
    private readonly ICatalogService _catalog;

    public ProductController(ICatalogService catalog)
    {
        _catalog = catalog;
    }

    public async Task<IActionResult> Index(string? category, string? search, int page = 1)
    {
        var model = new ProductListViewModel
        {
            Products = await _catalog.GetProductsAsync(category, search, page, 12),
            Categories = await _catalog.GetActiveCategoriesAsync(),
            CurrentCategory = string.IsNullOrWhiteSpace(category) ? null : await _catalog.GetCategoryBySlugAsync(category),
            Search = search,
            CategorySlug = category
        };
        return View(model);
    }

    [Route("urun/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var product = await _catalog.GetProductBySlugAsync(slug);
        if (product is null)
            return NotFound();

        var model = new ProductDetailViewModel
        {
            Product = product,
            RelatedProducts = await _catalog.GetRelatedProductsAsync(product.Id, product.CategoryId, 4)
        };
        return View(model);
    }
}
