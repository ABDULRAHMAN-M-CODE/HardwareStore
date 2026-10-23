using CommandLine;
using HardwareStore.DTOs;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStore.Services.ActorIdGetterServices;
using HardwareStore.Services.AdminServices.ImportProductsFromExcelFile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Spire.Xls;
using System.Diagnostics;
using System.Reflection;


namespace HardwareStore.Services
{
    public class AllBusinessProductsReader
    {
        public async Task<NonJunctionModelsSnapShot> ReadAllData(Worksheet worksheet)
        {


            var items = new Func<IEnumerable<INonJunctionEntity>>[]
            {
                ()=>ReadSpecificColumns<Category>(["EnglishName"], ["Category"]),////Result[0]

                ()=>ReadSpecificColumns<Country>(["EnglishName"], ["Country"]),// Resul[1]

                ()=>ReadSpecificColumns<Unit>( ["EnglishName"], ["Unit"]),//Resul[2]
       
                ()=>ReadSpecificColumns<Brand>( ["EnglishName"], ["Brand"]), //Resul[3]
       
                ()=>ReadSpecificColumns<Supplier>( ["EnglishName"], ["Supplier"]),//Result[4]

                //Result[5]
                ()=>ReadSpecificColumns<Product>(
                 [ "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" ],
                [ "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" ]),

                ()=>ReadSpecificColumns<Bin>( ["EnglishName"], ["Bin"]),//Result[6]

                 ()=>ReadSpecificColumns<Manufacturer>( ["EnglishName"], ["Manufacturer"]),//Result[7]
         
                 ()=>ReadSpecificColumns<SubCategory>(["EnglishName"], ["Subcategory"])//Result[8]


            };


            var tasks = new List<Task<List<INonJunctionEntity>>>();

            foreach (var item in items)
            {
                tasks.Add(Task.Run(() => item.Invoke().ToList()));
            }


            var result = await Task.WhenAll(tasks);

            return new NonJunctionModelsSnapShot
            {
                Categories = result[0].Cast<Category>().ToList(),
                Countries = result[1].Cast<Country>().ToList(),
                Units = result[2].Cast<Unit>().ToList(),
                Brands = result[3].Cast<Brand>().ToList(),
                Suppliers = result[4].Cast<Supplier>().ToList(),
                Products = result[5].Cast<Product>().ToList(),
                Bins = result[6].Cast<Bin>().ToList(),
                Manufacturers = result[7].Cast<Manufacturer>().ToList(),
                SubCategories = result[8].Cast<SubCategory>().ToList(),

            };

            List<T> ReadSpecificColumns<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T : INonJunctionEntity, ICreator<T>
            {

                int numberOfProperties = entityPropertiesNames.Count;


                if (numberOfProperties != ExcelColumnsNames.Count)
                {
                    throw new ArgumentException("The number of the excel columns must equal the number of properties names");
                }



                PropertyInfo[] modelProperties = new PropertyInfo[numberOfProperties];
                for (int i = 0; i < numberOfProperties; i++)
                {
                    modelProperties[i] = typeof(T).GetProperty(entityPropertiesNames[i]); // wow removing the generic type made me notice; I can remove refelectio now?
                }



                int[] columnsIndicies = new int[numberOfProperties];
                for (int i = 0; i < numberOfProperties; i++)
                {
                    columnsIndicies[i] = worksheet.FindString(ExcelColumnsNames[i], false, false).Column;

                }



                List<T> result = new List<T>();
                for (int row = 2; row <= (worksheet.LastRow); row++)//Scan all the rows. Skip first row assuming it is a header.
                {

                    T entity = T.Create();

                    //Scan specific columns; populate all the relevant properties of a single Entity  
                    for (int i = 0; i < numberOfProperties; i++)
                    {
                        var excelCellValue = Convert.ChangeType(worksheet.Range[row, columnsIndicies[i]].Value, modelProperties[i].PropertyType);
                        modelProperties[i].SetValue(entity, excelCellValue);
                    }

                    result.Add(entity);
                }



                return result;

            }



        }
    }

    public class DbOverWriter
    {

        #region fields and constructor
        private readonly ApplicationDbContext _context;
        private readonly IActorIdGetter _actorIdGetter;
        public DbOverWriter(ApplicationDbContext context, IActorIdGetter actorIdGetter)
        {
            _context = context;
            _actorIdGetter = actorIdGetter;
        }

        #endregion

        #region public methods
        public async Task OverWrite(NonJunctionModelsSnapShot incomingData, CancellationToken cancellationToken)
        {

            #region All code

            #region deduplicate parents
            var uniqueSuppliers = incomingData.Suppliers.DistinctBy(supplier => new { supplier.EnglishName }).ToList();
            var uniqueBrands = incomingData.Brands.DistinctBy(brand => new { brand.EnglishName }).ToList();
            var uniqueUnits = incomingData.Units.DistinctBy(unit => new { unit.EnglishName }).ToList();
            var uniqueCategories = incomingData.Categories.DistinctBy(category => new { category.EnglishName }).ToList();
            var uniqueCountries = incomingData.Countries.DistinctBy(country => new { country.EnglishName }).ToList();
            var uniqueProducts = incomingData.Products.DistinctBy(product => new { product.EnglishName }).ToList();
            var uniqueBins = incomingData.Bins.DistinctBy(bin => new { bin.EnglishName }).ToList();
            var uniqueManufacturers = incomingData.Manufacturers.DistinctBy(manufacturer => new { manufacturer.EnglishName }).ToList();
            #endregion

            #region populateId Values
            PopulateIdentityColumn(uniqueSuppliers);
            PopulateIdentityColumn(uniqueBrands);
            PopulateIdentityColumn(uniqueUnits);
            PopulateIdentityColumn(uniqueCategories);
            PopulateIdentityColumn(uniqueCountries);
            PopulateIdentityColumn(uniqueProducts);
            PopulateIdentityColumn(uniqueBins);
            PopulateIdentityColumn(uniqueManufacturers);

            #endregion

            #region code
            #region  Lookup operations
            LookupIdValues<Category>(incomingData.Categories, uniqueCategories.ToList());
            LookupIdValues<Product>(incomingData.Products, uniqueProducts.ToList());
            LookupIdValues<Bin>(incomingData.Bins, uniqueBins.ToList());
            LookupIdValues<Brand>(incomingData.Brands, uniqueBrands.ToList());
            LookupIdValues<Supplier>(incomingData.Suppliers, uniqueSuppliers.ToList());
            LookupIdValues<Country>(incomingData.Countries, uniqueCountries.ToList());
            LookupIdValues<Manufacturer>(incomingData.Manufacturers, uniqueManufacturers.ToList());
            LookupIdValues<Unit>(incomingData.Units, uniqueUnits.ToList());
            #endregion


            #region Populating foregin key(s) for non-junction child(s)




            var zip = incomingData.SubCategories.Zip(incomingData.Categories, (sc, c) => new { sc, c.EnglishName });
            Dictionary<string, Category> categoriesLookup = uniqueCategories.ToDictionary(c => c.EnglishName, c => c);
            foreach (var a in zip)
            {
                a.sc.CategoryId = categoriesLookup[a.EnglishName].Id;

            }

            List<SubCategory> uniqueSubCategories = zip.Select(x => x.sc).DistinctBy(subCategory => new { subCategory.EnglishName }).ToList();
            PopulateIdentityColumn(uniqueSubCategories);
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

            #region Auditing parent tables records
            var actorId = _actorIdGetter.GetActorId();
            AuditParentRecords(uniqueSuppliers, actorId);
            AuditParentRecords(uniqueBrands, actorId);
            AuditParentRecords(uniqueUnits, actorId);
            AuditParentRecords(uniqueCategories, actorId);
            AuditParentRecords(uniqueCountries, actorId);
            AuditParentRecords(uniqueProducts, actorId);
            AuditParentRecords(uniqueBins, actorId);
            AuditParentRecords(uniqueManufacturers, actorId);
            AuditParentRecords(uniqueSubCategories, actorId);
            #endregion


            #region Adding parents and childs to  the context.
            await _context.AddRangeAsync(uniqueSuppliers, cancellationToken);
            await _context.AddRangeAsync(uniqueBrands, cancellationToken);
            await _context.AddRangeAsync(uniqueUnits, cancellationToken);
            await _context.AddRangeAsync(uniqueCategories, cancellationToken);
            await _context.AddRangeAsync(uniqueCountries, cancellationToken);
            await _context.AddRangeAsync(uniqueProducts, cancellationToken);
            await _context.AddRangeAsync(uniqueBins, cancellationToken);
            await _context.AddRangeAsync(uniqueManufacturers, cancellationToken);
            await _context.AddRangeAsync(uniqueSubCategories, cancellationToken);
            await _context.AddRangeAsync(uniqueBrandSuppliers, cancellationToken);
            await _context.AddRangeAsync(uniqueProductCountries, cancellationToken);
            await _context.AddRangeAsync(uniqueProductManufacturers, cancellationToken);
            await _context.AddRangeAsync(uniqueProductBins, cancellationToken);
            await _context.AddRangeAsync(uniqueProductSuppliers, cancellationToken);
            await _context.AddRangeAsync(uniqueProductBrands, cancellationToken);
            await _context.AddRangeAsync(uniqueProductCategories, cancellationToken);
            await _context.AddRangeAsync(uniqueProductUnits, cancellationToken);
            #endregion

            #endregion

            #region Transaction
            try
            {
                //https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections

                IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                    #region Delete content from database then save new content.

                    await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE  [dbo].[uspDeleteAllDataFromDatabase]", cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    #endregion

                    await transaction.CommitAsync(cancellationToken);
                });

            }
            catch (OperationCanceledException e)
            {
                // exception will not be catched if the cancellation was requested and
                // the transaction reached a point of no return.
                Console.WriteLine("###########################################################");
                Console.WriteLine("########## OPERATION CANCELLED SUCCESSFULLY ###############");
                Console.WriteLine("###########################################################");
                Debug.WriteLine("###########################################################");
                Debug.WriteLine("########## OPERATION CANCELLED SUCCESSFULLY ###############");
                Debug.WriteLine("###########################################################");
                Console.WriteLine("OperationCanceledException: " + e.Message);


            }





            #endregion


            #endregion
        }

        #endregion

        #region local functions
        static void PopulateIdentityColumn<T>(List<T> objects) where T : IHasIdentification
        {
            int number = 1;
            foreach (T obj in objects)
            {
                obj.Id = number;
                number++;
            }
        }

        static void LookupIdValues<T>(List<T> objects, List<T> sourceOfTruth) where T : IHasEnglishAndArabicName, IHasIdentification
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
        static void AuditParentRecords<T>(List<T> objects, string actorId) where T : AuditableEntity, INonJunctionEntity
        {
            foreach (T obj in objects)
            {
                obj.CreatorId = actorId;
                obj.UpdaterId = actorId;
                var now = DateTime.Now;
                obj.CreatedAt = now;
                obj.UpdatedAt = now;
            }
        }

        #endregion
    }

    public class WorkSheetToDbSyncronizer : ISourceToDistinationDataSyncronizer<Stream>
    {

        private readonly AllBusinessProductsReader _allDataReader;
        private readonly DbOverWriter _overWriter;

        public WorkSheetToDbSyncronizer(AllBusinessProductsReader reader, DbOverWriter overWriter)
        {
            _allDataReader = reader;
            _overWriter = overWriter;
        }

        public async Task SyncronizeAsync(Stream stream, CancellationToken cancellationToken)
        {
            var worksheet = GetWorkSheet(stream, 0);
            await ReadProductsAndOverRideDatabase(worksheet, cancellationToken);

        }
        static Worksheet GetWorkSheet(Stream stream, int sheetNumber)
        {

            var w = new Workbook();
            w.LoadFromStream(stream);
            return w.Worksheets[sheetNumber];

        }
        public async Task ReadProductsAndOverRideDatabase(Worksheet worksheet, CancellationToken cancellationToken)
        {
            NonJunctionModelsSnapShot allProducts = await _allDataReader.ReadAllData(worksheet);
            await _overWriter.OverWrite(allProducts, cancellationToken);
        }

    }

}
