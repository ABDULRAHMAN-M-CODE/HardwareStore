

namespace HardwareStore.Services

{
    using HardwareStore.DTOs;
    using HardwareStore.Models;
    using HardwareStoreNameSpace;

    using Microsoft.EntityFrameworkCore;
    using Spire.Xls;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Security.Claims;

    using System;


    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public  Worksheet _sheet;// populated inside the  ReadAndWriteData() method.
        private readonly string _adminId;

        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _adminId= _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        // Interface method.
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

        // Interface method.
        public void Write(IDataDto dataDto) 
        {
            Stopwatch writeParentWatch = new Stopwatch();
            writeParentWatch.Start();

            ExcelProductsDto epDto= (ExcelProductsDto)dataDto;
            WriteParentTables(epDto);
            
            writeParentWatch.Stop();
            TimeSpan writeParentsTime = writeParentWatch.Elapsed;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"D:\Training", "NoticePerformance.txt"), true))
            {
                outputFile.WriteLine($" WriteParentTables(epDto) Time: {writeParentsTime}\n");
            }


            Stopwatch writeChildsWatch = new Stopwatch();
            writeChildsWatch.Start();
            
            WriteChildTables();

            writeChildsWatch.Stop();
            TimeSpan writeChildsTime = writeChildsWatch.Elapsed;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"D:\Training", "NoticePerformance.txt"), true))
            {
                outputFile.WriteLine($" WriteChildTables() Time: {writeChildsTime}\n");
            }

        }

        /// <summary>
        /// Any parent table must be written before a child table.
        /// </summary>
        /// <param name="epDto"></param>
        public void  WriteParentTables(ExcelProductsDto epDto)
        {

            AddRange(epDto.Suppliers);
            AddRange(epDto.Brands);
            AddRange(epDto.Units);
            AddRange(epDto.Categories);
            AddRange(epDto.Countries);
            AddRange(epDto.Products);
            AddRange(epDto.Bins);
            AddRange(epDto.Manufacturers);
            _context.SaveChanges();
        }

        public void WriteChildTables()
        {

            List<SubCategory> subCategories = ReadSpecificColumns<SubCategory>(new List<string> { "EnglishName" }, new List<string> { "Subcategory" });
            subCategories = subCategories.DistinctBy(sc => new { sc.EnglishName, sc.CategoryId }).ToList();
            subCategories = subCategories.OrderBy(c => c.EnglishName).ToList();
            PopulateForeignKeyPropertyForEachChild<SubCategory, Category>("Subcategory", "Category", "CategoryId", subCategories);
            subCategories = AssignCreatorAndUpdaterToEntitis<SubCategory>(subCategories, _adminId);
            AddRange(subCategories);// Works, because each subCategory has english name and arabic name

            List<BrandSupplier> brandSuppliers = ConstructJunctionTableData<BrandSupplier, Brand, Supplier>(
                "BrandId", "SupplierId",
                 "Brand", "Supplier");
           
            AddJunctionTableDataToContext<BrandSupplier>(
                "BrandId", "SupplierId",
                brandSuppliers);


            List<ProductManufacturer> productManufacturers = ConstructJunctionTableData<ProductManufacturer, Product, Manufacturer>(
                "ProductId", "ManufacturerId",
                 "English Name", "Manufacturer");
            AddJunctionTableDataToContext<ProductManufacturer>(
                "ProductId", "ManufacturerId",
                productManufacturers);



            List<ProductBin> productBins = ConstructJunctionTableData<ProductBin, Product, Bin>(
                "ProductId", "BinId",
                 "English Name", "Bin");
            AddJunctionTableDataToContext<ProductBin>(
                "ProductId", "BinId",
                productBins);


            List<ProductSupplier> productSuppliers = ConstructJunctionTableData<ProductSupplier, Product, Supplier>(
                "ProductId", "SupplierId",
                 "English Name", "Supplier");
            AddJunctionTableDataToContext<ProductSupplier>(
                "ProductId", "SupplierId",
                productSuppliers);

            List<ProductBrand> productBrands = ConstructJunctionTableData<ProductBrand, Product, Brand>(
                "ProductId", "BrandId",
                 "English Name", "Brand");
            AddJunctionTableDataToContext<ProductBrand>(
                "ProductId", "BrandId",
                productBrands);

            List<ProductCategory> productCategories = ConstructJunctionTableData<ProductCategory, Product, Category>(
            "ProductId", "CategoryId",
            "English Name", "Category");
            AddJunctionTableDataToContext<ProductCategory>(
                "ProductId", "CategoryId",
                productCategories);


            List<ProductUnit> productUnits = ConstructJunctionTableData<ProductUnit, Product, Unit>(
                "ProductId", "UnitId",
                 "English Name", "Unit");
            AddJunctionTableDataToContext<ProductUnit>(
                "ProductId", "UnitId",
                productUnits);


            List<ProductCountry> productCountries = ConstructJunctionTableData<ProductCountry, Product, Country>(
                "ProductId", "CountryId",
                 "English Name", "Country");
            AddJunctionTableDataToContext<ProductCountry>(
                "ProductId", "CountryId",
                productCountries);

            _context.SaveChanges();// returned 0
        }

        //Helper method
        public  List<T> AssignCreatorAndUpdaterToEntitis<T>(List<T> entities, string Id) where T: HasCreatorAndUpdator
        {
            foreach (T entity in entities)
            {
                entity.CreatorId = Id;
                entity.UpdaterId = Id;
                
            }
            return entities;
        }

        //Helper method
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
            /*
             * Why used where T : new() ?
             * CS0304: Cannot create an instance of the variable type because it does not have the new() constraint.
             */
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
                    
                    for (int i = 0; i < relevantEntityPropertiesNames.Count; i++)
                    { 
                        var entityProperty = typeof(T).GetProperty(relevantEntityPropertiesNames[i]);
                        int relevantColumnNumber = _sheet.FindString(relevantExcelColumnsNames[i], false, false).Column; // Make string as parameter
                    try
                    {
                        var excelCellValue = Convert.ChangeType(_sheet.Range[row, relevantColumnNumber].Value, entityProperty!.PropertyType);
                        entityProperty.SetValue(entity, excelCellValue);
                    }
                    catch (Exception e)
                    {
                        //Debug.WriteLine("\n\n" + "some cell in Price column may be empty or wrong"+ "\n\n");
                        //Debug.WriteLine("\n\n"+e+"\n\n");
                        //Debug.WriteLine("Row, do not forget that row start from 2. not 1 or 0 :" + row);
                        //Debug.WriteLine("property index,don't forget index start from 0, not 1 : " + i+"\n\n");
                        throw new Exception("Error happend");
                    }
                        
                        



                    } // end of inner for loop

                        result.Add(entity);


            }// end of outermost for
                return result;

        }

        //Helper method
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

        //Helper method
        public void PopulateForeignKeyPropertyForEachChild<Child,Parent>(string childTablNameInExcel, string parentTableNameInExcel, string foreignKeyPropertyName,List<Child> childs) where Parent:class,IHasEnglishAndArabicName, IHasIdentification 



        {

            // Suppose we have two tables; Category and SubCategory, the relation is one-to-many.
            // The SubCategory Table  has two columns; EnglishName and CategoryId.
            // The job of this method is  to populate the CategoryId with the correct values.
            // EnglishName  alone is not sufficient to complete the job; a mapping  between  SubCategory and Category tables is required. 
            // This mapping is called 'lookup table', it has more than one record, each record contains a mapping between  one SubCategory and one Category.
            ///For each record, we can query the  relevant row in the Category Table; the row with the same EnglishName as the record.
            // The row that we found contains the Id; it's value will be used  as the value for the foreign key.
            
            List<RelationshipLookupRecord> records =CreateLookupTable( childTablNameInExcel,  parentTableNameInExcel);// Constructs a lookup table.



            // 2.SELECT UNIQUE RECORDS 
            records = records
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .OrderBy(r => r.LeftColumnCellValue)
                .ToList();


            if (records.Count > childs.Count)
            {

                throw new ArgumentException("childs length can't be less than the number of the records in the lookup table.\n");
            }


            //Refer to the parent to be able to set the foreign key.
            DbSet<Parent> dbSet = _context.Set<Parent>();
            PropertyInfo foreignKeyProperty=typeof(Child).GetProperty(foreignKeyPropertyName)!;
            List <Parent> parentRows= dbSet.ToList<Parent>();
            for (int i=0; i < records.Count; i++) 
            {

                //PROBLEM : use try catch blocks
                Parent foundEntity=parentRows.First<Parent>(r => r.EnglishName == records[i].RightColumnCellValue);

                foreignKeyProperty.SetValue(childs[i], foundEntity.Id); 
            }            
                
            
            
        }

        //Helper method
        /// <summary>
        /// <para>// Should I redesign the  function so that it gets the data of the parent from the database instead of the excel file</para>
        /// </summary>
        /// <param name="childTablNameInExcel"></param>
        /// <param name="parentTableNameInExcel"></param>
        /// <returns></returns>
        public List<RelationshipLookupRecord> CreateLookupTable(string childTablNameInExcel,string parentTableNameInExcel)
        {
            /*
             * Error 5000. System.NullReferenceException: 'Object reference not set to an instance of an object.'

_sheet was null.

               _sheet is not defined here. solution in very brief
             */
            int leftColumnNumber = this._sheet.FindString(childTablNameInExcel, false, false).Column;
            int rightColumnNumber = _sheet.FindString(parentTableNameInExcel, false, false).Column;
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

        //Helper method
        public List<J> ConstructJunctionTableData<J,Parent1,Parent2>(string firstPropertyName, string secondPropertyName, string firstExcelColumnName, string secondExcelColumnName) where J : new() where Parent1 : class, IHasIdentification, IHasEnglishAndArabicName, new() where Parent2 : class, IHasIdentification, IHasEnglishAndArabicName, new()

        {
            
            // junction table
            List<J> result = new List<J>();
            
            // Lookup table
            List<RelationshipLookupRecord> lookups =
                CreateLookupTable(firstExcelColumnName, secondExcelColumnName)
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .ToList();


            // retrive all parents into memory
            DbSet<Parent1> DbSet1 = _context.Set<Parent1>();
            List<Parent1> existingEntities1 = DbSet1
                .Select(p => new Parent1 { Id = p.Id, EnglishName = p.EnglishName})
                .ToList();
            DbSet<Parent2> DbSet2 = _context.Set<Parent2>();
            List<Parent2> existingEntities2 = DbSet2.ToList();


            var Property1 = typeof(J).GetProperty(firstPropertyName)!;
            var Property2 = typeof(J).GetProperty(secondPropertyName)!;

            foreach (RelationshipLookupRecord lookup in lookups)
            {
                try
                {
                    Parent1 Entity1 = existingEntities1.First<Parent1>(e => lookup.LeftColumnCellValue == e.EnglishName);

                    Parent2 Entity2 = existingEntities2.First<Parent2>(e => lookup.RightColumnCellValue == e.EnglishName);
                    J j = new J();
                    Property1.SetValue(j, Entity1.Id);
                    Property2.SetValue(j, Entity2.Id);
                    result.Add(j);

                }
                
                catch (System.InvalidOperationException ioe)
                {
                    Debug.WriteLine("catch block executed");
                    Debug.WriteLine("Executed catch block when Entity1 or Entity2 is null");
                    Debug.WriteLine("\n\ncurrent lookup is" + lookup + "\n\n");
                }



            }

            return result;
        }

        
        
        public void AddRange<M>(List<M> models)
            where M:class,IHasEnglishAndArabicName
        {

            DbSet<M> genericContext = _context.Set<M>();
            List<string?>? existingEntitiesNames = genericContext // nullability  should match
              .Select(e => e.EnglishName)
              .ToList();
            var newEntities = models.Where(model => !existingEntitiesNames.Contains(model.EnglishName))
                .ToList();
            if (newEntities.Count()!=0)
            {
                _context.AddRange(newEntities);
            }
            
        }
        
        //Helper method
        public void AddJunctionTableDataToContext<J>(string firstPropertyName, string secondPropertyName, List<J> models) where J : class, new()


        {
            DbSet<J> gDbSet = _context.Set<J>();

            var property1 = typeof(J).GetProperty(firstPropertyName)!;
            var property2 = typeof(J).GetProperty(secondPropertyName)!;
   
            List<J> existingEntities = gDbSet.ToList();


            //Where clause : Only intrested about the models that does not  exist in the dataabse
            // Any: tells us if something exist in something else
            // NOT(Any) clause: tells us if something does not exist in something else
            // for each model that does not exist in the database, the NOT(Any) will return true.
            var newEntities = models
             .Where(model => !existingEntities
                    .Any(e =>
                     property1.GetValue(e).Equals(property1.GetValue(model)) &&
                     property2.GetValue(e).Equals(property2.GetValue(model))))
            .ToList();


            if (newEntities.Count() != 0)
            {
                _context.AddRange(newEntities);
            }
            


        }



            }


        }
        // helper method







