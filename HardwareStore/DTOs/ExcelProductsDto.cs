namespace HardwareStore.DTOs
{
    using HardwareStore.Models;
    public class ExcelProductsDto:IDataDto
    {

        public required List<Category> Categories;
        public required List<Country> Countries;
        public required List<Unit> Units;
        public required List<Brand> Brands;
        public required List<Supplier> Suppliers;
        public required List<Product> Products;
        public required List<Bin> Bins;
        public required List<Manufacturer> Manufacturers;
        public required List<SubCategory> SubCategories;
    }
}
