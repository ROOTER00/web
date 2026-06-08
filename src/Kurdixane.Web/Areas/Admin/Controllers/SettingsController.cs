using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class SettingsController : AdminBaseController
{
    private readonly ISettingService _settings;

    public SettingsController(ISettingService settings) => _settings = settings;

    public async Task<IActionResult> Index()
    {
        var values = await _settings.GetAllAsync();
        return View(values);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Dictionary<string, string?> settings)
    {
        await _settings.SetManyAsync(settings);
        TempData["Success"] = "Ayarlar kaydedildi.";
        return RedirectToAction(nameof(Index));
    }
}
