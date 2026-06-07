using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class OrdersController : AdminBaseController
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    public async Task<IActionResult> Index() => View(await _orders.GetAllOrdersAsync());

    public async Task<IActionResult> Detail(int id)
    {
        var order = await _orders.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        await _orders.UpdateStatusAsync(id, status);
        TempData["Success"] = "Sipariş durumu güncellendi.";
        return RedirectToAction(nameof(Detail), new { id });
    }
}
