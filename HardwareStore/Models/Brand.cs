
using HardwareStore.SeedWork;


namespace HardwareStore.Models
{
    public class Brand: NonJunctionEntity<Brand>,IHasEnglishAndArabicName,IHasIdentification,ICreator<Brand>
    {

        public static Brand Create ()
        {
            return new Brand();
        }

        public string? ArabicName { get; set; }

        
        public IEnumerable<BrandSupplier> BrandSuppliers { get; } = new List<BrandSupplier>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
        public IEnumerable<ProductBrand> ProductBrands { get; } = new List<ProductBrand>();
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
 
    }
}
