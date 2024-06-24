using System.Threading.Tasks;
using Models;

public interface IProductService
{
    Task<BramProduct> GetProductAsync(int productId);
}
