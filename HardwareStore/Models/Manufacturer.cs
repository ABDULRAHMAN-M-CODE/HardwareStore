
namespace HardwareStore.Models
{
    using HardwareStore.Services;

    using HardwareStore.SeedWork;
    public class Manufacturer:NonJunctionEntity<Manufacturer>, IHasEnglishAndArabicName, IHasIdentification
    {
        public int Id { get; set; }

        public string? ArabicName { get; set; }

        public IEnumerable<ProductManufacturer> ProductManufacturers { get; } = new List<ProductManufacturer>();

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;

    }
}
