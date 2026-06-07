using System.Threading.Tasks;
using Kurdixane.Web.ViewModels;

namespace Kurdixane.Web.Services;

public interface ICartService
{
    CartViewModel GetCart();
    Task AddAsync(int productId, int quantity = 1);
    void UpdateQuantity(int productId, int quantity);
    void Remove(int productId);
    void Clear();
}
