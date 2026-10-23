using HardwareStore.Models;

namespace HardwareStore.Services.ProductsRepositoryServices
{
    public interface IProductsRepository
    {
        public IQueryable<Product> GetAll();
    }
}
