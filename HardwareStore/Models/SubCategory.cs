using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class SubCategory : NonJunctionEntity<SubCategory>,IHasEnglishAndArabicName
    {
        
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

        public string? ArabicName { get; set; }       
        public int CategoryId { get; set; } //FK


        public Category Category { get; set; } = null!; // Navigation
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
    }
}
