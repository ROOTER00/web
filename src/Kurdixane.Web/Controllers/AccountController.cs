using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _users;
    private readonly IOrderService _orders;

    public AccountController(IUserService users, IOrderService orders)
    {
        _users = users;
        _orders = orders;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
        => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _users.ValidateCredentialsAsync(model.Email, model.Password);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
            return View(model);
        }

        await SignInAsync(user, model.RememberMe);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return user.Role == UserRole.Admin
            ? RedirectToAction("Index", "Dashboard", new { area = "Admin" })
            : RedirectToAction("Profile");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error, user) = await _users.RegisterAsync(model.FullName, model.Email, model.Password);
        if (!success || user is null)
        {
            ModelState.AddModelError(string.Empty, error ?? "Kayıt başarısız.");
            return View(model);
        }

        await SignInAsync(user, false);
        TempData["Success"] = "Kayıt başarılı. Hoş geldiniz!";
        return RedirectToAction("Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = User.GetUserId();
        if (userId is null) return RedirectToAction("Login");
        ViewBag.Orders = await _orders.GetOrdersByUserAsync(userId.Value);
        var user = await _users.GetByIdAsync(userId.Value);
        return View(user);
    }

    public IActionResult AccessDenied() => View();

    private async Task SignInAsync(User user, bool persist)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var props = new AuthenticationProperties { IsPersistent = persist };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), props);
    }
}
