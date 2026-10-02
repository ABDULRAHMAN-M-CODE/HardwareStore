using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class SubCategory : NonJunctionEntity<SubCategory>,IHasEnglishAndArabicName,IHasIdentification,ICreator<SubCategory>
    {
        public static  SubCategory Create()
        {
            return new SubCategory();
        }

        public string? ArabicName { get; set; }       
        public int CategoryId { get; set; } //FK


        public Category Category { get; set; } = null!; // Navigation
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
    }
}
