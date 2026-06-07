using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public enum MenuLocation
{
    Header = 0,
    Footer = 1
}

public class MenuItem : BaseEntity
{
    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Url { get; set; } = "#";

    public int? ParentId { get; set; }
    public MenuItem? Parent { get; set; }
    public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();

    public MenuLocation Location { get; set; } = MenuLocation.Header;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
