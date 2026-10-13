using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace StackOverFlowQuestion
{
    /// <summary>
    /// Used to talk to the database using Entity-Framework Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Domain models and the Fluent API configurations.
    }

    /// <summary>
    /// Data snapshot that is used to overwrite the content of the database
    /// </summary>
    public interface ISomeData
    {

    }

    public class DatbaseOverWriter
    {


        private readonly ApplicationDbContext _context;
        public DatbaseOverWriter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> OverWrite(ISomeData data,CancellationToken cancellationToken)
        {
            #region Transaction
            try
            {
                //https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections
                
                //Add data to context.....

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

                return true;
            }
            catch (OperationCanceledException e)
            {
                // exception will not be catched if the cancellation was requested and
                // the transaction reached a point of no return.
                Console.WriteLine("###########################################################");
                Console.WriteLine("########## OperationCanceledException was caught ###############");
                Console.WriteLine("########## OPERATION CANCELLED SUCCESSFULLY ###############");
                Console.WriteLine("###########################################################");
                Debug.WriteLine("###########################################################");
                Debug.WriteLine("########## OPERATION CANCELLED SUCCESSFULLY ###############");
                Debug.WriteLine("###########################################################");
                Console.WriteLine("OperationCanceledException: " + e.Message);

                return false;
            }

            // Source - https://stackoverflow.com/a/80009656
            // Posted by JonasH, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-10-13, License - CC BY-SA 4.0
            catch (DbUpdateException due)
            {

                
                Console.WriteLine("###########################################################");
                Console.WriteLine("DbUpdateException was caught");
                Console.WriteLine(due.Message);
                Console.WriteLine(due.HResult);
                Console.WriteLine("###########################################################");

                return false;// indicates that the operation was cancelled, show a massege to the user telling him that.
            }


         



            #endregion

        }




    }

}