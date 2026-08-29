namespace HardwareStore.Models
{
    /// <summary>
    /// Junction table between Products and Brands tables.
    /// </summary>
    public class ProductBrand
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public Product Product { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
    }
}
