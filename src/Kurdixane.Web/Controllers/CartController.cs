using System.Threading.Tasks;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cart;

    public CartController(ICartService cart)
    {
        _cart = cart;
    }

    public IActionResult Index() => View(_cart.GetCart());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, bool ajax = false)
    {
        await _cart.AddAsync(productId, quantity);
        if (ajax)
            return Json(new { success = true, count = _cart.GetCart().ItemCount });
        TempData["Success"] = "Ürün sepete eklendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int productId, int quantity)
    {
        _cart.UpdateQuantity(productId, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        _cart.Remove(productId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        _cart.Clear();
        return RedirectToAction(nameof(Index));
    }
}
