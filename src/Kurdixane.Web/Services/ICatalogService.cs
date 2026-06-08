using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.Services;

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => PageSize <= 0 ? 0 : (int)System.Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}

public interface ICatalogService
{
    // Categories
    Task<List<Category>> GetActiveCategoriesAsync();
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryBySlugAsync(string slug);
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> SaveCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);

    // Products
    Task<PagedResult<Product>> GetProductsAsync(string? categorySlug = null, string? search = null, int page = 1, int pageSize = 12);
    Task<List<Product>> GetFeaturedProductsAsync(int take = 8);
    Task<List<Product>> GetLatestProductsAsync(int take = 8);
    Task<Product?> GetProductBySlugAsync(string slug);
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetRelatedProductsAsync(int productId, int categoryId, int take = 4);
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> SaveProductAsync(Product product);
    Task DeleteProductAsync(int id);
}
