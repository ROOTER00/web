using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kurdixane.Web.Models;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}

public class Order : BaseEntity
{
    [Required, StringLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    public int? UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(40)]
    public string? Phone { get; set; }

    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(100)]
    public string? City { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [StringLength(1000)]
    public string? Note { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
