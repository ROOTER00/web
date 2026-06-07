using System.Collections.Generic;
using System.Linq;

namespace Kurdixane.Web.ViewModels;

public class CartLine
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public class CartViewModel
{
    public List<CartLine> Lines { get; set; } = new();
    public decimal Subtotal => Lines.Sum(l => l.LineTotal);
    public int ItemCount => Lines.Sum(l => l.Quantity);
    public decimal ShippingCost => Subtotal > 0 && Subtotal < 500 ? 49.90m : 0m;
    public decimal Total => Subtotal + ShippingCost;
}
