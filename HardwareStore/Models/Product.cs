namespace HardwareStore.Models
{
    public class Product
    {
        public string? SKU { get; set; }
        public  string? Barcode{ get; set; }
        public string? Manufatcurer { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeleatedAt { get; set; }
        public string? EnglishName { get; set; } 
        public string? ArabicName { get; set; }
        public float MinStock { get; set; }
        public float ReorderQty { get; set; }
        public string? Bin { get; set; }
        public string? Description { get; set; } // Note: string is by default nvarchar, not varchar.

        public string? Status { get; set; }

        public int Id { get; set; } // Primary key, also Idenitity.
        public float VAT { get; set; }

        public int CategoryId { get; set; } //FK_Categories
        public Category Category { get; set; } = null!;
        

        public int UnitId { get; set; } // FK_Units
        public Unit Unit { get; set; } = null!; // Question to qutiba: Difference between optional and required  one to many relation
      

        public int SupplierId { get; set; } // part of FK_BrandsSuppliers
        public int BrandId { get; set; } // part of FK_BrandsSuppliers
        
        // Number of Foregin keys is 3, but the number of navigation properties is 4.
        public BrandSupplier BrandSupplier { get; set; } = null!;
        public IEnumerable<ProductCountry> ProductCountries { get; } = new List<ProductCountry>(); // documentation link : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations

        public Supplier Supplier { get; set; } = null!; //should I add this?
        public Brand Brand { get; set; } = null!; // should i add this ?
    }
}
