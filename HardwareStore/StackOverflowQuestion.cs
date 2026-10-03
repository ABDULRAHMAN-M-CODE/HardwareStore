

using Microsoft.EntityFrameworkCore;
namespace StackOverflowQuestion
{

    public interface IEntity
    {

    }
    public interface INonJunctionEntity : IEntity
    {

    }
    public interface IJunctionEntity : IEntity
    {

    }
    public class SubCategory : INonJunctionEntity
    {
        public string? EnglishName { get; set; }
    }

    public class BrandSupplier : IJunctionEntity
    {
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<BrandSupplier> BrandsSuppliers { get; set; }
        // alot of other DbSet<T>....

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
    // Other classes that inherit from IEntity... 
    public class DatabaseService
    {


        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public DatabaseService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }

        public async Task PersistDataInParallel_FirstVersion(IEnumerable<IEntity>[] dataPieces)
        {


            using (var _context0 = _contextFactory.CreateDbContext())
            using (var _context1 = _contextFactory.CreateDbContext())

            {
                var _contexts = new ApplicationDbContext[]
                {
                    _context0,
                    _context1
                };

                Task[] addingTasks = new Task[_contexts.Length];
                for (int i = 0; i < addingTasks.Length; i++)
                {
                    addingTasks[i] = _contexts[i].AddRangeAsync(dataPieces[i]);
                }
                Task.WhenAll(addingTasks).Wait();


            }

        }



    }

}// end of namespace
