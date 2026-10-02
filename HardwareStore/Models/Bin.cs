using HardwareStore.SeedWork;
namespace HardwareStore.Models
{

    
    public class Bin: NonJunctionEntity<Bin> ,IHasEnglishAndArabicName, IHasIdentification,ICreator<Bin>
    {

        public static Bin Create()
        {
            return new Bin();
        }

        public string? ArabicName { get; set; }

        public IEnumerable<ProductBin> ProductBins { get; } = new List<ProductBin>();

        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;

  
            



    }

}
