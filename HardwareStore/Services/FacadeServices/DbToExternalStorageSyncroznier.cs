using HardwareStore.DTOs;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStore.Services.DatabaseServices;
using HardwareStore.Services.Readers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace HardwareStore.Services.FacadeServices
{
    public class SheetToDbSyncronizer:ISourceToDistinationDataSyncronizer<NonJunctionModelsSnapShot>
    {
        private readonly ExcelSheet _excelSheet;
        
        public SheetToDbSyncronizer(ExcelSheet excelSheet)
        {
            _excelSheet=excelSheet;
        }

        public async Task< NonJunctionModelsSnapShot> ReadAllData()
        {

            #region reading __workSheet in parallel
            var items = new Func<IEnumerable<INonJunctionEntity>>[]
            {
                ()=>_excelSheet.ReadSpecificColumns<Category>(["EnglishName"], ["Category"]),////Result[0]

                ()=>_excelSheet.ReadSpecificColumns<Country>(["EnglishName"], ["Country"]),// Resul[1]

                ()=>_excelSheet.ReadSpecificColumns<Unit>( ["EnglishName"], ["Unit"]),//Resul[2]
       
                ()=>_excelSheet.ReadSpecificColumns<Brand>( ["EnglishName"], ["Brand"]), //Resul[3]
       
                ()=>_excelSheet.ReadSpecificColumns<Supplier>( ["EnglishName"], ["Supplier"]),//Result[4]

                //Result[5]
                ()=>_excelSheet.ReadSpecificColumns<Product>([ "SKU", "Barcode", "EnglishName", "ArabicName", "Description", "Status", "VAT", "Price", "MinStock", "ReorderQTY" ],[ "SKU", "Barcode", "English Name", "Arabic Name", "Description", "Status", "VAT", "Price", "Min Stock", "Reorder Qty" ]),

                ()=>_excelSheet.ReadSpecificColumns<Bin>( ["EnglishName"], ["Bin"]),//Result[6]

                 ()=>_excelSheet.ReadSpecificColumns<Manufacturer>( ["EnglishName"], ["Manufacturer"]),//Result[7]
         
                 ()=>_excelSheet.ReadSpecificColumns<SubCategory>(["EnglishName"], ["Subcategory"])//Result[8]


            };
            var tasks = new List<Task<List<INonJunctionEntity>>>();
            foreach (var item in items)
            {
                tasks.Add(Task.Run(() => item.Invoke().ToList()));
            }

            var continuation = Task.WhenAll(tasks);

            await continuation;
            #endregion

            #region return reults
            //The following does not work; I'm not asking why.
            //I either should find a way to wrap NonJunctionModelsSnapShot inside a task, or change the return type to Task <List<List<INonJunctionEntity>>>
            
            var result = new NonJunctionModelsSnapShot{
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
            return result;
            //return result;

            #endregion



        }

    }
}
