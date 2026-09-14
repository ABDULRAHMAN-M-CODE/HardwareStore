namespace HardwareStore.Models
{
    public class ProductUnit
    {
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;


        public bool Equals(ProductUnit other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.UnitId == this.UnitId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductUnit);
        public override int GetHashCode() => ( ProductId,UnitId ).GetHashCode();



    }
}
