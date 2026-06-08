using System.Collections.Generic;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;

namespace Kurdixane.Web.ViewModels;

public class HomeViewModel
{
    public List<Slider> Sliders { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Product> LatestProducts { get; set; } = new();
    public List<BlogPost> RecentPosts { get; set; } = new();
}

public class ProductListViewModel
{
    public PagedResult<Product> Products { get; set; } = new(new List<Product>(), 0, 1, 12);
    public List<Category> Categories { get; set; } = new();
    public Category? CurrentCategory { get; set; }
    public string? Search { get; set; }
    public string? CategorySlug { get; set; }
}

public class ProductDetailViewModel
{
    public Product Product { get; set; } = null!;
    public List<Product> RelatedProducts { get; set; } = new();
}

public class BlogListViewModel
{
    public PagedResult<BlogPost> Posts { get; set; } = new(new List<BlogPost>(), 0, 1, 9);
    public List<BlogPost> RecentPosts { get; set; } = new();
}
