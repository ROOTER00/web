using System;
using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public class BlogPost : BaseEntity
{
    [Required, StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(270)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Summary { get; set; }

    // Rich HTML content edited via CKEditor.
    public string? Content { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(150)]
    public string? Author { get; set; }

    public bool IsPublished { get; set; } = true;
    public DateTime? PublishedAt { get; set; } = DateTime.UtcNow;

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }
    [StringLength(500)]
    public string? MetaDescription { get; set; }
    [StringLength(500)]
    public string? MetaKeywords { get; set; }
}
