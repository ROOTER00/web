using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Helpers;
using Kurdixane.Web.Models;
using Kurdixane.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Services;

public class ContentService : IContentService
{
    private readonly IRepository<Slider> _sliders;
    private readonly IRepository<MenuItem> _menu;
    private readonly IRepository<BlogPost> _blog;
    private readonly IRepository<Page> _pages;
    private readonly IRepository<ContactMessage> _contact;

    public ContentService(
        IRepository<Slider> sliders,
        IRepository<MenuItem> menu,
        IRepository<BlogPost> blog,
        IRepository<Page> pages,
        IRepository<ContactMessage> contact)
    {
        _sliders = sliders;
        _menu = menu;
        _blog = blog;
        _pages = pages;
        _contact = contact;
    }

    // ---- Sliders ----
    public async Task<List<Slider>> GetActiveSlidersAsync()
        => await _sliders.Query().Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder).ToListAsync();

    public async Task<List<Slider>> GetAllSlidersAsync()
        => await _sliders.Query().OrderBy(s => s.DisplayOrder).ToListAsync();

    public async Task<Slider?> GetSliderByIdAsync(int id) => await _sliders.GetByIdAsync(id);

    public async Task<Slider> SaveSliderAsync(Slider slider)
    {
        if (slider.Id == 0) await _sliders.AddAsync(slider);
        else _sliders.Update(slider);
        await _sliders.SaveChangesAsync();
        return slider;
    }

    public async Task DeleteSliderAsync(int id)
    {
        var e = await _sliders.GetByIdAsync(id);
        if (e is null) return;
        _sliders.Remove(e);
        await _sliders.SaveChangesAsync();
    }

    // ---- Menu ----
    public async Task<List<MenuItem>> GetMenuAsync(MenuLocation location)
        => await _menu.Query()
            .Include(m => m.Children.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder))
            .Where(m => m.IsActive && m.Location == location && m.ParentId == null)
            .OrderBy(m => m.DisplayOrder).ToListAsync();

    public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        => await _menu.Query().OrderBy(m => m.Location).ThenBy(m => m.DisplayOrder).ToListAsync();

    public async Task<MenuItem?> GetMenuItemByIdAsync(int id) => await _menu.GetByIdAsync(id);

    public async Task<MenuItem> SaveMenuItemAsync(MenuItem item)
    {
        if (item.Id == 0) await _menu.AddAsync(item);
        else _menu.Update(item);
        await _menu.SaveChangesAsync();
        return item;
    }

    public async Task DeleteMenuItemAsync(int id)
    {
        var e = await _menu.GetByIdAsync(id);
        if (e is null) return;
        _menu.Remove(e);
        await _menu.SaveChangesAsync();
    }

    // ---- Blog ----
    public async Task<PagedResult<BlogPost>> GetBlogPostsAsync(int page = 1, int pageSize = 9)
    {
        if (page < 1) page = 1;
        var query = _blog.Query().Where(b => b.IsPublished);
        int total = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<BlogPost>(items, total, page, pageSize);
    }

    public async Task<List<BlogPost>> GetRecentBlogPostsAsync(int take = 3)
        => await _blog.Query().Where(b => b.IsPublished)
            .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt).Take(take).ToListAsync();

    public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        => await _blog.FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);

    public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        => await _blog.Query().OrderByDescending(b => b.Id).ToListAsync();

    public async Task<BlogPost?> GetBlogPostByIdAsync(int id) => await _blog.GetByIdAsync(id);

    public async Task<BlogPost> SaveBlogPostAsync(BlogPost post)
    {
        if (string.IsNullOrWhiteSpace(post.Slug))
            post.Slug = SlugHelper.Generate(post.Title);
        if (post.Id == 0) await _blog.AddAsync(post);
        else _blog.Update(post);
        await _blog.SaveChangesAsync();
        return post;
    }

    public async Task DeleteBlogPostAsync(int id)
    {
        var e = await _blog.GetByIdAsync(id);
        if (e is null) return;
        _blog.Remove(e);
        await _blog.SaveChangesAsync();
    }

    // ---- Pages ----
    public async Task<Page?> GetPageBySlugAsync(string slug)
        => await _pages.FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

    public async Task<List<Page>> GetAllPagesAsync()
        => await _pages.Query().OrderBy(p => p.Title).ToListAsync();

    public async Task<Page?> GetPageByIdAsync(int id) => await _pages.GetByIdAsync(id);

    public async Task<Page> SavePageAsync(Page page)
    {
        if (string.IsNullOrWhiteSpace(page.Slug))
            page.Slug = SlugHelper.Generate(page.Title);
        if (page.Id == 0) await _pages.AddAsync(page);
        else _pages.Update(page);
        await _pages.SaveChangesAsync();
        return page;
    }

    public async Task DeletePageAsync(int id)
    {
        var e = await _pages.GetByIdAsync(id);
        if (e is null) return;
        _pages.Remove(e);
        await _pages.SaveChangesAsync();
    }

    // ---- Contact ----
    public async Task AddContactMessageAsync(ContactMessage message)
    {
        await _contact.AddAsync(message);
        await _contact.SaveChangesAsync();
    }

    public async Task<List<ContactMessage>> GetContactMessagesAsync()
        => await _contact.Query().OrderByDescending(c => c.Id).ToListAsync();

    public async Task<ContactMessage?> GetContactMessageByIdAsync(int id) => await _contact.GetByIdAsync(id);

    public async Task MarkContactMessageReadAsync(int id)
    {
        var e = await _contact.GetByIdAsync(id);
        if (e is null) return;
        e.IsRead = true;
        _contact.Update(e);
        await _contact.SaveChangesAsync();
    }

    public async Task DeleteContactMessageAsync(int id)
    {
        var e = await _contact.GetByIdAsync(id);
        if (e is null) return;
        _contact.Remove(e);
        await _contact.SaveChangesAsync();
    }
}
