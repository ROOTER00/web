using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class ContactController : Controller
{
    private readonly IContentService _content;
    private readonly ISettingService _settings;

    public ContactController(IContentService content, ISettingService settings)
    {
        _content = content;
        _settings = settings;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Settings = await _settings.GetAllAsync();
        return View(new ContactMessage());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessage model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Settings = await _settings.GetAllAsync();
            return View(model);
        }

        await _content.AddContactMessageAsync(model);
        TempData["Success"] = "Mesajınız alındı. En kısa sürede dönüş yapacağız.";
        return RedirectToAction(nameof(Index));
    }
}
