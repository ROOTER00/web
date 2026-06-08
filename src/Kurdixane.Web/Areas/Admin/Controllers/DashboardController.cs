using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Areas.Admin.Models;
using Kurdixane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kurdixane.Web.Areas.Admin.Controllers;

public class DashboardController : AdminBaseController
{
    private readonly ICatalogService _catalog;
    private readonly IOrderService _orders;
    private readonly IUserService _users;
    private readonly IContentService _content;

    public DashboardController(ICatalogService catalog, IOrderService orders, IUserService users, IContentService content)
    {
        _catalog = catalog;
        _orders = orders;
        _users = users;
        _content = content;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _catalog.GetAllProductsAsync();
        var orders = await _orders.GetAllOrdersAsync();
        var model = new DashboardViewModel
        {
            ProductCount = products.Count,
            CategoryCount = (await _catalog.GetAllCategoriesAsync()).Count,
            OrderCount = orders.Count,
            UserCount = (await _users.GetAllAsync()).Count,
            BlogCount = (await _content.GetAllBlogPostsAsync()).Count,
            MessageCount = (await _content.GetContactMessagesAsync()).Count,
            TotalRevenue = await _orders.GetTotalRevenueAsync(),
            RecentOrders = orders.Take(5).ToList(),
            LowStockProducts = products.Where(p => p.Stock < 10).Take(5).ToList()
        };
        return View(model);
    }
}
