using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.Services;

public interface IContentService
{
    // Sliders
    Task<List<Slider>> GetActiveSlidersAsync();
    Task<List<Slider>> GetAllSlidersAsync();
    Task<Slider?> GetSliderByIdAsync(int id);
    Task<Slider> SaveSliderAsync(Slider slider);
    Task DeleteSliderAsync(int id);

    // Menu
    Task<List<MenuItem>> GetMenuAsync(MenuLocation location);
    Task<List<MenuItem>> GetAllMenuItemsAsync();
    Task<MenuItem?> GetMenuItemByIdAsync(int id);
    Task<MenuItem> SaveMenuItemAsync(MenuItem item);
    Task DeleteMenuItemAsync(int id);

    // Blog
    Task<PagedResult<BlogPost>> GetBlogPostsAsync(int page = 1, int pageSize = 9);
    Task<List<BlogPost>> GetRecentBlogPostsAsync(int take = 3);
    Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
    Task<List<BlogPost>> GetAllBlogPostsAsync();
    Task<BlogPost?> GetBlogPostByIdAsync(int id);
    Task<BlogPost> SaveBlogPostAsync(BlogPost post);
    Task DeleteBlogPostAsync(int id);

    // Pages
    Task<Page?> GetPageBySlugAsync(string slug);
    Task<List<Page>> GetAllPagesAsync();
    Task<Page?> GetPageByIdAsync(int id);
    Task<Page> SavePageAsync(Page page);
    Task DeletePageAsync(int id);

    // Contact messages
    Task AddContactMessageAsync(ContactMessage message);
    Task<List<ContactMessage>> GetContactMessagesAsync();
    Task<ContactMessage?> GetContactMessageByIdAsync(int id);
    Task MarkContactMessageReadAsync(int id);
    Task DeleteContactMessageAsync(int id);
}
