using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required, StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string? AltText { get; set; }

    public int DisplayOrder { get; set; }
}
