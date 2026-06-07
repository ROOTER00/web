using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Repositories;
using Kurdixane.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orders;

    public OrderService(IRepository<Order> orders)
    {
        _orders = orders;
    }

    public async Task<Order> CreateOrderAsync(CheckoutViewModel checkout, CartViewModel cart, int? userId)
    {
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            CustomerName = checkout.CustomerName,
            Email = checkout.Email,
            Phone = checkout.Phone,
            Address = checkout.Address,
            City = checkout.City,
            Note = checkout.Note,
            Status = OrderStatus.Pending,
            TotalAmount = cart.Total,
            Items = cart.Lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                ProductName = l.Name,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity
            }).ToList()
        };

        await _orders.AddAsync(order);
        await _orders.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
        => await _orders.Query().Include(o => o.Items)
            .OrderByDescending(o => o.Id).ToListAsync();

    public async Task<Order?> GetOrderByIdAsync(int id)
        => await _orders.Query().Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        => await _orders.Query().Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Id).ToListAsync();

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _orders.GetByIdAsync(orderId);
        if (order is null) return;
        order.Status = status;
        _orders.Update(order);
        await _orders.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync()
        => await _orders.Query()
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

    public async Task<int> GetOrderCountAsync() => await _orders.CountAsync();

    private static string GenerateOrderNumber()
        => $"KX{DateTime.UtcNow:yyyyMMdd}{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
}
