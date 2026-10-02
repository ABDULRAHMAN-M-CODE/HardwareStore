using HardwareStore.SeedWork;

namespace HardwareStore.Models
{
    public class Unit:NonJunctionEntity<Unit>,IHasEnglishAndArabicName,IHasIdentification,ICreator<Unit>
    {
        public static Unit Create()
        {
            return new Unit();
        }
        public string? ArabicName { get; set; }

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        public IEnumerable<ProductUnit> ProductUnits { get; } = new List<ProductUnit>();

    }
}
