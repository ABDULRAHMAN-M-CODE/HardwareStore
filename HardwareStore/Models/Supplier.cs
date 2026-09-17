using Azure;
using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class Supplier:NonJunctionEntity<Supplier>,IHasEnglishAndArabicName,IHasIdentification, IParentEntity
    {
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        
        public string? ArabicName { get; set; }
        

        public IEnumerable<ProductSupplier> ProductSuppliers { get; } = new List<ProductSupplier>();
        public IEnumerable<BrandSupplier>BrandSuppliers { get; } = new List<BrandSupplier>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;


    }
}
