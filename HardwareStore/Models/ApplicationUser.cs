using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        ///////////////////////////////////////////////////////////
        // additional columns besides the ones that are inherited from IdentityUser
        ///////////////////////////////////////////////////////////
        public DateTime RegistrationDate { get; set; }



        ////////////////////////////////////////////////////////////
        //Naviagation properties: For Auditing
        ///////////////////////////////////////////////////////////
        public IEnumerable<Brand>CreatedBrands { get; } = new List<Brand>();
        public IEnumerable<Brand> UpdatedBrands { get; } = new List<Brand>();
        public IEnumerable<Brand> DeletedBrands { get; } = new List<Brand>();
        
        public IEnumerable<Category> CreatedCategories { get; } = new List<Category>();
        public IEnumerable<Category> UpdatedCategories { get; } = new List<Category>();
        public IEnumerable<Category> DeletedCategories { get; } = new List<Category>();
        

        
        public IEnumerable<Product> CreatedProducts { get; } = new List<Product>();
        public IEnumerable<Product> DeletedProducts { get; } = new List<Product>();
        public IEnumerable<Product> UpdatedProducts { get; } = new List<Product>();

        public IEnumerable<SubCategory> CreatedSubCategories { get; } = new List<SubCategory>();
        public IEnumerable<SubCategory> UpdatedSubCategories { get; } = new List<SubCategory>();
        public IEnumerable<SubCategory> DeletedSubCategories { get; } = new List<SubCategory>();
        
        public IEnumerable<Supplier> CreatedSuppliers { get; } = new List<Supplier>();
        public IEnumerable<Supplier> UpdatedSuppliers { get; } = new List<Supplier>();
        public IEnumerable<Supplier> DeletedSuppliers { get; } = new List<Supplier>();

        public IEnumerable<Unit> CreatedUnits { get; } = new List<Unit>();
        public IEnumerable<Unit> UpdatedUnits { get; } = new List<Unit>();
        public IEnumerable<Unit> DeletedUnits { get; } = new List<Unit>();

        public IEnumerable<Country> CreatedCountries { get; } = new List<Country>();
        public IEnumerable<Country> UpdatedCountries { get; } = new List<Country>();
        public IEnumerable<Country> DeletedCountries { get; } = new List<Country>();


        public IEnumerable<Manufacturer> CreatedManufacturers { get; } = new List<Manufacturer>();
        public IEnumerable<Manufacturer> UpdatedManufacturers { get; } = new List<Manufacturer>();
        public IEnumerable<Manufacturer> DeletedManufacturers { get; } = new List<Manufacturer>();

        public IEnumerable<Bin> CreatedBins { get; } = new List<Bin>();
        public IEnumerable<Bin> UpdatedBins { get; } = new List<Bin>();
        public IEnumerable<Bin> DeletedBins { get; } = new List<Bin>();
    }


}
