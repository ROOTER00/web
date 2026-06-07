using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly IRepository<Category> _categories;
    private readonly IRepository<Product> _products;

    public CatalogService(IRepository<Category> categories, IRepository<Product> products)
    {
        _categories = categories;
        _products = products;
    }

    // ---- Categories ----
    public async Task<List<Category>> GetActiveCategoriesAsync()
        => await _categories.Query()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
            .ToListAsync();

    public async Task<List<Category>> GetAllCategoriesAsync()
        => await _categories.Query()
            .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
            .ToListAsync();

    public async Task<Category?> GetCategoryBySlugAsync(string slug)
        => await _categories.FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

    public async Task<Category?> GetCategoryByIdAsync(int id)
        => await _categories.GetByIdAsync(id);

    public async Task<Category> SaveCategoryAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Slug))
            category.Slug = SlugHelper.Generate(category.Name);

        if (category.Id == 0)
            await _categories.AddAsync(category);
        else
            _categories.Update(category);

        await _categories.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var entity = await _categories.GetByIdAsync(id);
        if (entity is null) return;
        _categories.Remove(entity);
        await _categories.SaveChangesAsync();
    }

    // ---- Products ----
    public async Task<PagedResult<Product>> GetProductsAsync(string? categorySlug = null, string? search = null, int page = 1, int pageSize = 12)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 12;

        var query = _products.Query()
            .Include(p => p.Category)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(categorySlug))
            query = query.Where(p => p.Category != null && p.Category.Slug == categorySlug);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || (p.ShortDescription != null && p.ShortDescription.Contains(search)));

        int total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product>(items, total, page, pageSize);
    }

    public async Task<List<Product>> GetFeaturedProductsAsync(int take = 8)
        => await _products.Query()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.Id)
            .Take(take).ToListAsync();

    public async Task<List<Product>> GetLatestProductsAsync(int take = 8)
        => await _products.Query()
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Id)
            .Take(take).ToListAsync();

    public async Task<Product?> GetProductBySlugAsync(string slug)
        => await _products.Query()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

    public async Task<Product?> GetProductByIdAsync(int id)
        => await _products.Query(tracking: true)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Product>> GetRelatedProductsAsync(int productId, int categoryId, int take = 4)
        => await _products.Query()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.CategoryId == categoryId && p.Id != productId)
            .OrderByDescending(p => p.Id)
            .Take(take).ToListAsync();

    public async Task<List<Product>> GetAllProductsAsync()
        => await _products.Query()
            .Include(p => p.Category)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

    public async Task<Product> SaveProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Slug))
            product.Slug = SlugHelper.Generate(product.Name);

        if (product.Id == 0)
            await _products.AddAsync(product);
        else
            _products.Update(product);

        await _products.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(int id)
    {
        var entity = await _products.GetByIdAsync(id);
        if (entity is null) return;
        _products.Remove(entity);
        await _products.SaveChangesAsync();
    }
}
