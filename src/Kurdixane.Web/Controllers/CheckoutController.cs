using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Services;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cart;
    private readonly IOrderService _orders;

    public CheckoutController(ICartService cart, IOrderService orders)
    {
        _cart = cart;
        _orders = orders;
    }

    public IActionResult Index()
    {
        var cart = _cart.GetCart();
        if (cart.ItemCount == 0)
        {
            TempData["Info"] = "Sepetiniz boş.";
            return RedirectToAction("Index", "Cart");
        }
        return View(new CheckoutViewModel { Cart = cart });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        var cart = _cart.GetCart();
        if (cart.ItemCount == 0)
        {
            TempData["Info"] = "Sepetiniz boş.";
            return RedirectToAction("Index", "Cart");
        }

        model.Cart = cart;
        if (!ModelState.IsValid)
            return View(model);

        var order = await _orders.CreateOrderAsync(model, cart, User.GetUserId());
        _cart.Clear();
        return RedirectToAction(nameof(Success), new { orderNumber = order.OrderNumber });
    }

    public IActionResult Success(string orderNumber)
    {
        ViewBag.OrderNumber = orderNumber;
        return View();
    }
}
