using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Kurdixane.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Kurdixane.Web.Services;

/// <summary>
/// Session-backed shopping cart. The cart is serialized to a session key
/// as JSON so it survives across requests without DB persistence.
/// </summary>
public class CartService : ICartService
{
    private const string SessionKey = "KURDIXANE_CART";
    private readonly IHttpContextAccessor _accessor;
    private readonly ICatalogService _catalog;

    public CartService(IHttpContextAccessor accessor, ICatalogService catalog)
    {
        _accessor = accessor;
        _catalog = catalog;
    }

    private ISession Session => _accessor.HttpContext!.Session;

    public CartViewModel GetCart()
    {
        var json = Session.GetString(SessionKey);
        if (string.IsNullOrEmpty(json))
            return new CartViewModel();
        return JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
    }

    private void Save(CartViewModel cart)
        => Session.SetString(SessionKey, JsonSerializer.Serialize(cart));

    public async Task AddAsync(int productId, int quantity = 1)
    {
        if (quantity < 1) quantity = 1;
        var cart = GetCart();
        var line = cart.Lines.FirstOrDefault(l => l.ProductId == productId);
        if (line is not null)
        {
            line.Quantity += quantity;
        }
        else
        {
            var product = await _catalog.GetProductByIdAsync(productId);
            if (product is null) return;
            cart.Lines.Add(new CartLine
            {
                ProductId = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price,
                Quantity = quantity
            });
        }
        Save(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var line = cart.Lines.FirstOrDefault(l => l.ProductId == productId);
        if (line is null) return;
        if (quantity <= 0)
            cart.Lines.Remove(line);
        else
            line.Quantity = quantity;
        Save(cart);
    }

    public void Remove(int productId)
    {
        var cart = GetCart();
        var line = cart.Lines.FirstOrDefault(l => l.ProductId == productId);
        if (line is null) return;
        cart.Lines.Remove(line);
        Save(cart);
    }

    public void Clear() => Session.Remove(SessionKey);
}
