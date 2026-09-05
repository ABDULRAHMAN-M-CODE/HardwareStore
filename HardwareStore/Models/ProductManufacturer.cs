namespace HardwareStore.Models
{
    public class ProductManufacturer
    {
        public int ProductId { get; set; }
        public int ManufacturerId { get; set; }
        public Product Product { get; set; } = null!;
        public Manufacturer Manufacturer { get; set; } = null!;
    }
}
