using HardwareStore.Services;
using System.ComponentModel;

namespace HardwareStore.Models
{
    public class BrandSupplier
        
    {
       
        // THe following 2 properties and 2 navigation properties  are needed to for the bridge table  between Brands and Suppliers
        public int BrandId { get; set; } 
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public Brand Brand { get; set; } = null!;


        public bool Equals(BrandSupplier other)
        {
            if (other is null)
                return false;

            return 
                (other.BrandId == this.BrandId)
                 &&
                (other.SupplierId==this.SupplierId);
        }

        public override bool Equals(object? obj) => Equals(obj as BrandSupplier);
        public override int GetHashCode() => (BrandId,SupplierId).GetHashCode();

    }
}
