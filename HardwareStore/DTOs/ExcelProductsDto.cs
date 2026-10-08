namespace HardwareStore.DTOs
{
    using HardwareStore.Models;
    public class NonJunctionModelsSnapShot: IDataDto
    {

        public required List<Category> Categories; // data to be written to the `Categories` table, which is a parent table
        public required List<Country> Countries;  // data to be written to the `Countries` table, which is a parent table
        public required List<Unit> Units;          // data to be written to the `Units` table, which is a parent table
        public required List<Brand> Brands;         // data to be written to the `Brands` table , which is a parent table
        public required List<Supplier> Suppliers;   // data to be written to the `Suppliers` table, which is a parent table
        public required List<Product> Products;     // data to be written to the `Products` table, which is a parent table
        public required List<Bin> Bins;             // data to be written to the `Bins` table, which is a parent table
        public required List<Manufacturer> Manufacturers; // data to be written to the `Manufacturers` table ,which is a parent table
        public required List<SubCategory> SubCategories;  // data to be written to the `Subcategories` table ,which is a child table that depends on `Categories`
    }
}
