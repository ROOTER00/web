using System.Collections.Generic;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.Areas.Admin.Models;

public class DashboardViewModel
{
    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int OrderCount { get; set; }
    public int UserCount { get; set; }
    public int BlogCount { get; set; }
    public int MessageCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
    public List<Product> LowStockProducts { get; set; } = new();
}
