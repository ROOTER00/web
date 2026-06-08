using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

/// <summary>
/// Generic key/value store for editable system settings
/// (site title, logo, contact info, social links, SEO defaults...).
/// </summary>
public class SiteSetting : BaseEntity
{
    [Required, StringLength(150)]
    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(80)]
    public string? Group { get; set; }
}
