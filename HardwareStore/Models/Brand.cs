using Azure;

namespace HardwareStore.Models
{
    public class Brand: Timestampable
    {
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
       
        public string? ArabicName { get; set; }

        public IEnumerable<BrandSupplier> BrandSuppliers { get; } = new List<BrandSupplier>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
        public IEnumerable<Product> Products { get; } = new List<Product>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
    }
}
