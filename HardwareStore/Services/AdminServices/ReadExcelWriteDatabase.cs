using HardwareStore.DTOs;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStoreNameSpace;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;
using System.Reflection;
namespace HardwareStore.Services.AdminServices

{
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter,IOmniReaderWriter
    {
        #region fields and properties
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private   Worksheet _sheet;
        #endregion

        #region Constructor
        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _contextFactory = contextFactory;
       
        }
        #endregion


        #region public methods
        public async Task ReadWrite()
        {
            IDataDto data =  await Read();
            await Write(data);
        }

        public async Task<IDataDto> Read()
        {



            var file = _httpContextAccessor.HttpContext.Request.Form.Files["file"];
            var workbook = new Workbook();
            using var stream = file.OpenReadStream();
            workbook.LoadFromStream(stream);
            this._sheet = workbook.Worksheets[1];



            var categories = Task<List<Category>>.Run(() =>
                GetDistinctModels<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" })
            );
            //List <Category> categories = ReadSpecificColumns<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" });
            //categories = categories.DistinctBy(c => c.EnglishName).ToList();


            var countries=Task<List<Country>>.Run(
                () =>

                    GetDistinctModels<Country>(

                        new List<string> { "EnglishName" }, new List<string> { "Country" }
                    )
                );
            //List<Country> countries = ReadSpecificColumns<Country>(new List<string> { "EnglishName" }, new List<string> { "Country" });
            //countries = countries.DistinctBy(c => c.EnglishName).ToList();

            var units = Task<List<Unit>>.Run(
                () =>

                    GetDistinctModels<Unit>(

                        new List<string> { "EnglishName" }, new List<string> { "Unit" }
                    )
                );

            //List<Unit> units = ReadSpecificColumns<Unit>(new List<string> { "EnglishName" }, new List<string> { "Unit" });
            //units = units.DistinctBy(u => u.EnglishName).ToList();

            var brands = Task<List<Brand>>.Run(
                () =>

                    GetDistinctModels<Brand>(

                        [ "EnglishName" ], [ "Brand" ]
                    )
                );

            //List<Brand> brands = ReadSpecificColumns<Brand>(new List<string> { "EnglishName" }, new List<string> { "Brand" });
            //brands = brands.DistinctBy(b => b.EnglishName).ToList();


            var suppliers = Task<List<Supplier>>.Run(
                () =>

                    GetDistinctModels<Supplier>(

                        ["EnglishName"], ["Supplier"]
                    )
                );
            //List<Supplier> suppliers = ReadSpecificColumns<Supplier>(new List<string> { "EnglishName" }, new List<string> { "Supplier" });
            //suppliers = suppliers.DistinctBy(s => s.EnglishName).ToList();

            var products= Task<List<Product>>.Run(
                () =>

                    GetDistinctModels<Product>(

                        [ "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" ],
                        [ "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" ]
                    )
                );

            //List<Product> products = ReadSpecificColumns<Product>(
            //new List<string> { "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" },
            //new List<string> { "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" }
            //);
            //products = products.DistinctBy(p => p.EnglishName).ToList();


            var bins = Task<List<Bin>>.Run(
                () =>

                    GetDistinctModels<Bin>(

                        ["EnglishName"], ["Bin"]
                    )
                );

            //List<Bin> bins = ReadSpecificColumns<Bin>(new List<string> { "EnglishName" }, new List<string> { "Bin" });
            //bins = bins.DistinctBy(c => c.EnglishName).ToList();

            var manufacturers = Task<List<Manufacturer>>.Run(
                () =>

                    GetDistinctModels<Manufacturer>(

                        ["EnglishName"], ["Manufacturer"]
                    )
                );

            //List<Manufacturer> manufacturers = ReadSpecificColumns<Manufacturer>(new List<string> { "EnglishName" }, new List<string> { "Manufacturer" });
            //manufacturers = manufacturers.DistinctBy(m => m.EnglishName).ToList();

            var subCategories =
            Task<List<SubCategory>>.Run(
                () => ReadSpecificColumns<SubCategory>
                (new List<string> { "EnglishName" }, new List<string> { "Subcategory" })
                .DistinctBy(sc => new { sc.EnglishName, sc.CategoryId })
                .OrderBy(c => c.EnglishName)
                .ToList()
                );

            return new ExcelProductsDto
            {
                Categories = await categories,
                Countries = await countries,
                Units = await units,
                Brands = await brands,
                Suppliers = await suppliers,
                Products = await products,
                Bins = await bins,
                Manufacturers = await manufacturers,
                SubCategories=await subCategories
            };
            
            List<T> GetDistinctModels<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T : IHasEnglishAndArabicName, new()
            {
                List<T> result = ReadSpecificColumns<T>(entityPropertiesNames, ExcelColumnsNames);
                return result.DistinctBy(c => c.EnglishName).ToList();
            }
             #region Local functions
            /// <summary>
            /// 
            /// <para>
            /// Normal relational sql table contains key attributes, like primary keys and foreign keys,
            /// and non-key attributes. this method  reads data from the specified excel sheet, the data
            /// will be used to populate the non-key attributes.
            /// </para>
            /// <para>
            /// Those entities returned by this method are  not unique
            /// it's up the caller to use LINQ expression to Distinct between the entities
            /// </para>
            /// Constraint : the order of the excel column names must match the order of the properties names
            /// </summary>
            /// <typeparam name="T">d</typeparam>
            /// <param name="relevantEntityPropertiesNames"></param>
            /// <param name="relevantExcelColumnsNames"></param>
            /// <param name="sheet"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            List<T> ReadSpecificColumns<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T : IHasEnglishAndArabicName, new()
            {

                int numberOfProperties = entityPropertiesNames.Count;

                #region Safety check
                if (numberOfProperties != ExcelColumnsNames.Count)
                {
                    throw new ArgumentException("The number of the excel columns must equal the number of properties names");
                }
                #endregion

                #region Get the model's properties given there names.
                PropertyInfo[] modelProperties = new PropertyInfo[numberOfProperties];
                for (int i=0; i < numberOfProperties;i++)
                {
                    modelProperties[i] = typeof(T).GetProperty(entityPropertiesNames[i]);
                }
                #endregion
                
                #region Obtain columns indicies given there names.
                int[] columnsIndicies = new int[numberOfProperties];
                for (int i=0; i < numberOfProperties; i++)
                {
                    columnsIndicies[i]= _sheet.FindString(ExcelColumnsNames[i], false, false).Column;

                }
                #endregion
                
                #region Create an in-memory representation of the Excel data.
                List<T> result = new List<T>();                
                for (int row = 2; row <= (_sheet.LastRow); row++)//Scan all the rows. Skip first row assuming it is a header.
                {

                    T entity = new T();
                    //Scan specific columns; populate all the relevant properties of a single Entity  
                    for (int i = 0; i < numberOfProperties; i++)
                    {
                        var excelCellValue = Convert.ChangeType(_sheet.Range[row, columnsIndicies[i]].Value, modelProperties[i].PropertyType);
                        modelProperties[i].SetValue(entity, excelCellValue);
                    }

                    result.Add(entity);
                }
                #endregion
                
                
                return result;

            }

            #endregion


        }

        public async Task Write(IDataDto dataDto)
        {
            #region delete all data from the database
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXECUTE uspDeleteAllDataFromDatabase"
                );
            }
            #endregion

            var incomingData = (ExcelProductsDto)dataDto;

            #region childs cannot be populated unless the parents are populated first
            using (var _context0 = _contextFactory.CreateDbContext())
            using (var _context1 = _contextFactory.CreateDbContext())
            using (var _context2 = _contextFactory.CreateDbContext())
            using (var _context3 = _contextFactory.CreateDbContext())
            using (var _context4 = _contextFactory.CreateDbContext())
            using (var _context5 = _contextFactory.CreateDbContext())
            using (var _context6 = _contextFactory.CreateDbContext())
            using (var _context7 = _contextFactory.CreateDbContext())
            {
                Task a0 = _context0.AddRangeAsync(incomingData.Suppliers);
                Task a1 = _context1.AddRangeAsync(incomingData.Brands);
                Task a2 = _context2.AddRangeAsync(incomingData.Units);
                Task a3 = _context3.AddRangeAsync(incomingData.Categories);
                Task a4 = _context4.AddRangeAsync(incomingData.Countries);
                Task a5 = _context5.AddRangeAsync(incomingData.Products);
                Task a6 = _context6.AddRangeAsync(incomingData.Bins);
                Task a7 = _context7.AddRangeAsync(incomingData.Manufacturers);
                await a0;
                await a1;
                await a2;
                await a3;
                await a4;
                await a5;
                await a6;
                await a7;

                Task s0 = _context0.SaveChangesAsync();
                Task s1 = _context1.SaveChangesAsync();
                Task s2 = _context2.SaveChangesAsync();
                Task s3 = _context3.SaveChangesAsync();
                Task s4 = _context4.SaveChangesAsync();
                Task s5 = _context5.SaveChangesAsync();
                Task s6 = _context6.SaveChangesAsync();
                Task s7 = _context7.SaveChangesAsync();

                await s0;
                await s1;
                await s2;
                await s3;
                await s4;
                await s5;
                await s6;
                await s7;
            }

            #endregion

            #region Populating non-junction  child tables
            // lastly, optimize this

            var subCategoryCategoriesLookups =
                // optimize this; why the fuck read the SubCateogry inside the function again, even though you have incomingData.SubCategories?
                CreateLookupTable("Subcategory", "Category") // optimize this; why the fuck read Category again , even though you have incomingData.Categories
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .OrderBy(r => r.LeftColumnCellValue)
                .ToList();
            PopulateForeignKeyPropertyForNonJunctionChild<SubCategory, Category>( //optimize this 
                "CategoryId", incomingData.SubCategories,
                subCategoryCategoriesLookups,
                incomingData.Categories
            );
            #endregion

            #region Populating junction tables (a junction table contain two foreign keys)
            var brandSupplierslookups =
                CreateLookupTable("Brand", "Supplier")// optimize this; why the fuck read Brand and Supplier again even though you have the incoming data?
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .ToList();
            var brandSuppliers = ConstructJunctionTableData<BrandSupplier, Brand, Supplier>(
                "BrandId", "SupplierId", brandSupplierslookups,
                 incomingData.Brands, incomingData.Suppliers
            );
            /////////
            var productCountriesLookups = CreateLookupTable("English Name", "Country")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productCountries = ConstructJunctionTableData<ProductCountry, Product, Country>(
                "ProductId", "CountryId",
                 productCountriesLookups,
                 incomingData.Products, incomingData.Countries
            );
            ////////
            var productManufacturersLookups =
                CreateLookupTable("English Name", "Manufacturer")
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .ToList();
            var productManufacturers = ConstructJunctionTableData<ProductManufacturer, Product, Manufacturer>(
                "ProductId", "ManufacturerId", productManufacturersLookups,
                 incomingData.Products, incomingData.Manufacturers

            );
            ////
            var productBinslookups = CreateLookupTable("English Name", "Bin")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productBins = ConstructJunctionTableData<ProductBin, Product, Bin>(
                    "ProductId", "BinId",
                     productBinslookups,
                     incomingData.Products, incomingData.Bins
            );
            //////
            var productSuppliersLookups = CreateLookupTable("English Name", "Supplier")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productSuppliers = ConstructJunctionTableData<ProductSupplier, Product, Supplier>(
                "ProductId", "SupplierId",
                 productSuppliersLookups,
                 incomingData.Products, incomingData.Suppliers
                 );

            /////////
            var productBrandsLookups = CreateLookupTable("English Name", "Brand")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            List<ProductBrand> productBrands = ConstructJunctionTableData<ProductBrand, Product, Brand>(
                "ProductId", "BrandId",
                 productBrandsLookups,
                 incomingData.Products, incomingData.Brands
                 );
            ////
            var productCategoriesLookups = CreateLookupTable("English Name", "Category")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productCategories = ConstructJunctionTableData<ProductCategory, Product, Category>(
            "ProductId", "CategoryId",
            productCategoriesLookups,
            incomingData.Products, incomingData.Categories
            );
            //////
            var productUnitsLookups = CreateLookupTable("English Name", "Unit")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productUnits = ConstructJunctionTableData<ProductUnit, Product, Unit>(
                "ProductId", "UnitId",
                 productUnitsLookups,
                 incomingData.Products, incomingData.Units
            );

            #endregion

            #region Asyncronusly Adding data to multiple contexts and saving changes
            using (var _context0 = _contextFactory.CreateDbContext())
            using (var _context1 = _contextFactory.CreateDbContext())
            using (var _context2 = _contextFactory.CreateDbContext())
            using (var _context3 = _contextFactory.CreateDbContext())
            using (var _context4 = _contextFactory.CreateDbContext())
            using (var _context5 = _contextFactory.CreateDbContext())
            using (var _context6 = _contextFactory.CreateDbContext())
            using (var _context7 = _contextFactory.CreateDbContext())
            using (var _context8 = _contextFactory.CreateDbContext())
            {

                Task a0 = _context0.AddRangeAsync(incomingData.SubCategories);
                Task a1 = _context1.AddRangeAsync(brandSuppliers);
                Task a2 = _context2.AddRangeAsync(productCountries);
                Task a3 = _context3.AddRangeAsync(productManufacturers);
                Task a4 = _context4.AddRangeAsync(productBins);
                Task a5 = _context5.AddRangeAsync(productSuppliers);
                Task a6 = _context6.AddRangeAsync(productBrands);
                Task a7 = _context7.AddRangeAsync(productCategories);
                Task a8 = _context8.AddRangeAsync(productUnits);
                await a0;
                await a1;
                await a2;
                await a3;
                await a4;
                await a5;
                await a6;
                await a7;
                await a8;
                Task s0 = _context0.SaveChangesAsync();
                Task s1 = _context1.SaveChangesAsync();
                Task s2 = _context2.SaveChangesAsync();
                Task s3 = _context3.SaveChangesAsync();
                Task s4 = _context4.SaveChangesAsync();
                Task s5 = _context5.SaveChangesAsync();
                Task s6 = _context6.SaveChangesAsync();
                Task s7 = _context7.SaveChangesAsync();
                Task s8 = _context8.SaveChangesAsync();
                await s0;
                await s1;
                await s2;
                await s3;
                await s4;
                await s5;
                await s6;
                await s7;
                await s8;
            }
            #endregion

            #region Local functions

            List<J> ConstructJunctionTableData<J, Parent1, Parent2>(string firstPropertyName, string secondPropertyName, List<RelationshipLookupRecord> lookups, List<Parent1> existingEntities1, List<Parent2> existingEntities2) where J : new() where Parent1 : class, IHasIdentification, IHasEnglishAndArabicName, new() where Parent2 : class, IHasIdentification, IHasEnglishAndArabicName, new()

            {


                var Property1 = typeof(J).GetProperty(firstPropertyName)!;
                var Property2 = typeof(J).GetProperty(secondPropertyName)!;
                List<J> result = new List<J>();
                foreach (RelationshipLookupRecord lookup in lookups)
                {
                    // optimize this by using tasking

                    Parent1 Entity1 = existingEntities1.First<Parent1>(e => lookup.LeftColumnCellValue == e.EnglishName);//optimize

                    Parent2 Entity2 = existingEntities2.First<Parent2>(e => lookup.RightColumnCellValue == e.EnglishName);//optimize
                    J j = new J();
                    Property1.SetValue(j, Entity1.Id);
                    Property2.SetValue(j, Entity2.Id);
                    result.Add(j);
                }

                return result;
            }

            void PopulateForeignKeyPropertyForNonJunctionChild<Child, Parent>(string foreignKeyPropertyName, List<Child> childs, List<RelationshipLookupRecord> lookups, List<Parent> parentRows) where Parent : class, IHasEnglishAndArabicName, IHasIdentification



            {

                // Suppose we have two tables; Category and SubCategory, the relation is one-to-many.
                // The SubCategory Table  has two columns; EnglishName and CategoryId.
                // The job of this method is  to populate the CategoryId with the correct values.
                // EnglishName  alone is not sufficient to complete the job; a mapping  between  SubCategory and Category tables is required. 
                // This mapping is called 'lookup table', it has more than one record, each record contains a mapping between  one SubCategory and one Category.
                ///For each record, we can query the  relevant row in the Category Table; the row with the same EnglishName as the record.
                // The row that we found contains the Id; it's value will be used  as the value for the foreign key.

                if (lookups.Count > childs.Count)
                {

                    throw new ArgumentException("childs length can't be less than the number of the records in the lookup table.\n");
                }

                //Refer to the parent to be able to set the foreign key.
                PropertyInfo foreignKeyProperty = typeof(Child).GetProperty(foreignKeyPropertyName)!;

                for (int i = 0; i < lookups.Count; i++) // optimize this in any way
                {
                    // optimize this by using tasking
                    Parent foundEntity = parentRows.First<Parent>(r => r.EnglishName == lookups[i].RightColumnCellValue);
                    foreignKeyProperty.SetValue(childs[i], foundEntity.Id);
                }
            }

            List<RelationshipLookupRecord> CreateLookupTable(string firstTablNameInExcel, string secondTableNameInExcel)
            {

                //Reading operation is the only process that should be responsible for reading excel file
                // Why the fuck am I reading the excel again? I already have the incomingData passed from the Write method!

                int leftColumnNumber = this._sheet.FindString(firstTablNameInExcel, false, false).Column;
                int rightColumnNumber = _sheet.FindString(secondTableNameInExcel, false, false).Column;
                List<RelationshipLookupRecord> result = new();
                for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
                {
                    result.Add(new RelationshipLookupRecord
                    {
                        LeftColumnCellValue = _sheet.Range[rowNumber, leftColumnNumber].Value,
                        RightColumnCellValue = _sheet.Range[rowNumber, rightColumnNumber].Value
                    });
                }
                return result;
            }

            #endregion
        }



        #endregion

        #region private methods
        // currently no private methods
        #endregion




    }
}