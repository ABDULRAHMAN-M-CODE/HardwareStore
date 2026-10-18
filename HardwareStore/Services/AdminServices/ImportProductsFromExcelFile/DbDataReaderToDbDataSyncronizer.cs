using HardwareStore.Services.ActorIdGetterServices;
using Microsoft.Data.SqlClient;
using Sylvan.Data.Excel;
using System.Data.Common;
namespace HardwareStore.Services.AdminServices.ImportProductsFromExcelFile
{
    public class DbDataReaderGetter
    {
        public async Task<DbDataReader> GetDbDataReader(Stream stream,CancellationToken cancellationToken)
        {
           return await ExcelDataReader.CreateAsync(stream,ExcelWorkbookType.ExcelXml,cancel:cancellationToken);
        }
    }

    public class BulkInserter
    {
        private readonly IActorIdGetter _actorIdGetter;
        public BulkInserter(IActorIdGetter actorIdGetter)
        {
            _actorIdGetter = actorIdGetter;
        }
        public async Task BulkInsertProductsToStagingTable(DbDataReader dbDataReader, CancellationToken cancellationToken)
        {
            using (SqlConnection destinationConnection =
                       new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=HardwareDB;Trusted_Connection=True;"))
            {
                await destinationConnection.OpenAsync(cancellationToken);

                // Set up the bulk copy object.
                // Note that the column positions in the source
                // data reader match the column positions in
                // the destination table so there is no need to
                // map columns.
                using (SqlBulkCopy bulkCopy =
                           new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.StagingProducts";

                    try
                    {
                        // Write from the source to the destination.
                        await bulkCopy.WriteToServerAsync(dbDataReader,cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        // Close the SqlDataReader. The SqlBulkCopy
                        // object is automatically closed at the end
                        // of the using block.
                        await dbDataReader.CloseAsync();
                    }
                }

                //####The following does not work yet.#####
                //// Perform a final count on the destination
                //// table to see how many rows were added.
                //long countEnd = System.Convert.ToInt32(
                //    commandRowCount.ExecuteScalar());
                //Console.WriteLine("Ending row count = {0}", countEnd);
                //Console.WriteLine("{0} rows were added.", countEnd - countStart);
                //Console.WriteLine("Press Enter to finish.");
                //Console.ReadLine();
            }
        }

    }


    public class WorkSheetToDbSyncronizerV2 : ISourceToDistinationDataSyncronizer<Stream>
    {

        private readonly DbDataReaderGetter _dbDataReaderGetter;
        private readonly BulkInserter _bulkInserter;

        public WorkSheetToDbSyncronizerV2(DbDataReaderGetter dbDataReaderGetter, BulkInserter bulkInserter)
        {
            _dbDataReaderGetter = dbDataReaderGetter;
            _bulkInserter = bulkInserter;
        }

        public async Task SyncronizeAsync(Stream stream, CancellationToken cancellationToken)
        {
            DbDataReader reader =await _dbDataReaderGetter.GetDbDataReader(stream, cancellationToken);
            await _bulkInserter.BulkInsertProductsToStagingTable(reader,cancellationToken);
            // Now call a stored procedure  that internelly selects data from the staging table
            //to populate all the parents then child tables

        }




    }


}// end of namespace
