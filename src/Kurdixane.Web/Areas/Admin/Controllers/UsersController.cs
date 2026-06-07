using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class UsersController : AdminBaseController
{
    private readonly IUserService _users;

    public UsersController(IUserService users) => _users = users;

    public async Task<IActionResult> Index() => View(await _users.GetAllAsync());

    public IActionResult Create() => View(new User { IsActive = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User model, string? password)
    {
        ModelState.Remove(nameof(Kurdixane.Web.Models.User.PasswordHash));
        if (string.IsNullOrWhiteSpace(password))
            ModelState.AddModelError("password", "Şifre gereklidir.");
        if (await _users.EmailExistsAsync(model.Email))
            ModelState.AddModelError(nameof(Kurdixane.Web.Models.User.Email), "Bu e-posta zaten kayıtlı.");
        if (!ModelState.IsValid) return View(model);
        await _users.SaveAsync(model, password);
        TempData["Success"] = "Kullanıcı eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User model, string? password)
    {
        ModelState.Remove(nameof(Kurdixane.Web.Models.User.PasswordHash));
        if (!ModelState.IsValid) return View(model);
        await _users.SaveAsync(model, string.IsNullOrWhiteSpace(password) ? null : password);
        TempData["Success"] = "Kullanıcı güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _users.DeleteAsync(id);
        TempData["Success"] = "Kullanıcı silindi.";
        return RedirectToAction(nameof(Index));
    }
}
