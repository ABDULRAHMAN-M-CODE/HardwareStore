using HardwareStore.Models;
using HardwareStoreNameSpace;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;
using Spire.Xls.Core;
namespace HardwareStore.Services
{
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ReadExcelWriteDatabase(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        /// <summary>
        /// Reads Excel data for parent database tables (tables without foreign keys) by calling the "ReadSpecificColumns" method
        /// It does  read data for child tables (tables containing foreign keys) indirectly by calling the  "ReadDataForChildTables"  method.
        /// </summary>
        /// <returns>The Excel data read for the parent tables.</returns>
        public object ReadData()// Interface is Unified between all classes. Read can return anything
        {
            var httpcontext = _httpContextAccessor.HttpContext;
            var file = httpcontext.Request.Form.Files["file"];

            var workbook = new Workbook();
            using var stream = file.OpenReadStream();
            workbook.LoadFromStream(stream);
            Worksheet sheet = workbook.Worksheets[0];


            List<Brand> brands = ReadExcelWriteDatabase.ReadSpecificColumns<Brand>(new List<string> { "EnglishName" }, new List<string> { "Brand" }, sheet);
            List<Category> categories = ReadExcelWriteDatabase.ReadSpecificColumns<Category>(new List<string> { "EnglishName" }, new List<string> { "Category" }, sheet);

            List<Country> countries = ReadExcelWriteDatabase.ReadSpecificColumns<Country>(new List<string> { "EnglishName" }, new List<string> { "Country" }, sheet);

            List<Supplier> suppliers = ReadExcelWriteDatabase.ReadSpecificColumns<Supplier>(new List<string> { "EnglishName" }, new List<string> { "Supplier" }, sheet);
            List<Unit> units = ReadExcelWriteDatabase.ReadSpecificColumns<Unit>(new List<string> { "EnglishName" }, new List<string> { "Unit" }, sheet);

            //List<SubCategory> ReadDataForChildTables(Worksheet sheet)
            List<SubCategory> subCategories = ReadDataForChildTables(sheet); // PROBELM : Should I make it generic method ?


            return new List<object> { units, brands, categories, countries, suppliers,subCategories};
        }

       
        //PROBLEM : Change The parameter from List<T> to object, the interface must be unified.
        public async Task WriteData<T>(List<T> entities) where T :class, IHasEnglishAndArabicName    //This is called the constraint list
        {
            // Set Returns DbSet<T> that can be used to query the entity T
            //T is not hardcoded, it's generic, it's reusable.
            DbSet<T> dbSet = _context.Set<T>();// CS0452 : T must be a reference type in order to use it as parameter: solution → T:class
            foreach (T entity in entities)
            {

                // check if the entity already in the database or not
                var foundEntity = await dbSet.FirstOrDefaultAsync(e => e.EnglishName == entity.EnglishName);//PROBLEM: EnglishName is harcoded, what if the EnglishName property changes?

                if (foundEntity == null)
                {




                    dbSet.Add(entity);// NoteXXXXX: This code was executed



                }
            }
            await _context.SaveChangesAsync();
        }

        // static;  does not access member data
        private static List<T> ReadSpecificColumns<T>(List<string> relevantEntityPropertiesNames, List<string> relevantExcelColumnsNames, Worksheet sheet) where T : IHasEnglishAndArabicName, new()
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
            // Logic : I want loop over all the rows
            for (int row = 2; row <= sheet.LastRow; row++)//assuming row 1 is header, want skip it.
            {

                T entity = new T();
                Type type = typeof(T);

                // Populate all the relevant properties of a single Entity
                //Loop over specific columns only
                for (int i = 0; i < relevantEntityPropertiesNames.Count; i++)// method parameter
                {



                    var entityProperty = type.GetProperty(relevantEntityPropertiesNames[i]);
                    // Inserts a space before any uppercase letter that follows a lowercase letter

                    //string relevantHeaderName = Regex.Replace(relevantPropertyName, "(?<=[a-z])(?=[A-Z])", " ");

                    //int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column;
                    // PROBLEM : should Excel column be parameter or computed ?

                    int relevantColumnNumber = sheet.FindString(relevantExcelColumnsNames[0], false, false).Column; // Make string as parameter



                    var excelCellValue = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, entityProperty!.PropertyType);


                    entityProperty.SetValue(entity, excelCellValue); // MAKE this generic



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
        /// This method reads data for Child tables (Tables that contains a single).
        /// Foreign key values for child tables are populated  using lookups
        /// For reading against the corresponding parent tables.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private List<SubCategory> ReadDataForChildTables(Worksheet sheet)
        {
            List<CategorySubCategoryRecord> records = new();
            int categoryColumn = sheet.FindString("Category", false, false).Column;
            int subCategoryColumn = sheet.FindString("SubCategory", false, false).Column;

            for (int row = 2; row <= sheet.LastRow; row++)
            {
                records.Add(new CategorySubCategoryRecord
                {
                    CategoryName = sheet.Range[row, categoryColumn].Value,
                    SubCategoryName = sheet.Range[row, subCategoryColumn].Value
                });
            }
            records = records
                .DistinctBy(r => new { r.CategoryName, r.SubCategoryName })
                .OrderBy(r => r.CategoryName)
                .ToList();

            List<SubCategory> result = new List<SubCategory>();
            foreach (CategorySubCategoryRecord record in records)
            {


                //  did not use .FirstAsync for specific purpose.
                var category = _context.Categories
                    .First(c => c.EnglishName == record.CategoryName);

                var subCategory = new SubCategory
                {
                    EnglishName = record.SubCategoryName,
                    CategoryId = category.Id
                };

                result.Add(subCategory);
            }
            return result;
        }

    }



}

