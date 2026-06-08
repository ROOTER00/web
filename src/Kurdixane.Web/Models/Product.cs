using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kurdixane.Web.Models;

public class Product : BaseEntity
{
    [Required, StringLength(250)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(270)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Sku { get; set; }

    [StringLength(500)]
    public string? ShortDescription { get; set; }

    // Rich HTML content edited via CKEditor.
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? OldPrice { get; set; }

    public int Stock { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }
    [StringLength(500)]
    public string? MetaDescription { get; set; }
    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
