using HardwareStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HardwareStore.Services.ProductsRepositoryServices
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> GetAll()
        {
            IQueryable<Product> x = _context.Products;
            return x;
        }
        public async Task<bool> IsThereProductsInDb()
        {
            return await _context.Products.AnyAsync();
        }

    }
}
