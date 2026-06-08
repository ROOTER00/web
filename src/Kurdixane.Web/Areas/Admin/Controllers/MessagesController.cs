using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class MessagesController : AdminBaseController
{
    private readonly IContentService _content;

    public MessagesController(IContentService content) => _content = content;

    public async Task<IActionResult> Index() => View(await _content.GetContactMessagesAsync());

    public async Task<IActionResult> Detail(int id)
    {
        var msg = await _content.GetContactMessageByIdAsync(id);
        if (msg == null) return NotFound();
        if (!msg.IsRead) await _content.MarkContactMessageReadAsync(id);
        return View(msg);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _content.DeleteContactMessageAsync(id);
        TempData["Success"] = "Mesaj silindi.";
        return RedirectToAction(nameof(Index));
    }
}
