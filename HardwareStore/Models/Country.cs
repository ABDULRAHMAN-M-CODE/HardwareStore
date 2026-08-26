using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class Country:Timestampable,IHasEnglishAndArabicName
    {
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

        public string? ArabicName { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

       

        public IEnumerable<ProductCountry> ProductCountries { get; } = new List<ProductCountry>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
    }
}
