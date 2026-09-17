namespace HardwareStore.Models
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;


        public bool Equals(ProductCategory other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.CategoryId == this.CategoryId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductCategory);
        public override int GetHashCode() => ( ProductId,CategoryId ).GetHashCode();



    }
}
