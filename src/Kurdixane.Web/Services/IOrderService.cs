using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.ViewModels;

namespace Kurdixane.Web.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CheckoutViewModel checkout, CartViewModel cart, int? userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<List<Order>> GetOrdersByUserAsync(int userId);
    Task UpdateStatusAsync(int orderId, OrderStatus status);
    Task<decimal> GetTotalRevenueAsync();
    Task<int> GetOrderCountAsync();
}
