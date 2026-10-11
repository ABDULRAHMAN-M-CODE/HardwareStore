namespace HardwareStore.DTOs
{
    using HardwareStore.Models;
    public class NonJunctionModelsSnapShot: IData
    {

        public  List<Category> Categories; // data to be written to the `Categories` table, which is a parent table
        public  List<Country> Countries;  // data to be written to the `Countries` table, which is a parent table
        public  List<Unit> Units;          // data to be written to the `Units` table, which is a parent table
        public  List<Brand> Brands;         // data to be written to the `Brands` table , which is a parent table
        public  List<Supplier> Suppliers;   // data to be written to the `Suppliers` table, which is a parent table
        public  List<Product> Products;     // data to be written to the `Products` table, which is a parent table
        public  List<Bin> Bins;             // data to be written to the `Bins` table, which is a parent table
        public  List<Manufacturer> Manufacturers; // data to be written to the `Manufacturers` table ,which is a parent table
        public  List<SubCategory> SubCategories;  // data to be written to the `Subcategories` table ,which is a child table that depends on `Categories`
    }
}
