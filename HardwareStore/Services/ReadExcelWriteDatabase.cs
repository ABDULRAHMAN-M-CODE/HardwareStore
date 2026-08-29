using HardwareStore.Models;
using HardwareStoreNameSpace;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;
using Spire.Xls.Core;
using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HardwareStore.Services
{
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public  Worksheet _sheet;// populated inside the  ReadAndWriteData() method.

        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            //_sheet = sheet;// did not work
        }

        public void ReadAndWriteData()// LATER :refactor this  method to only read, not to also write data.
        {


            //PROBLEM : I should refactor the code later, so that the ReadData() only reads, not also writes.

            var httpcontext = _httpContextAccessor.HttpContext;
            var file = httpcontext.Request.Form.Files["file"];
            var workbook = new Workbook();
            using var stream = file.OpenReadStream();
            workbook.LoadFromStream(stream);
            this._sheet = workbook.Worksheets[0];

            List<Category> categories = ReadSpecificColumns<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" });
            WriteData(categories);// I know I'm mixing reading and writing in the same method, but I had no choice, I suffered alot.
            List<SubCategory> subCategories = ReadSpecificColumns<SubCategory>(new List<string> { "EnglishName" }, new List<string> { "Subcategory" });
            subCategories = subCategories // subCategories records is already unique
            .OrderBy(c => c.EnglishName)
            .ToList();
            PopulateForeignKeyPropertyForEachChild<SubCategory, Category>("Subcategory", "Category", "CategoryId", subCategories);
            WriteData(subCategories);// I know I'm mixing  reading and writing in the same method, but I had no choice, I suffered alot.


            List<Country> countries = ReadSpecificColumns<Country>(new List<string> { "EnglishName" }, new List<string> { "Country" });
            WriteData(countries);
            List<Unit> units = ReadSpecificColumns<Unit>(new List<string> { "EnglishName" }, new List<string> { "Unit" });
            WriteData(units);
            List<Brand> brands = ReadSpecificColumns<Brand>(new List<string> { "EnglishName" }, new List<string> { "Brand" });
            WriteData(brands);
            List<Supplier> suppliers = ReadSpecificColumns<Supplier>(new List<string> { "EnglishName" }, new List<string> { "Supplier" });
            WriteData(suppliers);


            //Excel file does not provide the data for the brandsSuppliers,
            // brandsSuppliers is not inherently unique
            List<BrandSupplier> brandSuppliers = new List<BrandSupplier>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                brandSuppliers.Add(new BrandSupplier());
            }
            PopulateForeignKeyPropertyForEachChild<BrandSupplier, Brand>("Supplier", "Brand", "BrandId", brandSuppliers);
            //brandSuppliers = brandSuppliers.OrderBy(bs => bs.SupplierId).ToList();// Wrong
            brandSuppliers = brandSuppliers.
                OrderBy(bs => bs.BrandId).
                ToList();


            // At this point, all values for SupplierId are still 0, therefore, filtering using the 'where caluse' should not be applied here,  
            PopulateForeignKeyPropertyForEachChild<BrandSupplier, Supplier>("Brand", "Supplier", "SupplierId", brandSuppliers);
            

                // very necessary before the distinct.
                brandSuppliers=brandSuppliers
                .Where(bs => bs.SupplierId != 0 && bs.BrandId!=0) // filtering the non-zero records
                .DistinctBy(bs => new { bs.SupplierId, bs.BrandId })
                .ToList();
            //WriteData(brandsSuppliers); // Wrong, can't do this, brandsSuppliers does not have English Name
            foreach (BrandSupplier bs in brandSuppliers)
            {

                // check if the entity already in the database or not
                var foundEntity = _context.BrandsSuppliers.FirstOrDefault(e => (e.BrandId == bs.BrandId) && (e.SupplierId == bs.SupplierId));

                if (foundEntity == null)
                {




                    _context.Add(bs);



                }
            }


            

            //Reading and Writing for Products table.
            List<Product> products = ReadSpecificColumns<Product>(

            new List<string> { "SKU", "Barcode", "Manufacturer", "EnglishName", "ArabicName", "MinStock", "ReorderQty", "Bin", "Description", "Status", "VAT" },

            new List<string> { "SKU", "Barcode", "Manufacturer", "English Name", "Arabic Name", "MinStock", "ReorderQty", "Bin", "Description", "Status", "VAT" }

            );
            products = products 
           .OrderBy(p => p.EnglishName)//Necessary to order the left column in the Child table before calling the below method. unless we are dealing with zeroed column.
            .ToList();
            PopulateForeignKeyPropertyForEachChild<Product, Unit>("English Name", "Unit", "UnitId", products);
            products = products  // increased saftey against failure
            .OrderBy(p => p.EnglishName)
            .ToList();
            PopulateForeignKeyPropertyForEachChild<Product, Category>("English Name", "Category", "CategoryId", products);
            WriteData<Product>(products);



            // #####Reading and Writing for ProductsCountries table.#####
            List<ProductCountry> productCountries = new List<ProductCountry>();
            for (int rowNumber = 2; rowNumber <= _sheet.LastRow; rowNumber++)
            {

                productCountries.Add(new ProductCountry());
            }
            PopulateForeignKeyPropertyForEachChild<ProductCountry, Product>("Country", "English Name", "ProductId", productCountries);
            productCountries = productCountries

                
                .OrderBy(bc => bc.ProductId)
                .ToList();
            //problema was not solved
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
            productSuppliers = productSuppliers




                .OrderBy(ps => ps.ProductId)
                .ToList();
            PopulateForeignKeyPropertyForEachChild<ProductSupplier, Supplier>("English Name","Supplier", "SupplierId", productSuppliers);
            productSuppliers = productSuppliers

                //.Where(ps => ps.SupplierId != 0) // Wrong, because some values of the ProductId might be zero.
                .Where(ps => ps.SupplierId != 0 && ps.ProductId != 0) // analogy L: cut the unwanted, the empty or the unwanted  portion pucket.
                .DistinctBy(ps => new { ps.ProductId, ps.SupplierId })
                .ToList();

            // note :at this point, productSuppliers length is 1998. 
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
            productBrands = productBrands




                .OrderBy(pb => pb.ProductId)
                .ToList();
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



            _context.SaveChanges();
             Debug.WriteLine(" My break point.");
            // I should make this method return something later. instead of mixing reading and writing
            //return new List<object> { categories, subCategories,units,countries, brands, suppliers, brandSuppliers };


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
        /// Reads Data from the specified   Excel sheet that is related 
        /// to tables that DO NOT contain any foreign key,
        /// i.e., the parent tables only.
        /// this entities returned by this method are unique.
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
                List<object> duplicatesEntitiesNames = new List<object>();
                //scan all the rows 
                for (int row = 2; row <= (_sheet.LastRow); row++)//assuming row 1 is header, want skip it.
                {

                
                    //scan specific columns
                    // Populate all the relevant properties of a single Entity 
                    T entity = new T();
                    Type type = typeof(T);
                    for (int i = 0; i < relevantEntityPropertiesNames.Count; i++)
                    { 
                        var entityProperty = type.GetProperty(relevantEntityPropertiesNames[i]);
                        int relevantColumnNumber = _sheet.FindString(relevantExcelColumnsNames[i], false, false).Column; // Make string as parameter
                        var excelCellValue = Convert.ChangeType(_sheet.Range[row, relevantColumnNumber].Value, entityProperty!.PropertyType);
                        entityProperty.SetValue(entity, excelCellValue); 



                    } // end of inner for loop

                    if (!duplicatesEntitiesNames.Contains(entity.EnglishName!))//T : IHasEnglishAndArabicName solved that
                    {
                        result.Add(entity);
                        duplicatesEntitiesNames.Add(entity.EnglishName!); //T : IHasEnglishAndArabicName solved that
                    }



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
        ///  the foreign key is not guaranteed to contain non-zero values, so it's up to the caller
        ///  to filter out the zero values.
        /// </para>
        /// 
        /// <para>
        ///  if you want to set the values for <strong>two foreing keys</strong>, call this method twice, the first call
        ///  is to set the first foreign key, the second call is to set the second foreign key,
        ///  only after the second call you are allowed to filter out the non-zero values,
        ///  you should not filter out the non-zero values after the first call of this method.
        ///  
        ///  <para>
        ///  
        ///  For example, if you have two foreing keys;  <strong>CategoryId</strong> and <strong>ProductId</strong>, call this method
        ///  once to set the values for the <strong>ProductId</strong>, some values might be zero, but <strong>don't</strong> filter them
        ///  out yet. then, <strong>Sort</strong> the values of the <strong>ProductId</strong> then  call this method again to set the values for the <strong>CategoryId</strong>,
        ///  After the second call, you can filter out the zeros, both <strong>CategoryId</strong>
        ///  and <strong>ProductId</strong> are not allowed to contain zeros.
        ///  </para>
        ///  </para>
        ///   
        /// 
        /// 
        ///   <strong>Notes for my self</strong>
        ///   <para>In the case of many-to-1 relationship, there is a child and parent tables.</para>
        /// 
        ///   
        ///    Child is the table that contains one or more foreing keys, while parent is the table that is referenced by the foreign key.
        ///       
        /// 
        ///    <para>in database terms: This method sets the values of the foreign-key column that is inside the child table. </para>
        ///     
        ///    <para> in objects terms: each row in the Child table is considered object.</para>   
        ///    
        ///    <para>As a single row can have multiple columns, a single object my have multiple properties,</para>
        ///     
        ///    <para>one or more of those properties  might be mapped to a foreign key in the database</para>  
        ///  
        /// 
        ///     
        ///     <para>
        ///     The  passed child table is called <strong>Special Child</strong> if: <br/>
        ///     1- It's a <strong>junction</strong> table; only contains foreing keys <br/> 
        ///     2- It is not populated with values , i.e., <strong>either empty or zeroed</strong><br/>
        ///     </para>
        ///     
        ///      <para>
        ///     If the passed child does not qualify as a 
        ///     <strong>Special Child</strong>strong>', then 
        ///     it's  rows must be <strong>sorted</strong> by the
        ///     primary key before calling  this method, <strong>otherwise</strong>
        ///     , there is <strong>no need </strong>  for sorting.
        ///     </para>
        ///     <para>
        ///       
        ///     A lookup table, called records
        ///     will will
        ///     Child table's size must be
        ///     greater or equal than the records
        ///     , 
        ///     THis method does not write the values to the database, it's completely a read operation, the destination if the  foreign key property, not the foreign key in the database.
        /// 
        ///     </para>
        /// </summary>
        /// <typeparam name="Child"></typeparam>
        /// <typeparam name="Parent"></typeparam>
        /// <param name="childTablNameInExcel"></param>
        /// <param name="parentTableNameInExcel"></param>
        /// <param name="foreignKeyPropertyName"></param>
        /// <param name="childs"></param>
        /// <exception cref="ArgumentException"></exception>        
        public void PopulateForeignKeyPropertyForEachChild<Child,Parent>(string childTablNameInExcel, string parentTableNameInExcel, string foreignKeyPropertyName,List<Child> childs) where Parent:class,IHasEnglishAndArabicName,IHasIdentification where Child:new()



        {
            // 1.Construct a lookup table.

            List<RelationshipLookupRecord> records =CreateLookupTable( childTablNameInExcel,  parentTableNameInExcel);



            // 2.SELECT UNIQUE RECORDS 
            records = records
                .DistinctBy(r => new { r.LeftColumnCellValue, r.RightColumnCellValue })
                .OrderBy(r => r.LeftColumnCellValue)
                .ToList();

            // analogy : like small empty bucket of water that is filled by bigger bucket of water, water will eventually overflow
            // similarly, if the records (the bigger bucket) is bigger than the childs (smaller bucket), index out of bound will happen.
            if (records.Count > childs.Count)
            {
                
                throw new ArgumentException("childs length can't be less than the number of the records in the lookup table.\n");


            }
            //childs = childs // does not work
            //.OrderBy(c => c.EnglishName)
            //.ToList();

            //Refer to the parent to be able to set the foreign key.
            DbSet<Parent> dbSet = _context.Set<Parent>();
            //List<Child> result = new List<Child>();
                    
            //foreach (RelationshipLookupRecord record in records)  {}

            for (int i=0; i < records.Count; i++) // looping over childs is wrong, either single child is populated or Index out of about occur
            {

                //  did not use .FirstAsync for specific purpose.
                Parent foundEntity = dbSet
                    .First(e => e.EnglishName == records[i].RightColumnCellValue);// PROBLEM : EnglishName is harcoded.

                //reflection : because c# is not dynamic language
                typeof(Child).GetProperty(foreignKeyPropertyName)!.SetValue(childs[i], foundEntity.Id); 
            }            
                //result.Add(child);// changed my approach
            
            //return result;// changed my approach
        }

 
        /// <summary>
        /// describe it later
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

