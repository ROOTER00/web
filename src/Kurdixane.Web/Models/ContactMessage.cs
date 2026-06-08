using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public class ContactMessage : BaseEntity
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Subject { get; set; }

    [Required, StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }
}
