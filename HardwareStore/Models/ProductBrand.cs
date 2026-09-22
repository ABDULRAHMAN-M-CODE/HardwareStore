using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    /// <summary>
    /// Junction table between Products and Brands tables.
    /// </summary>
    public class ProductBrand:IJunctionEntity
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public Product Product { get; set; } = null!;
        public Brand Brand { get; set; } = null!;


        public bool Equals(ProductBrand other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.BrandId == this.BrandId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductBrand);
        public override int GetHashCode() => ( ProductId,BrandId ).GetHashCode();



    }
}
