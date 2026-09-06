using HardwareStore.Services;
namespace HardwareStore.Models
{
    public class Bin: Timestampable, IHasEnglishAndArabicName, IHasIdentification
    {

        public int Id { get; set; }
        public string? EnglishName { get; set; }
        public string? ArabicName { get; set; }

        public IEnumerable<ProductBin> ProductBins { get; } = new List<ProductBin>();

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;

    }
}
