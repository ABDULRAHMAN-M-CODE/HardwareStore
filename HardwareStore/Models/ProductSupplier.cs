namespace HardwareStore.Models
{
    public class ProductSupplier

    {
       public int ProductId { get; set; }
       public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
