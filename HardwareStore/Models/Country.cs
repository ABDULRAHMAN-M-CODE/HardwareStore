
namespace HardwareStore.Models
{
    using HardwareStore.SeedWork;
    public class Country: NonJunctionEntity<Country>,IHasEnglishAndArabicName,IHasIdentification,ICreator<Country>
    {

        public static Country Create()
        {
            return new Country();
        }
        public string? ArabicName { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.


        #region navigation properties
        public IEnumerable<ProductCountry> ProductCountries { get; } = new List<ProductCountry>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        #endregion
    }
}
