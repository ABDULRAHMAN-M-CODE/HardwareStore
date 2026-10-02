using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class ProductManufacturer:IJunctionEntity
    {
        public int ProductId { get; set; }
        public int ManufacturerId { get; set; }
        public Product Product { get; set; } = null!;
        public Manufacturer Manufacturer { get; set; } = null!;


        public bool Equals(ProductManufacturer other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.ManufacturerId == this.ManufacturerId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductManufacturer);
        public override int GetHashCode() => (ProductId ,ManufacturerId ).GetHashCode();



    }
}
