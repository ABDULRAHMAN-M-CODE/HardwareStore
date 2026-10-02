using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class ProductBin:IJunctionEntity
    {
        public int ProductId { get; set; }
        public int BinId { get; set; }
        public Product Product { get; set; } = null!;
        public Bin Bin { get; set; } = null!;



        public bool Equals(ProductBin other)
        {
            if (other is null)
                return false;

            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.BinId == this.BinId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductBin);
        public override int GetHashCode() => (ProductId,BinId).GetHashCode();



    }
}
