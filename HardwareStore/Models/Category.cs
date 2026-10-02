
namespace HardwareStore.Models
{
    using HardwareStore.SeedWork;

    public class Category: NonJunctionEntity<Category>, IHasEnglishAndArabicName,IHasIdentification, IParentEntity,ICreator<Category>
    {


        #region interface(s) methods
        public static Category  Create()
        {
            return new  Category();
        }
        #endregion

        #region properties and fields

        public string? ArabicName { get; set; }

        #endregion

        #region navigation properties

        public IEnumerable<SubCategory> SubCategories { get; }=new List<SubCategory>();
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        public IEnumerable<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();

        #endregion
    }
}
