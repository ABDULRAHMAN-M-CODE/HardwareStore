namespace HardwareStore.Models
{
    public class BrandSupplier
        
    {
       
        // THe following 4  lines of code are needed to for the bridge table it self 
        public int BrandId { get; set; } 
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;
        public Brand Brand { get; set; } = null!;


        
        // relation between the bridge table and products, this relation is not required to create the bridge table it self.
        public IEnumerable<Product> Products { get; } = new List<Product>(); 

        
    }
}
