namespace HardwareStore.Services.AdminServices

{
    using HardwareStore.DTOs;
    using HardwareStore.Models;
    using HardwareStore.SeedWork;
    using HardwareStoreNameSpace;
    using Microsoft.EntityFrameworkCore;
    using Spire.Xls;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Claims;

    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter,IOmniReaderWriter
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        //private readonly ApplicationDbContext _context2
        //private readonly ApplicationDbContext _context3
        public Worksheet _sheet;
        private readonly string _adminId;

        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _adminId= _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        public  async Task ReadWrite()
        {
                    IDataDto data = Read();
                    await Write(data);
        }

        public IDataDto Read()
        {

            

            var file = _httpContextAccessor.HttpContext.Request.Form.Files["file"];
            var workbook = new Workbook();
            using var stream = file.OpenReadStream();
            workbook.LoadFromStream(stream);
            this._sheet = workbook.Worksheets[1];



            List<Category> categories = ReadSpecificColumns<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" });
            categories = categories.DistinctBy(c => c.EnglishName).ToList();
            categories = AssignCreatorAndUpdaterToEntitis<Category>(categories, _adminId);

           
            List<Country> countries = ReadSpecificColumns<Country>(new List<string> { "EnglishName" }, new List<string> { "Country" });
            countries = countries.DistinctBy(c => c.EnglishName).ToList();
            countries = AssignCreatorAndUpdaterToEntitis<Country>(countries, _adminId);
            

            List<Unit> units = ReadSpecificColumns<Unit>(new List<string> { "EnglishName" }, new List<string> { "Unit" });
            units = units.DistinctBy(u => u.EnglishName).ToList();
            units = AssignCreatorAndUpdaterToEntitis<Unit>(units, _adminId);
            

            List<Brand> brands = ReadSpecificColumns<Brand>(new List<string> { "EnglishName" }, new List<string> { "Brand" });
            brands = brands.DistinctBy(b => b.EnglishName).ToList();
            brands = AssignCreatorAndUpdaterToEntitis<Brand>(brands, _adminId);
            

            List<Supplier> suppliers = ReadSpecificColumns<Supplier>(new List<string> { "EnglishName" }, new List<string> { "Supplier" });
            suppliers = suppliers.DistinctBy(s => s.EnglishName).ToList();
            suppliers = AssignCreatorAndUpdaterToEntitis<Supplier>(suppliers, _adminId);
            

            List<Product> products = ReadSpecificColumns<Product>(
            new List<string> { "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" },
            new List<string> { "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" }
            );
            products = products.DistinctBy(p => p.EnglishName).ToList();
            products = AssignCreatorAndUpdaterToEntitis<Product>(products, _adminId);
           

            List<Bin> bins = ReadSpecificColumns<Bin>(new List<string> { "EnglishName" }, new List<string> { "Bin" });
            bins = bins.DistinctBy(c => c.EnglishName).ToList();
            bins = AssignCreatorAndUpdaterToEntitis<Bin>(bins, _adminId);
            

            List<Manufacturer> manufacturers = ReadSpecificColumns<Manufacturer>(new List<string> { "EnglishName" }, new List<string> { "Manufacturer" });
            manufacturers = manufacturers.DistinctBy(m => m.EnglishName).ToList();
            manufacturers = AssignCreatorAndUpdaterToEntitis<Manufacturer>(manufacturers, _adminId);

            return  new ExcelProductsDto
            {
                     Categories=categories,
                     Countries=countries,
                     Units=units,
                    Brands=brands,
                     Suppliers=suppliers,
                    Products=products,
                     Bins=bins,
                    Manufacturers=manufacturers
            };



        }



        /// <summary>
        /// Any parent table must be written before a child table.
        /// </summary>
        /// <param name="epDto"></param>
        public async Task  Write(IDataDto dataDto)
        {

            var incomingData = (ExcelProductsDto)dataDto;
            
            // maybe can use ToListAsync without  casuing race condition???
            List<Supplier> preExistingSuppliers = _context.Suppliers.ToList();
            List<Brand> preExistingBrands= _context.Brands.ToList();
            List<Unit> preExistingUnits= _context.Units.ToList();
            List<Category> preExistingCategories= _context.Categories.ToList();
            List<Country> preExistingCountries= _context.Countries.ToList();
            List<Product> preExistingProducts= _context.Products.ToList();
            List<Bin> preExistingBins= _context.Bins.ToList();
            List<Manufacturer> preExistingManufacturers= _context.Manufacturers.ToList();

            // Populating parents tables; tables that does not have any foreign key.
            var newAndObsoleteSuppliers = GetNewAndObsoleteEntities(incomingData.Suppliers,preExistingSuppliers);
            var newAndObsoleteBrands = GetNewAndObsoleteEntities(incomingData.Brands, preExistingBrands);
            var newAndObsoleteUnits = GetNewAndObsoleteEntities(incomingData.Units, preExistingUnits);
            var newAndObsoleteCategories = GetNewAndObsoleteEntities(incomingData.Categories, preExistingCategories);
            var newAndObsoleteCountries = GetNewAndObsoleteEntities(incomingData.Countries, preExistingCountries);
            var newAndObsoleteProducts = GetNewAndObsoleteEntities(incomingData.Products, preExistingProducts);
            var newAndObsoleteBins = GetNewAndObsoleteEntities(incomingData.Bins, preExistingBins);
            var newAndObsoleteManufacturers = GetNewAndObsoleteEntities(incomingData.Manufacturers, preExistingManufacturers);
            
            List<Supplier> newSuppliers = newAndObsoleteSuppliers[0];
            if ( newSuppliers.Count != 0)
                {

                    preExistingSuppliers.AddRange(newSuppliers);
                    _context.AddRange(newSuppliers);
                }
            List<Supplier> obsoleteSuppliers = newAndObsoleteSuppliers[1];
            if (obsoleteSuppliers.Count != 0)
                {
                    foreach (var oS in obsoleteSuppliers)
                    {
                        preExistingSuppliers.Remove(oS);
                    }
                    _context.RemoveRange(obsoleteSuppliers);
                }
            ////
            List<Brand> newBrands = newAndObsoleteBrands[0];
            if (newBrands.Count != 0)
            {
                preExistingBrands.AddRange(newBrands);
                _context.AddRange(newBrands);
            }
            List<Brand> obsoleteBrands = newAndObsoleteBrands[1];
            if (obsoleteBrands.Count != 0)
            {
                foreach(Brand b in obsoleteBrands)
                {
                    preExistingBrands.Remove(b);
                }
                _context.RemoveRange(obsoleteBrands);
            }
            ///
            List<Unit> newUnits = newAndObsoleteUnits[0];
            if (newUnits.Count != 0)
            {
                preExistingUnits.AddRange(newUnits);
                _context.AddRange(newUnits);
            }
            List<Unit> obsoleteUnits =  newAndObsoleteUnits[1];
            if (obsoleteUnits.Count != 0)
            {
                foreach(Unit oU in obsoleteUnits)
                {
                    preExistingUnits.Remove(oU);
                }
                _context.RemoveRange(obsoleteUnits);
            }
            ///
            List<Category> newCategories =newAndObsoleteCategories[0];
            if (newCategories.Count != 0)
            {
                preExistingCategories.AddRange(newCategories);
                _context.AddRange(newCategories);
            }
            List<Category> obsoleteCategories = newAndObsoleteCategories[1];
            if (obsoleteCategories.Count != 0)
            {
                foreach(Category  oC  in obsoleteCategories)
                {
                    preExistingCategories.Remove(oC);
                }
                _context.RemoveRange(obsoleteCategories);
            }
            ///
            List<Country> newCountries = newAndObsoleteCountries[0];
            if (newCountries.Count != 0)
            {
                preExistingCountries.AddRange(newCountries);
                _context.AddRange(newCountries);
            }
            List<Country> obsoleteCountries = newAndObsoleteCountries[1];
            if (obsoleteCountries.Count != 0)
            {
                foreach (Country oC in obsoleteCountries)
                {
                    preExistingCountries.Remove(oC);
                }
                _context.RemoveRange(obsoleteCountries);
            }
            ///
            List<Product> newProducts = newAndObsoleteProducts[0];
            if (newProducts.Count != 0)
            {
                preExistingProducts.AddRange(newProducts);
                _context.AddRange(newProducts);
            }
            List<Product> obsoleteProducts = newAndObsoleteProducts[1];
            if (obsoleteProducts.Count != 0)
            {
                foreach(Product oP in obsoleteProducts)
                {
                    preExistingProducts.Remove(oP);    
                }
                _context.RemoveRange(obsoleteProducts);
            }
            ///
            List<Bin> newBins = newAndObsoleteBins[0];
            if (newBins.Count != 0)
            {
                preExistingBins.AddRange(newBins);
                _context.AddRange(newBins);
            }
            List<Bin> obsoleteBins = newAndObsoleteBins[1];
            if (obsoleteBins.Count != 0)
            {
                foreach(Bin oB in obsoleteBins)
                {
                    preExistingBins.Remove(oB);
                }
                _context.RemoveRange(obsoleteBins);
            }
            ///
            List<Manufacturer> newManufacturers = newAndObsoleteManufacturers[0];
            if (newManufacturers.Count != 0)
            {
                preExistingManufacturers.AddRange(newManufacturers);
                _context.AddRange(newManufacturers);
            }
            List<Manufacturer> obsoleteManufacturers = newAndObsoleteManufacturers[1];
            if (obsoleteManufacturers.Count != 0)
            {
                foreach(Manufacturer oM in obsoleteManufacturers)
                {
                    preExistingManufacturers.Remove(oM);
                }
                _context.RemoveRange(obsoleteManufacturers);
            }

            await _context.SaveChangesAsync();
 
            
            //Populating Childs; NonJunction and Junction childs.

            var subCategories = ReadSpecificColumns<SubCategory>
                (new List<string> { "EnglishName" }, new List<string> { "Subcategory" })
                .DistinctBy(sc => new { sc.EnglishName, sc.CategoryId })
                .OrderBy(c => c.EnglishName)
                .ToList();
            var subCategoryCategoriesLookups = 
                CreateLookupTable("Subcategory", "Category")
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .OrderBy(r => r.LeftColumnCellValue)
                .ToList();
            PopulateForeignKeyPropertyForNonJunctionChild<SubCategory, Category>(
                "CategoryId", subCategories, // NonJunction childs are already constructed.
                subCategoryCategoriesLookups,
                preExistingCategories // syncronized with DB
            );
            subCategories = AssignCreatorAndUpdaterToEntitis<SubCategory>(subCategories, _adminId);
            var newAndObsoleteSubCategories = GetNewAndObsoleteEntities(subCategories, _context.SubCategories.ToList());
            List<SubCategory> newSubCategories = newAndObsoleteSubCategories[0];
            if (newSubCategories.Count != 0)
            {
                _context.AddRange(newSubCategories);
            }
            List<SubCategory> obsoleteSubCategories = newAndObsoleteSubCategories[1];
            if (obsoleteSubCategories.Count != 0)
            {
                _context.RemoveRange(obsoleteSubCategories);
            }

            var brandSupplierslookups =
                CreateLookupTable("Brand", "Supplier")
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .ToList();
            var brandSuppliers = ConstructJunctionTableData<BrandSupplier, Brand, Supplier>(
                "BrandId", "SupplierId", brandSupplierslookups, 
                 preExistingBrands, preExistingSuppliers
            );
            
            var newAndObsoleteBrandSuppliers =GetNewAndObsoleteJunctionEntities<BrandSupplier>(
                 brandSuppliers, _context.BrandsSuppliers.ToList()
            );

            var productCountriesLookups = CreateLookupTable("English Name", "Country")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productCountries = ConstructJunctionTableData<ProductCountry, Product, Country>(
                "ProductId", "CountryId",
                 productCountriesLookups,
                 preExistingProducts, preExistingCountries
            );
            var newAndObsoleteProductsCountries = GetNewAndObsoleteJunctionEntities<ProductCountry>(
                productCountries, _context.ProductsCountries.ToList()

                );

            var productManufacturersLookups =
                CreateLookupTable("Product", "Manufacturer")
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .ToList();
            var productManufacturers = ConstructJunctionTableData<ProductManufacturer, Product, Manufacturer>(
                "ProductId", "ManufacturerId",productManufacturersLookups,
                 preExistingProducts,preExistingManufacturers
          
            );
            var newAndObsoleteProductManufacturers = GetNewAndObsoleteJunctionEntities<ProductManufacturer>(
                productManufacturers, _context.ProductsManufacturers.ToList()
            );

            var productBinslookups= CreateLookupTable("English Name", "Bin")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productBins = ConstructJunctionTableData<ProductBin, Product, Bin>(
                    "ProductId", "BinId",
                     productBinslookups,
                     preExistingProducts, preExistingBins       
            );


            var productSuppliersLookups = CreateLookupTable("English Name","Supplier")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productSuppliers = ConstructJunctionTableData<ProductSupplier, Product, Supplier>(
                "ProductId", "SupplierId",
                 productSuppliersLookups, 
                 preExistingProducts, preExistingSuppliers
                 );

            var productBrandsLookups = CreateLookupTable("English Name","Brand")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            List<ProductBrand> productBrands = ConstructJunctionTableData<ProductBrand, Product, Brand>(
                "ProductId", "BrandId",
                 productBrandsLookups, 
                 preExistingProducts, preExistingBrands
                 );

            var productCategoriesLookups = CreateLookupTable("English Name", "Category")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productCategories = ConstructJunctionTableData<ProductCategory, Product, Category>(
            "ProductId", "CategoryId",
            productCategoriesLookups, 
            preExistingProducts, preExistingCategories
            );

            var productUnitsLookups = CreateLookupTable("English Name", "Unit")
               .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
               .ToList();
            var productUnits = ConstructJunctionTableData<ProductUnit, Product, Unit>(
                "ProductId", "UnitId",
                 productUnitsLookups, 
                 preExistingProducts, preExistingUnits
            );


            AddRangeOfJunctionEntities<BrandSupplier>(
                brandSuppliers);
            AddRangeOfJunctionEntities<ProductManufacturer>(
                productManufacturers);
            AddRangeOfJunctionEntities<ProductBin>(
                productBins);
            AddRangeOfJunctionEntities<ProductSupplier>(
                productSuppliers);
            AddRangeOfJunctionEntities<ProductBrand>(
                productBrands);
            AddRangeOfJunctionEntities<ProductCategory>(
                productCategories);
            AddRangeOfJunctionEntities<ProductUnit>(
                productUnits);
            AddRangeOfJunctionEntities<ProductCountry>(
                productCountries);

            await _context.SaveChangesAsync();



        //Local functions inside Write()
         List<J> ConstructJunctionTableData<J, Parent1, Parent2>
         (string firstPropertyName, string secondPropertyName, List<RelationshipLookupRecord> lookups, List<Parent1> existingEntities1, List<Parent2> existingEntities2)
            where J : new()
            where Parent1 : class, IHasIdentification, IHasEnglishAndArabicName, new()
            where Parent2 : class, IHasIdentification, IHasEnglishAndArabicName, new()
            {


                var Property1 = typeof(J).GetProperty(firstPropertyName)!;
            var Property2 = typeof(J).GetProperty(secondPropertyName)!;
            List<J> result = new List<J>();
            foreach (RelationshipLookupRecord lookup in lookups)
            {

                Parent1 Entity1 = existingEntities1.First<Parent1>(e => lookup.LeftColumnCellValue == e.EnglishName);//optimize

                Parent2 Entity2 = existingEntities2.First<Parent2>(e => lookup.RightColumnCellValue == e.EnglishName);//optimize
                J j = new J();
                Property1.SetValue(j, Entity1.Id);
                Property2.SetValue(j, Entity2.Id);
                result.Add(j);
            }

            return result;
        }

        void PopulateForeignKeyPropertyForNonJunctionChild<Child, Parent>(string foreignKeyPropertyName, List<Child> childs,  List<RelationshipLookupRecord> lookups, List<Parent> parentRows) where Parent : class, IHasEnglishAndArabicName, IHasIdentification



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

            for (int i = 0; i < lookups.Count; i++)
            {
                Parent foundEntity = parentRows.First<Parent>(r => r.EnglishName == lookups[i].RightColumnCellValue);
                foreignKeyProperty.SetValue(childs[i], foundEntity.Id);
            }
        }
    }

        

        public  List<T> AssignCreatorAndUpdaterToEntitis<T>(List<T> entities, string Id) where T: HasCreatorAndUpdator
        {
            foreach (T entity in entities)
            {
                entity.CreatorId = Id;
                entity.UpdaterId = Id;
                
            }
            return entities;
        }


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
        private List<T> ReadSpecificColumns<T>(List<string> relevantEntityPropertiesNames, List<string> relevantExcelColumnsNames) where T : IHasEnglishAndArabicName, new()
        {
            
            if (relevantEntityPropertiesNames.Count != relevantExcelColumnsNames.Count)
            {
                throw new ArgumentException("The number of the excel columns must equal the number of properties names");
            }

                List<T> result = new List<T>();
                 
                //scan all the rows 
                for (int row = 2; row <= (_sheet.LastRow); row++)//assuming row 1 is header, want skip it.
                {
                    //scan specific columns
                    // Populate all the relevant properties of a single Entity 
                    T entity = new T();
                    
                    for (int i = 0; i < relevantEntityPropertiesNames.Count; i++)// most of the time, the inner for loop will have only one iteration, good.
                    { 
                        var entityProperty = typeof(T).GetProperty(relevantEntityPropertiesNames[i]);
                        int relevantColumnNumber = _sheet.FindString(relevantExcelColumnsNames[i], false, false).Column; 

                        var excelCellValue = Convert.ChangeType(_sheet.Range[row, relevantColumnNumber].Value, entityProperty!.PropertyType);
                        entityProperty.SetValue(entity, excelCellValue);
                    }

                        result.Add(entity);
            }
                return result;

        }


        /// <summary>
        /// 
        /// <para>
        ///   You can use this method to set the values for a  <strong >single foreign key</strong>
        ///   
        ///   ,<strong>OR</strong> multiple foreign keys by calling this method <strong>multiple times</strong>,
        ///   <strong>but</strong> there are important details to consider.
        /// </para>
        ///  
        /// 
        ///  <para>
        ///  If you want to set a <strong>single foreign key</strong> column, call this method once, 
        ///  the populated foreign key is not guaranteed to contain non-zero values, so it's up to the caller
        ///  to filter out the zero values.
        /// </para>
        /// 
        /// <para>
        ///  if you want to set the values for <strong>two foreing keys</strong>, call this method twice, the first call
        ///  is to set the values for the first foreign key, the second call is to set the values for the second foreign key,
        ///  only after the second call you are allowed to filter out the zero values,
        ///  you should not filter out the zero values after the first call of this method.
        ///  
        /// <para>
        /// Below is an example on how to populate a 
        /// <strong>Junction table</strong> that contains 
        /// two foreign keys columns, initially, each column 
        /// is zeroed, i.e., all the values in the column are
        /// zeros.
        /// </para>
        ///  <para>
        ///  
        ///  A junction table called <strong>ProductsCategories</strong>
        ///  has  two foreing keys;  <strong>CategoryId</strong>
        ///  and <strong>ProductId</strong>, call this method
        ///  once to set the values for the <strong>ProductId</strong>
        ///  , some values might still be zero , but <strong>don't</strong>
        ///  filter them out yet. then, <strong>Don't Sort</strong> the values of the <strong>ProductId</strong>, then  call this method again 
        ///  to set the values for the <strong>CategoryId</strong>,
        ///  After the second call, you can filter out the zeros, both <strong>CategoryId</strong>
        ///  and <strong>ProductId</strong> are not allowed to contain zeros.
        ///  </para>
        ///  </para>
        ///   
        ///  <para>
        ///  If the table is not a <strong> junction table</strong>,i.e. it's a
        ///  a table at the <strong>Many</strong> side in a <strong>1:M</strong>
        ///  relation that, at least, contains three columns; Id , EnglishName,
        ///  and a foreign-key column, then to populate the foreign key, it is
        ///  <strong>required to sort</strong> the table by the EnglishName 
        ///  before calling this method <strong>once</strong>
        /// , 
        ///  
        ///</para>
        ///<para>    
        ///  This method does not write the values to the database, it's 
        ///  completely a read operation, the destination that this method
        ///  writes to is the  foreign key property in the C# model, not the 
        ///  foreign key column in the database.
        ///</para>
        /// </summary>
        /// <typeparam name="Child"></typeparam>
        /// <typeparam name="Parent"></typeparam>
        /// <param name="childTablNameInExcel"></param>
        /// <param name="parentTableNameInExcel"></param>
        /// <param name="foreignKeyPropertyName"></param>
        /// <param name="childs"></param>
        /// <exception cref="ArgumentException">
        /// </exception>        



        /// <summary>
        /// <para>// Should I redesign the  function so that it gets the data of the parent from the database instead of the excel file</para>
        /// </summary>
        /// <param name="childTablNameInExcel"></param>
        /// <param name="parentTableNameInExcel"></param>
        /// <returns></returns>
        public List<RelationshipLookupRecord> CreateLookupTable(string firstTablNameInExcel,string secondTableNameInExcel)
        {

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

        /// <summary>
        /// 
        /// Add objects to the context, if those objects are not tracked.
        /// 
        /// <para>
        /// Internally, the Except method is used to produce the set difference of two sequences by using a custom equality comparer to compare values
        /// Instead of comparing references.
        /// </para>
        /// 
        /// </summary>
        /// <typeparam name="M"> generic type for a model class</typeparam>
        /// <param name="objects">List of models, each model has the generic type M</param>
        public  List<NJM>[] GetNewAndObsoleteEntities<NJM>(List<NJM> objects,List<NJM> existingEntities)
            where NJM:IHasEnglishAndArabicName
        {

                return [objects.Except(existingEntities).ToList(), existingEntities.Except(objects).ToList()];
        }
        public List<JM>[] GetNewAndObsoleteJunctionEntities<JM>(List<JM> objects, List<JM> existingEntities)
            where JM:IJunctionEntity
        {
            return [objects.Except(existingEntities).ToList(), existingEntities.Except(objects).ToList()];
        }
    }


}