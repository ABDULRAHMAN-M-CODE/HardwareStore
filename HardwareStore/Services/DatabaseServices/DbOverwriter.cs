using HardwareStore.DTOs;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStoreNameSpace;
using Microsoft.EntityFrameworkCore;


using System.Reflection;


namespace HardwareStore.Services.DatabaseServices

//Note for myself:  Use `PROBLEM:` to remind your self to back to something later.
{
    public class DbOverwriter : ISqlServerWriter
    {
        private readonly ApplicationDbContext _context;


        public DbOverwriter(ApplicationDbContext context)
        {

            _context = context;
        }

        #region Write(s) method(s)
        public async Task Write(IDataDto dataDto)
        {
            var incomingData = (NonJunctionModelsSnapShot)dataDto; // contains alot of duplicated data

            #region delete all data from the database

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXECUTE uspDeleteAllDataFromDatabase"
            );

            #endregion

            #region deduplicate parents
            var uniqueSuppliers = incomingData.Suppliers.DistinctBy(supplier => new { supplier.EnglishName });
            var uniqueBrands = incomingData.Brands.DistinctBy(brand => new { brand.EnglishName });
            var uniqueUnits = incomingData.Units.DistinctBy(unit => new { unit.EnglishName });
            var uniqueCategories = incomingData.Categories.DistinctBy(category => new { category.EnglishName });
            var uniqueCountries = incomingData.Countries.DistinctBy(country => new { country.EnglishName });
            var uniqueProducts = incomingData.Products.DistinctBy(product => new { product.EnglishName });
            var uniqueBins = incomingData.Bins.DistinctBy(bin => new { bin.EnglishName });
            var uniqueManufacturers = incomingData.Manufacturers.DistinctBy(manufacturer => new { manufacturer.EnglishName });
            #endregion

            #region persisting non-child tables (child tables cannot exist without parent tables)




            await _context.AddRangeAsync(uniqueSuppliers);
            await _context.AddRangeAsync(uniqueBrands);
            await _context.AddRangeAsync(uniqueUnits);
            await _context.AddRangeAsync(uniqueCategories);
            await _context.AddRangeAsync(uniqueCountries);
            await _context.AddRangeAsync(uniqueProducts);
            await _context.AddRangeAsync(uniqueBins);
            await _context.AddRangeAsync(uniqueManufacturers);


            await _context.SaveChangesAsync();





            #endregion

            #region  To be moved to `TwoListsOperations` service class.
            PopulateIdProperty<Category>(incomingData.Categories, uniqueCategories.ToList());
            PopulateIdProperty<Product>(incomingData.Products, uniqueProducts.ToList());
            PopulateIdProperty<Bin>(incomingData.Bins, uniqueBins.ToList());
            PopulateIdProperty<Brand>(incomingData.Brands, uniqueBrands.ToList());
            PopulateIdProperty<Supplier>(incomingData.Suppliers, uniqueSuppliers.ToList());
            PopulateIdProperty<Country>(incomingData.Countries, uniqueCountries.ToList());
            PopulateIdProperty<Manufacturer>(incomingData.Manufacturers, uniqueManufacturers.ToList());
            PopulateIdProperty<Unit>(incomingData.Units, uniqueUnits.ToList());
            #endregion

            #region Persisting non-junction  child tables




            var zip = incomingData.SubCategories.Zip(incomingData.Categories, (sc, c) => new { sc, c.EnglishName });
            Dictionary<string, Category> categoriesLookup = uniqueCategories.ToDictionary(c => c.EnglishName, c => c);
            foreach (var a in zip)
            {
                a.sc.CategoryId = categoriesLookup[a.EnglishName].Id;

            }

            List<SubCategory> uniqueSubCategories = zip.Select(x => x.sc).DistinctBy(subCategory => new { subCategory.EnglishName }).ToList();

            #endregion

            #region Populating junction tables (a junction table contain two foreign keys)

            var castedBrands = incomingData.Brands.Cast<INonJunctionEntity>().ToList();
            var castedSuppliers = incomingData.Suppliers.Cast<INonJunctionEntity>().ToList();
            var castedProducts = incomingData.Products.Cast<INonJunctionEntity>().ToList();



            var uniqueBrandSuppliers =
                ConstructJunctionTable<BrandSupplier>(
                    castedBrands,
                    castedSuppliers,
                    "BrandId", "SupplierId"
                    );

            var uniqueProductCountries = ConstructJunctionTable<ProductCountry>(
                castedProducts,
                 incomingData.Countries.Cast<INonJunctionEntity>().ToList(),
                 "ProductId", "CountryId"
                );
            ////////
            var uniqueProductManufacturers =
                ConstructJunctionTable<ProductManufacturer>(
                 castedProducts,
                 incomingData.Manufacturers.Cast<INonJunctionEntity>().ToList(),
                 "ProductId", "ManufacturerId"
                );

            ////
            var uniqueProductBins = ConstructJunctionTable<ProductBin>(
                castedProducts,
                incomingData.Bins.Cast<INonJunctionEntity>().ToList(),
                "ProductId", "BinId"
            );
            //////
            var uniqueProductSuppliers =
                ConstructJunctionTable<ProductSupplier>(
                castedProducts,
                castedSuppliers,
                "ProductId", "SupplierId"
            );

            /////////
            var uniqueProductBrands =
                ConstructJunctionTable<ProductBrand>(
                    castedProducts,
                    castedBrands,
                    "ProductId", "BrandId"
                    );
            ////
            var uniqueProductCategories = ConstructJunctionTable<ProductCategory>(
                castedProducts,
                incomingData.Categories.Cast<INonJunctionEntity>().ToList(),
                "ProductId", "CategoryId"
                );
            //////
            var uniqueProductUnits = ConstructJunctionTable<ProductUnit>(
                castedProducts,
                incomingData.Units.Cast<INonJunctionEntity>().ToList(),
                "ProductId", "UnitId"
                );
            #endregion

            #region adding and saving childs to the context
            await _context.AddRangeAsync(uniqueSubCategories);
            await _context.AddRangeAsync(uniqueBrandSuppliers);
            await _context.AddRangeAsync(uniqueProductCountries);
            await _context.AddRangeAsync(uniqueProductManufacturers);
            await _context.AddRangeAsync(uniqueProductBins);
            await _context.AddRangeAsync(uniqueProductSuppliers);
            await _context.AddRangeAsync(uniqueProductBrands);
            await _context.AddRangeAsync(uniqueProductCategories);
            await _context.AddRangeAsync(uniqueProductUnits);

            await _context.SaveChangesAsync();

        }
            #endregion


        #region Local functions
        static void PopulateIdProperty<T>(List<T> objects, List<T> sourceOfTruth) where T : IHasEnglishAndArabicName, IHasIdentification
        {
            var lookups = sourceOfTruth.ToDictionary(t => t.EnglishName, t => t);
            foreach (T obj in objects)
            {
                obj.Id = lookups[obj.EnglishName].Id;

            }
        }
        static HashSet<T> ConstructJunctionTable<T>(List<INonJunctionEntity> leftEntities, List<INonJunctionEntity> rightEntities, string name1, string name2) where T : new()
        {

            PropertyInfo property1 = typeof(T).GetProperty(name1);
            PropertyInfo property2 = typeof(T).GetProperty(name2);
            HashSet<T> result = new HashSet<T>();
            var zip = leftEntities.Zip(rightEntities, (le, re) => new { le, re });
            foreach (var x in zip)
            {
                T t = new T(); // PROBLEM: Replace with createInstance?
                property1.SetValue(t, x.le.Id);
                property2.SetValue(t, x.re.Id);
                result.Add(t); // O(n)  PROBELM: optimize this method
            }
            return result;
        }

        #endregion

        #endregion




    }
}