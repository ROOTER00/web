using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kurdixane.Web.Data;

/// <summary>
/// Applies pending migrations and seeds baseline content on first run.
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await db.Database.MigrateAsync();
        await SeedAsync(db, hasher);
    }

    private static async Task SeedAsync(ApplicationDbContext db, IPasswordHasher hasher)
    {
        // ---- Admin user ----
        if (!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                FullName = "Sistem Yöneticisi",
                Email = "admin@kurdixane.com",
                PasswordHash = hasher.Hash("Admin123!"),
                Role = UserRole.Admin,
                IsActive = true
            });
            await db.SaveChangesAsync();
        }

        // ---- Settings ----
        if (!await db.SiteSettings.AnyAsync())
        {
            db.SiteSettings.AddRange(
                new SiteSetting { Key = "SiteTitle", Value = "Kurdixane", Group = "Genel", Description = "Site başlığı" },
                new SiteSetting { Key = "SiteSlogan", Value = "Geleneksel lezzetler, modern dokunuş", Group = "Genel" },
                new SiteSetting { Key = "ContactEmail", Value = "info@kurdixane.com", Group = "İletişim" },
                new SiteSetting { Key = "ContactPhone", Value = "+90 555 000 00 00", Group = "İletişim" },
                new SiteSetting { Key = "ContactAddress", Value = "Diyarbakır, Türkiye", Group = "İletişim" },
                new SiteSetting { Key = "FooterText", Value = "© 2026 Kurdixane. Tüm hakları saklıdır.", Group = "Footer" },
                new SiteSetting { Key = "FacebookUrl", Value = "#", Group = "Sosyal" },
                new SiteSetting { Key = "InstagramUrl", Value = "#", Group = "Sosyal" },
                new SiteSetting { Key = "TwitterUrl", Value = "#", Group = "Sosyal" },
                new SiteSetting { Key = "MetaDescription", Value = "Kurdixane - kaliteli ürünler online mağaza", Group = "SEO" },
                new SiteSetting { Key = "MetaKeywords", Value = "kurdixane, e-ticaret, online alışveriş", Group = "SEO" }
            );
            await db.SaveChangesAsync();
        }

        // ---- Categories ----
        if (!await db.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Gıda Ürünleri", Slug = "gida-urunleri", DisplayOrder = 1, IsActive = true, Description = "Doğal ve organik gıda ürünleri" },
                new() { Name = "Baharatlar", Slug = "baharatlar", DisplayOrder = 2, IsActive = true, Description = "Geleneksel baharat çeşitleri" },
                new() { Name = "Kuru Gıda", Slug = "kuru-gida", DisplayOrder = 3, IsActive = true, Description = "Kuruyemiş ve kurutulmuş ürünler" },
                new() { Name = "İçecekler", Slug = "icecekler", DisplayOrder = 4, IsActive = true, Description = "Geleneksel içecekler" }
            };
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync();

            // ---- Products ----
            var rnd = new Random(42);
            var sampleNames = new (string Cat, string[] Items)[]
            {
                ("gida-urunleri", new[] { "Organik Bal", "Köy Peyniri", "Zeytinyağı", "Ev Yapımı Reçel" }),
                ("baharatlar", new[] { "İsot Biberi", "Toz Kırmızı Biber", "Kimyon", "Sumak" }),
                ("kuru-gida", new[] { "Antep Fıstığı", "Çiğ Badem", "Kuru İncir", "Ceviz İçi" }),
                ("icecekler", new[] { "Mırra Kahvesi", "Bitki Çayı", "Nar Ekşisi Şurubu", "Şalgam Suyu" })
            };

            var products = new List<Product>();
            foreach (var group in sampleNames)
            {
                var cat = categories.First(c => c.Slug == group.Cat);
                int order = 0;
                foreach (var name in group.Items)
                {
                    decimal price = rnd.Next(40, 400) + 0.90m;
                    products.Add(new Product
                    {
                        Name = name,
                        Slug = Helpers.SlugHelper.Generate(name),
                        CategoryId = cat.Id,
                        Sku = $"KX-{cat.Id:00}-{++order:00}",
                        ShortDescription = $"{name} - özenle seçilmiş, kaliteli ürün.",
                        Description = $"<p><strong>{name}</strong> doğal yöntemlerle üretilmiştir. Geleneksel lezzeti modern kalite standartlarıyla buluşturuyoruz.</p><ul><li>%100 doğal</li><li>Özenli paketleme</li><li>Hızlı teslimat</li></ul>",
                        Price = price,
                        OldPrice = rnd.Next(0, 2) == 1 ? price + rnd.Next(10, 80) : null,
                        Stock = rnd.Next(5, 100),
                        ImageUrl = $"/img/products/placeholder-{(products.Count % 8) + 1}.svg",
                        IsActive = true,
                        IsFeatured = order <= 2,
                        DisplayOrder = order,
                        MetaTitle = name,
                        MetaDescription = $"{name} satın al - Kurdixane"
                    });
                }
            }
            db.Products.AddRange(products);
            await db.SaveChangesAsync();
        }

        // ---- Sliders ----
        if (!await db.Sliders.AnyAsync())
        {
            db.Sliders.AddRange(
                new Slider { Title = "Geleneksel Lezzetler", Subtitle = "Doğanın en taze ürünleri kapınızda", ImageUrl = "/img/sliders/slide-1.svg", Link = "/Product", ButtonText = "Alışverişe Başla", DisplayOrder = 1, IsActive = true },
                new Slider { Title = "Yöresel Baharatlar", Subtitle = "Mutfağınıza otantik dokunuş", ImageUrl = "/img/sliders/slide-2.svg", Link = "/Category/baharatlar", ButtonText = "Keşfet", DisplayOrder = 2, IsActive = true },
                new Slider { Title = "Doğal Kuruyemişler", Subtitle = "Sağlıklı atıştırmalıklar", ImageUrl = "/img/sliders/slide-3.svg", Link = "/Category/kuru-gida", ButtonText = "İncele", DisplayOrder = 3, IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        // ---- Menu ----
        if (!await db.MenuItems.AnyAsync())
        {
            db.MenuItems.AddRange(
                new MenuItem { Title = "Anasayfa", Url = "/", Location = MenuLocation.Header, DisplayOrder = 1, IsActive = true },
                new MenuItem { Title = "Ürünler", Url = "/Product", Location = MenuLocation.Header, DisplayOrder = 2, IsActive = true },
                new MenuItem { Title = "Blog", Url = "/Blog", Location = MenuLocation.Header, DisplayOrder = 3, IsActive = true },
                new MenuItem { Title = "Hakkımızda", Url = "/Page/hakkimizda", Location = MenuLocation.Header, DisplayOrder = 4, IsActive = true },
                new MenuItem { Title = "İletişim", Url = "/Contact", Location = MenuLocation.Header, DisplayOrder = 5, IsActive = true },
                new MenuItem { Title = "Hakkımızda", Url = "/Page/hakkimizda", Location = MenuLocation.Footer, DisplayOrder = 1, IsActive = true },
                new MenuItem { Title = "İletişim", Url = "/Contact", Location = MenuLocation.Footer, DisplayOrder = 2, IsActive = true },
                new MenuItem { Title = "Gizlilik Politikası", Url = "/Page/gizlilik-politikasi", Location = MenuLocation.Footer, DisplayOrder = 3, IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        // ---- Pages ----
        if (!await db.Pages.AnyAsync())
        {
            db.Pages.AddRange(
                new Page { Title = "Hakkımızda", Slug = "hakkimizda", IsActive = true, Content = "<h2>Hakkımızda</h2><p>Kurdixane, yöresel ve geleneksel ürünleri sizlerle buluşturan bir e-ticaret platformudur. Amacımız, doğal ve kaliteli ürünleri güvenilir biçimde kapınıza ulaştırmaktır.</p>", MetaTitle = "Hakkımızda - Kurdixane" },
                new Page { Title = "Gizlilik Politikası", Slug = "gizlilik-politikasi", IsActive = true, Content = "<h2>Gizlilik Politikası</h2><p>Kişisel verileriniz gizlilik politikamız kapsamında korunmaktadır.</p>", MetaTitle = "Gizlilik Politikası" }
            );
            await db.SaveChangesAsync();
        }

        // ---- Blog ----
        if (!await db.BlogPosts.AnyAsync())
        {
            db.BlogPosts.AddRange(
                new BlogPost { Title = "Doğal Beslenmenin Önemi", Slug = "dogal-beslenmenin-onemi", Author = "Kurdixane", Summary = "Sağlıklı yaşam için doğal ürünlerin faydaları.", Content = "<p>Doğal beslenme, sağlıklı bir yaşamın temelidir. Bu yazımızda doğal ürünlerin faydalarından bahsediyoruz.</p>", ImageUrl = "/img/blog/post-1.svg", IsPublished = true, PublishedAt = DateTime.UtcNow.AddDays(-10) },
                new BlogPost { Title = "Yöresel Baharatların Kullanımı", Slug = "yoresel-baharatlarin-kullanimi", Author = "Kurdixane", Summary = "Mutfağınızda yöresel baharatları nasıl kullanırsınız?", Content = "<p>Yöresel baharatlar yemeklerinize otantik bir lezzet katar.</p>", ImageUrl = "/img/blog/post-2.svg", IsPublished = true, PublishedAt = DateTime.UtcNow.AddDays(-5) },
                new BlogPost { Title = "Kuruyemişlerin Faydaları", Slug = "kuruyemislerin-faydalari", Author = "Kurdixane", Summary = "Kuruyemişler neden sağlıklı bir atıştırmalık?", Content = "<p>Kuruyemişler protein ve sağlıklı yağlar açısından zengindir.</p>", ImageUrl = "/img/blog/post-3.svg", IsPublished = true, PublishedAt = DateTime.UtcNow.AddDays(-2) }
            );
            await db.SaveChangesAsync();
        }
    }
}
