using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public class Category : BaseEntity
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(220)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();

    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }
    [StringLength(500)]
    public string? MetaDescription { get; set; }
    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
