namespace HardwareStore.Models
{
    public class ProductCountry
    {
        public int ProductId { get; set; }
        public int CountryId { get; set; }

        public Product Product { get; set; } = null!; // following documentation examples
        public Country Country { get; set; } = null!;// following documentation examples
    }
}
