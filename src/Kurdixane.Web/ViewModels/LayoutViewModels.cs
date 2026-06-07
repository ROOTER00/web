using System.Collections.Generic;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.ViewModels;

public class HeaderViewModel
{
    public List<MenuItem> Menu { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int CartItemCount { get; set; }
    public Dictionary<string, string?> Settings { get; set; } = new();
}

public class FooterViewModel
{
    public List<MenuItem> Menu { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public Dictionary<string, string?> Settings { get; set; } = new();
}
