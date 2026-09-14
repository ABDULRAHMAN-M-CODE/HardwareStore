using HardwareStore.SeedWork;
using HardwareStore.Services;
namespace HardwareStore.Models
{
    public class Bin: NonJunctionEntity<Bin> , IHasEnglishAndArabicName, IHasIdentification
    {

        public int Id { get; set; }// surrogote key

        public string? ArabicName { get; set; }

        public IEnumerable<ProductBin> ProductBins { get; } = new List<ProductBin>();

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;

  
            



    }

}
