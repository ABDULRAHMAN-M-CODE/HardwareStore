using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class Unit:Timestampable,IHasEnglishAndArabicName,IHasIdentification
    {
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.      
        public string? ArabicName { get; set; }
 
        public ApplicationUser User { get; set; } = null!;
        public IEnumerable<ProductUnit> ProductUnits { get; } = new List<ProductUnit>();

    }
}
