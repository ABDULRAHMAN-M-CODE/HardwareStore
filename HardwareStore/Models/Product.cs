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
        public int CategoryId { get; set; } //FK_Categories
        public float VAT { get; set; }
        public int UnitId { get; set; } // FK_Units
        public int SupplierId { get; set; } // part of FK_BrandsSuppliers
        public Unit Unit { get; set; } = null!; // Question to qutiba: Difference between optional and required  one to many
        public int BrandId { get; set; } // part of FK_BrandsSuppliers

        public Category Category { get; set; } = null!;
    }
}
