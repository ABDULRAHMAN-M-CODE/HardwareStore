using HardwareStore.DTOs;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStoreNameSpace;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;


using System.Reflection;

namespace HardwareStore.Services.AdminServices

// Use PROBLEM: if you want to back to something later.
{
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter,IOmniReaderWriter
    {

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


        public ReadExcelWriteDatabase(IDbContextFactory<ApplicationDbContext> contextFactory)
        {        
            _contextFactory = contextFactory;

        }

        //public ReadExcelWriteDatabase(IExcel excel) // For benchmarking different versions of the `Read(WorkSheet sheet)` method
        //{
        //    _excel = excel;
        //}


        #region interface methods : signatrue should not never be modified unless it's the last resort.

        #region Read(s) method(s)



        #region Base Read
        public async Task<IDataDto> Read(object sheet)
        {


            var castedSheet = (Worksheet)sheet;

            #region reading _sheet in parallel
            var items =new Func<IEnumerable<INonJunctionEntity>>[]
            {
                ()=>ReadSpecificColumns<Category>(["EnglishName"], ["Category"]),////Result[0]

                ()=>ReadSpecificColumns<Country>(["EnglishName"], ["Country"]),// Resul[1]

                ()=>ReadSpecificColumns<Unit>( ["EnglishName"], ["Unit"]),//Resul[2]
               
                ()=>ReadSpecificColumns<Brand>( ["EnglishName"], ["Brand"]), //Resul[3]
               
                ()=>ReadSpecificColumns<Supplier>( ["EnglishName"], ["Supplier"]),//Result[4]

                //Result[5]
                ()=>ReadSpecificColumns<Product>([ "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" ],[ "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" ]),
                
                ()=>ReadSpecificColumns<Bin>( ["EnglishName"], ["Bin"]),//Result[6]

                 ()=>ReadSpecificColumns<Manufacturer>( ["EnglishName"], ["Manufacturer"]),//Result[7]
                 
                 ()=>ReadSpecificColumns<SubCategory>(["EnglishName"], ["Subcategory"])//Result[8]


            };
            var tasks = new List<Task<List<INonJunctionEntity>>>();
            foreach (var item in items)
            {
                tasks.Add(Task.Run(() => item.Invoke().ToList()));
            }

            var continuation = Task.WhenAll(tasks);
            continuation.Wait();

            #endregion

            #region return reults
            return new ExcelProductsDto
            {
                Categories = continuation.Result[0].Cast<Category>().ToList(),
                Countries = continuation.Result[1].Cast<Country>().ToList(),
                Units = continuation.Result[2].Cast<Unit>().ToList(),
                Brands = continuation.Result[3].Cast<Brand>().ToList(),
                Suppliers = continuation.Result[4].Cast<Supplier>().ToList(),
                Products = continuation.Result[5].Cast<Product>().ToList(),
                Bins = continuation.Result[6].Cast<Bin>().ToList(),
                Manufacturers = continuation.Result[7].Cast<Manufacturer>().ToList(),
                SubCategories = continuation.Result[8].Cast<SubCategory>().ToList(),
            };
            #endregion

            #region Local functions


            List<T> ReadSpecificColumns<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T:INonJunctionEntity,ICreator<T>
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
                for (int i = 0; i < numberOfProperties; i++)
                {
                    modelProperties[i] =typeof(T).GetProperty(entityPropertiesNames[i]); // wow removing the generic type made me notice; I can remove refelectio now?
                }
                #endregion

                #region Obtain columns indicies given there names. (OPTIMIZE (OR USE OTHER LIBRARY))
                int[] columnsIndicies = new int[numberOfProperties];
                for (int i = 0; i < numberOfProperties; i++)
                {
                    columnsIndicies[i] = castedSheet.FindString(ExcelColumnsNames[i], false, false).Column;

                }
                #endregion
                

                #region Create an in-memory representation of the Excel data.
                List<T> result = new List<T>();
                for (int row = 2; row <= (castedSheet.LastRow); row++)//Scan all the rows. Skip first row assuming it is a header.
                {

                    T entity = T.Create();

                    //Scan specific columns; populate all the relevant properties of a single Entity  
                    for (int i = 0; i < numberOfProperties; i++)
                    {
                        var excelCellValue = Convert.ChangeType(castedSheet.Range[row, columnsIndicies[i]].Value, modelProperties[i].PropertyType);
                        modelProperties[i].SetValue(entity, excelCellValue);
                    }

                    result.Add(entity);
                }
                #endregion


                return result;

            }

            #endregion

        }
        #endregion

        #region Read the returns DbDataReader to be sent to SqlBulkCopy.


        #endregion

        #endregion


        #region Write(s) method(s)
        public async Task Write(IDataDto dataDto)
        {
            var incomingData = (ExcelProductsDto)dataDto; // contains alot of duplicated data
            
            #region delete all data from the database
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXECUTE uspDeleteAllDataFromDatabase"
                );
            }
            #endregion

            #region deduplicate parents
            var uniqueSuppliers = incomingData.Suppliers.DistinctBy(supplier => new { supplier.EnglishName });
            var uniqueBrands = incomingData.Brands.DistinctBy(brand => new { brand.EnglishName });
            var uniqueUnits=incomingData.Units.DistinctBy(unit => new { unit.EnglishName });
            var uniqueCategories = incomingData.Categories.DistinctBy(category => new { category.EnglishName });
            var uniqueCountries=incomingData.Countries.DistinctBy(country => new { country.EnglishName });
            var uniqueProducts = incomingData.Products.DistinctBy(product => new { product.EnglishName });
            var uniqueBins= incomingData.Bins.DistinctBy(bin => new { bin.EnglishName });
            var uniqueManufacturers = incomingData.Manufacturers.DistinctBy(manufacturer => new { manufacturer.EnglishName });
            #endregion

            #region persisting non-child tables (child tables cannot exist without parent tables)

            using (var _context0 = _contextFactory.CreateDbContext())
            using (var _context1 = _contextFactory.CreateDbContext())
            using (var _context2 = _contextFactory.CreateDbContext())
            using (var _context3 = _contextFactory.CreateDbContext())
            using (var _context4 = _contextFactory.CreateDbContext())
            using (var _context5 = _contextFactory.CreateDbContext())
            using (var _context6 = _contextFactory.CreateDbContext())
            using (var _context7 = _contextFactory.CreateDbContext())
            {

                var asyncAddingOperations = new Func<Task>[]
                {
                    ()=>_context0.AddRangeAsync(uniqueSuppliers ),
                    ()=>_context1.AddRangeAsync(uniqueBrands),
                    ()=>_context2.AddRangeAsync(uniqueUnits),
                    ()=>_context3.AddRangeAsync(uniqueCategories),
                    ()=>_context4.AddRangeAsync(uniqueCountries),
                    ()=> _context5.AddRangeAsync(uniqueProducts),
                    ()=>_context6.AddRangeAsync(uniqueBins),
                    ()=>_context7.AddRangeAsync(uniqueManufacturers)
                };
                
                Task[] addingTasks = new Task[asyncAddingOperations.Length];
                for (int i = 0; i < addingTasks.Length; i++)
                {
                    addingTasks[i] = asyncAddingOperations[i].Invoke();  
                }

                Task continuation=Task.WhenAll(addingTasks);
                continuation.Wait();

                

                var asyncSavingOperations = new Func<Task>[]
                {
                    ()=>_context0.SaveChangesAsync()  ,
                    ()=> _context1.SaveChangesAsync() ,
                    ()=> _context2.SaveChangesAsync() ,
                    ()=> _context3.SaveChangesAsync() ,
                    ()=> _context4.SaveChangesAsync() ,
                    ()=> _context5.SaveChangesAsync() ,
                    ()=> _context6.SaveChangesAsync() ,
                    ()=>  _context7.SaveChangesAsync()
                };
                Task[] savingTasks = new Task[8];
                for (int i = 0; i < savingTasks.Length; i++)
                {
                    savingTasks[i] = asyncSavingOperations[i].Invoke();
                }

                Task savingContinuation=Task.WhenAll(savingTasks);
                savingContinuation.Wait();
            }

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

           List<SubCategory> uniqueSubCategories=zip.Select(x => x.sc).DistinctBy(subCategory => new { subCategory.EnglishName}).ToList();
           
            #endregion

            #region Populating junction tables (a junction table contain two foreign keys)
            
            var castedBrands = incomingData.Brands.Cast<INonJunctionEntity>().ToList();
            var castedSuppliers = incomingData.Suppliers.Cast<INonJunctionEntity>().ToList();
            var castedProducts = incomingData.Products.Cast<INonJunctionEntity>().ToList();          



            var uniqueBrandSuppliers =
                ConstructJunctionTable<BrandSupplier>(
                    castedBrands,
                    castedSuppliers,
                    "BrandId","SupplierId"
                    );

            var uniqueProductCountries = ConstructJunctionTable<ProductCountry>(
                castedProducts,
                 incomingData.Countries.Cast<INonJunctionEntity>().ToList(),
                 "ProductId","CountryId"
                );
            ////////
            var uniqueProductManufacturers =
                ConstructJunctionTable<ProductManufacturer>(
                 castedProducts,
                 incomingData.Manufacturers.Cast<INonJunctionEntity>().ToList(),
                 "ProductId","ManufacturerId"
                );

            ////
            var uniqueProductBins = ConstructJunctionTable<ProductBin>(
                castedProducts,
                incomingData.Bins.Cast<INonJunctionEntity>().ToList(),
                "ProductId","BinId"
            );
            //////
            var uniqueProductSuppliers =
                ConstructJunctionTable<ProductSupplier>(
                castedProducts,
                castedSuppliers,
                "ProductId","SupplierId"
            );

            /////////
            var uniqueProductBrands =
                ConstructJunctionTable<ProductBrand>(
                    castedProducts,
                    castedBrands,
                    "ProductId","BrandId"
                    );
            ////
            var uniqueProductCategories = ConstructJunctionTable<ProductCategory>(
                castedProducts,
                incomingData.Categories.Cast<INonJunctionEntity>().ToList(),
                "ProductId","CategoryId"
                );
            //////
            var uniqueProductUnits = ConstructJunctionTable<ProductUnit>(
                castedProducts,
                incomingData.Units.Cast<INonJunctionEntity>().ToList(),
                "ProductId","UnitId"
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
                //var addingDelegates = new Func<Task>[]
                //{
                //    ()=> _context0.AddRangeAsync(uniqueSubCategories),
                //    ()=> _context1.AddRangeAsync(uniqueBrandSuppliers),
                //    ()=> _context2.AddRangeAsync(uniqueProductCountries),
                //    ()=> _context3.AddRangeAsync(uniqueProductManufacturers),
                //    ()=> _context4.AddRangeAsync(uniqueProductBins),
                //    ()=> _context5.AddRangeAsync(uniqueProductSuppliers),
                //    ()=>_context6.AddRangeAsync(uniqueProductBrands) ,
                //    ()=>_context7.AddRangeAsync(uniqueProductCategories) ,
                //    ()=> _context8.AddRangeAsync(uniqueProductUnits)
                //};
                var _contexts = new ApplicationDbContext[]
                {
                    _context0,
                    _context1,
                    _context2 ,
                    _context3 ,
                    _context4 ,
                    _context5 ,
                    _context6 ,
                    _context7 ,
                    _context8 
                };
                
                var dataPieces = new IEnumerable<IEntity>[]
                {
                    uniqueSubCategories,
                    uniqueBrandSuppliers,
                    uniqueProductCountries,
                    uniqueProductManufacturers,
                    uniqueProductBins,
                    uniqueProductSuppliers,
                    uniqueProductBrands ,
                    uniqueProductCategories ,
                    uniqueProductUnits
                };
                Task[] addingTasks = new Task[_contexts.Length];
                //Task[] addingTasks = new Task[addingDelegates.Length];
                for (int i = 0; i < addingTasks.Length; i++)
                {
                    //addingTasks[i] = addingDelegates[i].Invoke();
                    addingTasks[i] = _contexts[i].AddRangeAsync(dataPieces[i]);
                }
                Task.WhenAll(addingTasks).Wait();



                Task[] savingTasks = new Task[_contexts.Length];
                for (int i = 0; i < savingTasks.Length; i++)
                {
                    savingTasks[i]=_contexts[i].SaveChangesAsync();
                }
                
                Task.WhenAll(savingTasks).Wait();
                
            }
            #endregion

            
            #region Local functions
            static void PopulateIdProperty<T>(List<T> objects, List<T> sourceOfTruth ) where T:IHasEnglishAndArabicName, IHasIdentification
            { 
                var lookups = sourceOfTruth.ToDictionary(t => t.EnglishName, t => t);
                foreach (T obj in objects)
                {
                    obj.Id = lookups[obj.EnglishName].Id;

                }
            }
            static HashSet<T> ConstructJunctionTable<T>(List<INonJunctionEntity> leftEntities,List<INonJunctionEntity> rightEntities,string name1,string name2) where T:new()
            {

                PropertyInfo property1 =typeof(T).GetProperty(name1);
                PropertyInfo property2 = typeof(T).GetProperty(name2);
                HashSet<T> result = new HashSet<T>();
                var zip=leftEntities.Zip(rightEntities, (le, re) => new { le, re });
                foreach (var x in zip )
                {
                    T t = new T(); // PROBLEM: Replace with createInstance?
                    property1.SetValue(t, x.le.Id);
                    property2.SetValue(t, x.re.Id);
                    result.Add(t); // O(n)  PROBELM: optimize this method
                }
                return result;
            }

            #endregion
        }
        #endregion

        #endregion

        #region facade method
        public async Task ReadWrite(object sheet)
        {


            
            IDataDto data = await Read((Worksheet)sheet);

            //using (MiniProfiler.Current.Step("Write()"))
            //{
                await Write(data);

            //}
        }

        //Old approach: Bad

        #endregion

    }
}