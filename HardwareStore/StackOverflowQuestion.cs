
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;

//namespace RedditQuestion
//{
//    /// <summary>
//    /// Used to talk to the database using Entity-Framework Core
//    /// </summary>
//    public class ApplicationDbContext:DbContext
//    {
//        // Domain models and the Fluent API configurations.
//    }


//    /// <summary>
//    /// Data snapshot that is used to overwrite the content of the database
//    /// </summary>
//    public interface ISomeData
//    {

//    }

//    public class DatbaseOverWriter
//    {


//        private readonly ApplicationDbContext _context;
//        public DatbaseOverWriter(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task OverWrite(ISomeData data)
//        {
//            //https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections
//            IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
//            await strategy.ExecuteAsync(async () =>
//            {
//                await using var transaction = await _context.Database.BeginTransactionAsync();

//                    //code to delete the database, saving data to the parents tables, and finally save child tables

//                await transaction.CommitAsync();
//            });

//        }




//    }

//}
