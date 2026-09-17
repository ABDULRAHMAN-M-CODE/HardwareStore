namespace HardwareStore.Models
{
    public class ProductSupplier

    {
       public int ProductId { get; set; }
       public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public Product Product { get; set; } = null!;


        public bool Equals(ProductSupplier other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.SupplierId == this.SupplierId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductSupplier);
        public override int GetHashCode() => (ProductId ,SupplierId ).GetHashCode();



    }
}
