using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BramProduct> GetProductAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }
}
