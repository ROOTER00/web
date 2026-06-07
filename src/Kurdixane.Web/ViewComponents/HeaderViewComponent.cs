using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly IContentService _content;
    private readonly ICatalogService _catalog;
    private readonly ISettingService _settings;
    private readonly ICartService _cart;

    public HeaderViewComponent(IContentService content, ICatalogService catalog, ISettingService settings, ICartService cart)
    {
        _content = content;
        _catalog = catalog;
        _settings = settings;
        _cart = cart;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new HeaderViewModel
        {
            Menu = await _content.GetMenuAsync(MenuLocation.Header),
            Categories = await _catalog.GetActiveCategoriesAsync(),
            CartItemCount = _cart.GetCart().ItemCount,
            Settings = await _settings.GetAllAsync()
        };
        return View("~/Views/Shared/_Header.cshtml", model);
    }
}
