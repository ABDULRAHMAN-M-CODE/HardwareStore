using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using HardwareStore.Models;
using HardwareStore.Services.ActorIdGetterServices;
using HardwareStore.Services.AdminServices.ImportProductsFromExcelFile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BenchmarkDotNet.Loggers;

namespace MyBenchMark
{
    [Config(typeof(Config))]
    public class Config : ManualConfig
    {
        public Config()
        {

            AddExporter(RPlotExporter.Default);

            AddLogger(ConsoleLogger.Default);
        }
    }
    public class ActorIdGetter:IActorIdGetter
    {
        public string GetActorId()
        {
            return "a1f28e89-2ba1-493e-b005-744decdd78b3";
        }
    }
    [MinColumn, MaxColumn]
    public class SyncronizationService
    {       
        private readonly DbDataReaderGetter _dbDataReaderGetter=new DbDataReaderGetter();
        private readonly BulkInserter _bulkInserter=new BulkInserter();
        private readonly IActorIdGetter _actorIdGetter=new ActorIdGetter();
        private readonly ApplicationDbContext _context;
        private readonly WorkSheetToDbSyncronizerV2 _syncronizerV2;


       
        public SyncronizationService()
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(
                    @"D:\Training\NETLEARNING\EF_learning\HardwareStore"))
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString =
                configuration.GetConnectionString("HardwareDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'HardwareDB' was not found.");

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

            _context = new ApplicationDbContext(contextOptions);


            _syncronizerV2=new WorkSheetToDbSyncronizerV2(_dbDataReaderGetter, _bulkInserter,_actorIdGetter,_context);
        }



        [Benchmark(Baseline =true)]
        public async Task SyncronizeV2()
        {
            await using var stream = File.OpenRead(
               @"D:\Training\NETLEARNING\EF_learning\HardwareStore\Available_Products\Excel2.xlsx");
            await _syncronizerV2.SyncronizeAsync(stream);
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            

            var summary = BenchmarkRunner.Run<SyncronizationService>(new Config());



        }
    }

}
