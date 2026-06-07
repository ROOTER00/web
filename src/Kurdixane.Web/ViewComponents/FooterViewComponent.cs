using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly IContentService _content;
    private readonly ICatalogService _catalog;
    private readonly ISettingService _settings;

    public FooterViewComponent(IContentService content, ICatalogService catalog, ISettingService settings)
    {
        _content = content;
        _catalog = catalog;
        _settings = settings;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new FooterViewModel
        {
            Menu = await _content.GetMenuAsync(MenuLocation.Footer),
            Categories = await _catalog.GetActiveCategoriesAsync(),
            Settings = await _settings.GetAllAsync()
        };
        return View("~/Views/Shared/_Footer.cshtml", model);
    }
}
