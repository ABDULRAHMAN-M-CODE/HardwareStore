
namespace HardwareStore.Models
{
    using HardwareStore.SeedWork;
    public class Product:NonJunctionEntity<Product>,IHasIdentification,IHasEnglishAndArabicName, IParentEntity
    {
        public int Id { get; set; } // Primary key, also Idenitity.
        public string? SKU { get; set; }
        public  string? Barcode{ get; set; }
        public string? ArabicName { get; set; }
        public string? Description { get; set; } 
        public string? Status { get; set; }
        public int MinStock { get; set; }
        public int VAT { get; set; }
        public int ReorderQTY { get; set; }
        public float Price { get; set; }

        public IEnumerable< ProductSupplier> ProductSuppliers { get; } = new List<ProductSupplier>(); // for Junction table
        public IEnumerable<ProductBrand> ProductBrands { get; } = new List<ProductBrand>();// for junction table
        public IEnumerable<ProductCountry> ProductCountries { get; } = new List<ProductCountry>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
        public ApplicationUser Creator { get; set; } = null!;
        public ApplicationUser Deleter { get; set; } = null!;
        public ApplicationUser Updater { get; set; } = null!;
        public IEnumerable<ProductBin> ProductBins { get; } = new List<ProductBin>();
        public IEnumerable<ProductCategory> ProductCategories { get; } = new List<ProductCategory>(); // for Junction table
        public IEnumerable<ProductUnit> ProductUnits { get; } = new List<ProductUnit>(); // for Junction table

        public IEnumerable<ProductManufacturer> ProductManufacturers { get; } = new List<ProductManufacturer>();
    }
}
