namespace HardwareStore.Models
{
    public class ProductBin
    {
        public int ProductId { get; set; }
        public int BinId { get; set; }
        public Product Product { get; set; } = null!;
        public Bin Bin { get; set; } = null!;
    }
}
