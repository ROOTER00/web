using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

/// <summary>
/// CMS-managed dynamic content page (e.g. About, custom landing pages).
/// </summary>
public class Page : BaseEntity
{
    [Required, StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(270)]
    public string Slug { get; set; } = string.Empty;

    // Rich HTML content edited via CKEditor.
    public string? Content { get; set; }

    public bool IsActive { get; set; } = true;

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }
    [StringLength(500)]
    public string? MetaDescription { get; set; }
    [StringLength(500)]
    public string? MetaKeywords { get; set; }
}
