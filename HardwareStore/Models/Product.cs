using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class Product:Timestampable,IHasIdentification,IHasEnglishAndArabicName
    {
        public int Id { get; set; } // Primary key, also Idenitity.
        public string? SKU { get; set; }
        public  string? Barcode{ get; set; }
        public string? Manufacturer{ get; set; }
        public string? EnglishName { get; set; } 
        public string? ArabicName { get; set; }
        public float MinStock { get; set; }
        public float ReorderQty { get; set; }
        public string? Bin { get; set; }
        public string? Description { get; set; } // Note: string is by default nvarchar, not varchar.
        public string? Status { get; set; }
        public float VAT { get; set; }
        public int CategoryId { get; set; } //FK_Categories
        public int UnitId { get; set; } // FK_Units
        


        public Category Category { get; set; } = null!;
        public Unit Unit { get; set; } = null!; 
        public IEnumerable< ProductSupplier> ProductSuppliers { get; } = new List<ProductSupplier>(); // for Junction table
        public IEnumerable<ProductBrand> ProductBrands { get; } = new List<ProductBrand>();// for junction table
        public IEnumerable<ProductCountry> ProductCountries { get; } = new List<ProductCountry>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations

       
        
    }
}
