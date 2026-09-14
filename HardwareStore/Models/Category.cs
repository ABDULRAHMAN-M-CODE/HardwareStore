
namespace HardwareStore.Models
{
    using HardwareStore.Services;
    using HardwareStore.SeedWork;

    public class Category: NonJunctionEntity<Category>, IHasEnglishAndArabicName,IHasIdentification
    {
     
        
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

        public string? ArabicName { get; set; }     
        public IEnumerable<SubCategory> SubCategories { get; }=new List<SubCategory>();
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        public IEnumerable<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
    }
}
