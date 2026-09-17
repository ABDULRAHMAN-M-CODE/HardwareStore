using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class Unit:NonJunctionEntity<Unit>,IHasEnglishAndArabicName,IHasIdentification, IParentEntity
    {

        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.      
        public string? ArabicName { get; set; }

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        public IEnumerable<ProductUnit> ProductUnits { get; } = new List<ProductUnit>();

    }
}
