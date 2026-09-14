namespace HardwareStore.Models
{
    public class ProductCountry
    {
        public int ProductId { get; set; }
        public int CountryId { get; set; }

        public Product Product { get; set; } = null!; // following documentation examples
        public Country Country { get; set; } = null!;// following documentation examples


        public bool Equals(ProductCountry other)
        {
            if (other is null)
                return false;
            return
                (other.ProductId == this.ProductId)
                 &&
                 (other.CountryId == this.CountryId);
        }

        public override bool Equals(object? obj) => Equals(obj as ProductCountry);
        public override int GetHashCode() => ( ProductId,CountryId ).GetHashCode();



    }
}
