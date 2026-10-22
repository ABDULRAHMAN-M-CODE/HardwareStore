using HardwareStore.Models;
using HardwareStore.Services.ActorIdGetterServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

   
    /// <summary>
    /// Transfers data from excel file to a staging table
    /// </summary>
    public class BulkInserter
    {
        
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
                        "dbo.BulkInsertedProducts";

                    try
                    {
                        
                        await bulkCopy.WriteToServerAsync(dbDataReader,cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("########################################################");
                        Console.WriteLine("###############"+ex.Message+"###########################");
                        Console.WriteLine("############### RETHROWING THE EXCEPTION ###############");
                        Console.WriteLine("############### EXCEPTION Happened inside BulkInsert" +
                            "ProductsToStagingTable" +
                            " method ##############################################################");

                       
                        
                        throw ;
                    }
                    finally
                    {
                        // Close the SqlDataReader. The SqlBulkCopy
                        // object is automatically closed at the end
                        // of the using block.
                        await dbDataReader.CloseAsync();
                    }
                }
            }
        }

    }

    /// <summary>
    /// Ensures that the data in the database matches the data inside an Excel worksheet.
    /// </summary>
    public class WorkSheetToDbSyncronizerV2 : ISourceToDistinationDataSyncronizer<Stream>
    {

        private readonly DbDataReaderGetter _dbDataReaderGetter;
        private readonly BulkInserter _bulkInserter;
        private readonly ApplicationDbContext _context;
        private readonly IActorIdGetter _actorIdGetter;


        public WorkSheetToDbSyncronizerV2(DbDataReaderGetter dbDataReaderGetter, BulkInserter bulkInserter, IActorIdGetter actorIdGetter, ApplicationDbContext context)
        {
            _dbDataReaderGetter = dbDataReaderGetter;
            _bulkInserter = bulkInserter;
            _actorIdGetter = actorIdGetter;
            _context = context;

        }

        public async Task SyncronizeAsync(Stream stream, CancellationToken cancellationToken=default)
        {


  

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);
            try
            {
                DbDataReader reader = await _dbDataReaderGetter.GetDbDataReader(stream, cancellationToken);
                await _context.Database.ExecuteSqlInterpolatedAsync($" DELETE FROM  BulkInsertedProducts", cancellationToken);
                await _bulkInserter.BulkInsertProductsToStagingTable(reader, cancellationToken);
                await _context.Database.ExecuteSqlInterpolatedAsync($" EXECUTE OverWriteDatabaseData {_actorIdGetter.GetActorId()}", cancellationToken);

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("########################################################");
                Console.WriteLine("############### Operation cancelled ###########################");
                Console.WriteLine("###############" + ex.Message + "###########################");
                Console.WriteLine("############### EXCEPTION Happened inside SyncronizeAsync method, specifically in the second catch block ");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine("########################################################");
                Console.WriteLine("############### `SqlException` was thrown instead of `OperationCancelledException` ###########################");
                Console.WriteLine("###############" + ex.Message + "###########################");
                Console.WriteLine("############### EXCEPTION Happened inside SyncronizeAsync method, specifically in the second catch block. ");


            }


        }

    }


}// end of namespace
