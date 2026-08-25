namespace HardwareStore.Models
{
    public class Unit:Timestampable
    {
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        
        public string? ArabicName { get; set; }


        // IEnumerable<T>, which provides a read-only view that cannot be randomly modified by application code
       
        public IEnumerable<Product> Products{ get; } = new List<Product>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
    }
}
