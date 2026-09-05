using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class Manufacturer: Timestampable, IHasEnglishAndArabicName, IHasIdentification
    {
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? ArabicName { get; set; }

        public IEnumerable<ProductManufacturer> ProductManufacturers { get; } = new List<ProductManufacturer>();


    }
}
