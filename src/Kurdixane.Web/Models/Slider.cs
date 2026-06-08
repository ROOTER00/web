using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public class Slider : BaseEntity
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Subtitle { get; set; }

    [Required, StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Link { get; set; }

    [StringLength(100)]
    public string? ButtonText { get; set; }

    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
