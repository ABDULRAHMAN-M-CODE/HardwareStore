namespace HardwareStore.Models
{
    public class ProductUnit
    {
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;
    }
}
