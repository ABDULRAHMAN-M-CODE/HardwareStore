

namespace HardwareStore.Services

{
    using HardwareStore.Models;
    using HardwareStoreNameSpace;
    using Microsoft.EntityFrameworkCore;
    using Spire.Xls;
    using System.Diagnostics;
    using System.Security.Claims;
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public  Worksheet _sheet;// populated inside the  ReadAndWriteData() method.

        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            
        }

        public void ReadAndWriteData()// LATER :refactor this  method to only read, not to also write data.
        {


            //PROBLEM : I should refactor the code later, so that the ReadData() only reads, not also writes.

            
            var httpcontext = _httpContextAccessor.HttpContext;
            
            var file = httpcontext.Request.Form.Files["file"];
            var workbook = new Workbook();
            using var stream = file.OpenReadStream();
            workbook.LoadFromStream(stream);
            this._sheet = workbook.Worksheets[1];

            var adminId = httpcontext.User.FindFirstValue(ClaimTypes.NameIdentifier);






            List<Category> categories = ReadSpecificColumns<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" });
            categories = categories.DistinctBy(c => c.EnglishName).ToList();
            //List<T> AssignCreatorAndUpdaterToEntitis<T>(List<T> entities, string Id)
            categories = AssignCreatorAndUpdaterToEntitis<Category>(categories, adminId);
            
            WriteData(categories);// I know I'm mixing reading and writing in the same method, but I had no choice, I suffered alot.
            

            List<SubCategory> subCategories = ReadSpecificColumns<SubCategory>(new List<string> { "EnglishName" }, new List<string> { "Subcategory" });
            subCategories = subCategories.DistinctBy(sc => new { sc.EnglishName, sc.CategoryId }).ToList();
            subCategories = subCategories.OrderBy(c => c.EnglishName).ToList();
            PopulateForeignKeyPropertyForEachChild<SubCategory, Category>("Subcategory", "Category", "CategoryId", subCategories);
            subCategories = AssignCreatorAndUpdaterToEntitis<SubCategory>(subCategories, adminId);     
            WriteData(subCategories);

            
            List<Country> countries = ReadSpecificColumns<Country>(new List<string> { "EnglishName" }, new List<string> { "Country" });
            countries = countries.DistinctBy(c => c.EnglishName).ToList();
            countries = AssignCreatorAndUpdaterToEntitis<Country>(countries, adminId);
            WriteData(countries);

            List<Unit> units = ReadSpecificColumns<Unit>(new List<string> { "EnglishName" }, new List<string> { "Unit" });
            units = units.DistinctBy(u => u.EnglishName).ToList();
            units = AssignCreatorAndUpdaterToEntitis<Unit>(units, adminId);
            WriteData(units);
            
            List<Brand> brands = ReadSpecificColumns<Brand>(new List<string> { "EnglishName" }, new List<string> { "Brand" });
            brands = brands.DistinctBy(b => b.EnglishName).ToList();
            brands = AssignCreatorAndUpdaterToEntitis<Brand>(brands, adminId);
            WriteData(brands);
            
            List<Supplier> suppliers = ReadSpecificColumns<Supplier>(new List<string> { "EnglishName" }, new List<string> { "Supplier" });
            suppliers = suppliers.DistinctBy(s => s.EnglishName).ToList();
            suppliers = AssignCreatorAndUpdaterToEntitis<Supplier>(suppliers, adminId);
            WriteData(suppliers);


            
            

            //Reading and Writing for Products table.
            List<Product> products = ReadSpecificColumns<Product>(

            new List<string> { "SKU", "Barcode",  "EnglishName", "ArabicName", "Description", "Status", "VAT","Price","MinStock","ReorderQTY" },

            new List<string> { "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT","Price","Min Stock", "Reorder Qty" }
            
            );
            products = products.DistinctBy(p => p.EnglishName).ToList();
            products =AssignCreatorAndUpdaterToEntitis<Product>(products, adminId);
            WriteData<Product>(products);

           


            // Reading and writing Bins table.
            List<Bin> bins = ReadSpecificColumns<Bin>(new List<string> { "EnglishName" }, new List<string> { "Bin" });
            bins = bins.DistinctBy(c => c.EnglishName).ToList();
            bins = AssignCreatorAndUpdaterToEntitis<Bin>(bins, adminId);
            WriteData(bins);// I know I'm mixing reading and writing in the same method, but I had no choice, I suffered alot.


            List<Manufacturer> manufacturers = ReadSpecificColumns<Manufacturer>(new List<string> { "EnglishName" }, new List<string> { "Manufacturer" });
            manufacturers = manufacturers.DistinctBy(m => m.EnglishName).ToList();
            manufacturers = AssignCreatorAndUpdaterToEntitis<Manufacturer>(manufacturers, adminId);
            WriteData(manufacturers);




        /////#### Reading and Writing junction tables######

            // Reading and Writing BrandsSuppliers.
            List<BrandSupplier> brandSuppliers = new List<BrandSupplier>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                brandSuppliers.Add(new BrandSupplier());
            }
            PopulateForeignKeyPropertyForEachChild<BrandSupplier, Brand>("Supplier", "Brand", "BrandId", brandSuppliers);
            PopulateForeignKeyPropertyForEachChild<BrandSupplier, Supplier>("Brand", "Supplier", "SupplierId", brandSuppliers);
            brandSuppliers = brandSuppliers
                .Where(bs => bs.SupplierId != 0 && bs.BrandId != 0)
                .DistinctBy(bs => new { bs.SupplierId, bs.BrandId })
                .ToList();
            foreach (BrandSupplier bs in brandSuppliers)
            {

                // check if the entity already in the database or not
                var foundEntity = _context.BrandsSuppliers.FirstOrDefault(e => (e.BrandId == bs.BrandId) && (e.SupplierId == bs.SupplierId));

                if (foundEntity == null)
                {

                    _context.Add(bs);

                }
            }


            // #####Reading and Writing for ProductsManufacturers table.#####
            List<ProductManufacturer> productManufacturers = new List<ProductManufacturer>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productManufacturers.Add(new ProductManufacturer());
            }
            PopulateForeignKeyPropertyForEachChild<ProductManufacturer, Product>("Manufacturer", "English Name", "ProductId", productManufacturers);
            PopulateForeignKeyPropertyForEachChild<ProductManufacturer, Manufacturer>("English Name", "Manufacturer", "ManufacturerId", productManufacturers);
            productManufacturers = productManufacturers
                .Where(pm => pm.ManufacturerId != 0 && pm.ProductId != 0)
                .DistinctBy(pm => new { pm.ProductId, pm.ManufacturerId })
                .ToList();
            foreach (ProductManufacturer pm in productManufacturers)
            {
                var foundEntity = _context.ProductsManufacturers.FirstOrDefault(e => (e.ProductId == pm.ProductId) && (e.ManufacturerId == pm.ManufacturerId));

                if (foundEntity == null)
                {

                    _context.Add(pm);

                }
            }

            // Reading and Writing for ProductsBins table.
            List <ProductBin> productBins = new List<ProductBin>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productBins.Add(new ProductBin());
            }
            PopulateForeignKeyPropertyForEachChild<ProductBin, Product>("Bin", "English Name", "ProductId", productBins);
            PopulateForeignKeyPropertyForEachChild<ProductBin, Bin>("English Name", "Bin", "BinId", productBins);
            productBins = productBins
                .Where(pb => pb.BinId != 0 && pb.ProductId != 0)
                .DistinctBy(pb => new { pb.ProductId, pb.BinId })
                .ToList();
            foreach (ProductBin pb in productBins)
            {
                var foundEntity = _context.ProductsBins.FirstOrDefault(e => (e.ProductId == pb.ProductId) && (e.BinId == pb.BinId));

                if (foundEntity == null)
                {

                    _context.Add(pb);

                }
            }


            // #####Reading and Writing for ProductsCountries table.#####
            List<ProductCountry> productCountries = new List<ProductCountry>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productCountries.Add(new ProductCountry());
            }
            PopulateForeignKeyPropertyForEachChild<ProductCountry, Product>("Country", "English Name", "ProductId", productCountries);
            PopulateForeignKeyPropertyForEachChild<ProductCountry, Country>("English Name", "Country", "CountryId", productCountries);
            productCountries = productCountries
                .Where(bc => bc.CountryId != 0 && bc.ProductId != 0)
                .DistinctBy(bs => new { bs.ProductId, bs.CountryId })
                .ToList();
            foreach (ProductCountry pc in productCountries)
            {
                var foundEntity = _context.ProductsCountries.FirstOrDefault(e => (e.ProductId == pc.ProductId) && (e.CountryId == pc.CountryId));

                if (foundEntity == null)
                {

                   _context.Add(pc);

                }
            }

           
            //#####Reading and Writing for ProductsSuppliers table.#####
            List<ProductSupplier> productSuppliers = new List<ProductSupplier>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productSuppliers.Add(new ProductSupplier());
            }// note :at this point, productSuppliers length is 5000.
            PopulateForeignKeyPropertyForEachChild<ProductSupplier, Product>("Supplier", "English Name", "ProductId", productSuppliers);
            PopulateForeignKeyPropertyForEachChild<ProductSupplier, Supplier>("English Name","Supplier", "SupplierId", productSuppliers);
            productSuppliers = productSuppliers
                .Where(ps => ps.SupplierId != 0 && ps.ProductId != 0) // analogy L: cut the unwanted, the empty or the unwanted  portion pucket.
                .DistinctBy(ps => new { ps.ProductId, ps.SupplierId })
                .ToList(); 
            foreach (ProductSupplier ps in productSuppliers)
            {

                var foundEntity = _context.ProductsSuppliers.FirstOrDefault(e => (e.ProductId == ps.ProductId) && (e.SupplierId == ps.SupplierId));
                if (foundEntity == null)
                {

                    _context.Add(ps);

                }
            }

          

            List<ProductBrand> productBrands = new List<ProductBrand>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productBrands.Add(new ProductBrand());
            }
            PopulateForeignKeyPropertyForEachChild<ProductBrand, Product>( "Brand", "English Name", "ProductId", productBrands);
            PopulateForeignKeyPropertyForEachChild<ProductBrand, Brand>( "English Name", "Brand", "BrandId", productBrands);
            productBrands = productBrands

                .Where(pb => pb.BrandId != 0 && pb.ProductId != 0) // LINQ
                .DistinctBy(pb => new { pb.ProductId, pb.BrandId })  
                .ToList();
            foreach (ProductBrand pb in productBrands)
            {

                var foundEntity = _context.ProductsBrands.FirstOrDefault(e => (e.ProductId == pb.ProductId) && (e.BrandId == pb.BrandId));

                if (foundEntity == null)
                {

                    _context.Add(pb);

                }
            }



            // Reading and Writing ProductsCategories.
            // later when refactoring code : await two dependencies.
            List<ProductCategory> productCategories = new List<ProductCategory>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productCategories.Add(new ProductCategory());
            }
            PopulateForeignKeyPropertyForEachChild<ProductCategory, Product>("Category", "English Name", "ProductId", productCategories);
            PopulateForeignKeyPropertyForEachChild<ProductCategory, Category>("English Name", "Category", "CategoryId", productCategories);
            productCategories = productCategories

                .Where(pc => pc.CategoryId != 0 && pc.ProductId != 0) // LINQ
                .DistinctBy(pc => new { pc.ProductId, pc.CategoryId })
                .ToList();
            foreach (ProductCategory pc in productCategories)
            {

                var foundEntity = _context.ProductsCategories.FirstOrDefault(e => (e.ProductId == pc.ProductId) && (e.CategoryId == pc.CategoryId));

                if (foundEntity == null)
                {

                    _context.Add(pc);

                }
            }



            // Reading and Writing ProductsUnits.
            // later when refactoring code : await two dependencies.
            List<ProductUnit> productUnits = new List<ProductUnit>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productUnits.Add(new ProductUnit());
            }
            PopulateForeignKeyPropertyForEachChild<ProductUnit, Product>("Unit", "English Name", "ProductId", productUnits);
            PopulateForeignKeyPropertyForEachChild<ProductUnit, Unit>("English Name", "Unit", "UnitId", productUnits);
            productUnits = productUnits
                .Where(pu => pu.UnitId != 0 && pu.ProductId != 0) // LINQ
                .DistinctBy(pu => new { pu.ProductId, pu.UnitId })
                .ToList();
            foreach (ProductUnit pu in productUnits)
            {

                var foundEntity = _context.ProductsUnits.FirstOrDefault(e => (e.ProductId == pu.ProductId) && (e.UnitId == pu.UnitId));

                if (foundEntity == null)
                {

                    _context.Add(pu);

                }
            }

            _context.SaveChanges();
             Debug.WriteLine(" My break point.");
            // I should make this method return something later. instead of mixing reading and writing
            //return new List<object> { categories, subCategories,units,countries, brands, suppliers, brandSuppliers };


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
        public void WriteData<T>(List<T> entities) where T :class, IHasEnglishAndArabicName    //This is called the constraint list
        {
            // Set Returns DbSet<T> that can be used to query the entity T
            //T is not hardcoded, it's generic, it's reusable.
            
            DbSet<T> dbSet = _context.Set<T>();// CS0452 : T must be a reference type in order to use it as parameter: solution → T:class
            foreach (T entity in entities)
            {

                // check if the entity already in the database or not
                var foundEntity = dbSet.FirstOrDefault(e => e.EnglishName == entity.EnglishName);//PROBLEM: EnglishName is harcoded, what if the EnglishName property changes?

                if (foundEntity == null)
                {




                    dbSet.Add(entity);// Debugging: This code was executed



                }
            }
             _context.SaveChanges();
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
                        Debug.WriteLine("\n\n" + "some cell in Price column may be empty or wrong"+ "\n\n");
                        Debug.WriteLine("\n\n"+e+"\n\n");
                        Debug.WriteLine("Row, do not forget that row start from 2. not 1 or 0 :" + row);
                        Debug.WriteLine("property index,don't forget index start from 0, not 1 : " + i+"\n\n");
                        throw new Exception("Error happend");
                    }
                        
                        



                    } // end of inner for loop

                        result.Add(entity);


            }// end of outermost for
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
            for (int i=0; i < records.Count; i++) 
            {      
                Parent foundEntity = dbSet
                    .First(e => e.EnglishName == records[i].RightColumnCellValue);

                //reflection : because c# is not dynamic language
                typeof(Child).GetProperty(foreignKeyPropertyName)!.SetValue(childs[i], foundEntity.Id); 
            }            
                
            
            
        }


        /// <summary>
        /// <para>// Should I redesign the  function so that it gets the data of the parent from the database instead of the excel file</para>
        /// </summary>
        /// <param name="childTablNameInExcel"></param>
        /// <param name="parentTableNameInExcel"></param>
        /// <returns></returns>
        public List<RelationshipLookupRecord> CreateLookupTable(string childTablNameInExcel,string parentTableNameInExcel)
        {
            int leftColumnNumber = _sheet.FindString(childTablNameInExcel, false, false).Column;
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
    }



}

