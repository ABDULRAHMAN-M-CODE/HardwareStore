using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using HardwareStore.DTOs;
using HardwareStore.Services.AdminServices;
using HardwareStore.Services.ExcelServices;
using HardwareStoreNameSpace;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;

namespace MyBenchmarks
{

    public class MyDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");

            return new ApplicationDbContext(optionsBuilder);// continue from here
        }
    }
    [MinColumn, MaxColumn]
    public class ReadWriteVersions
    {

        private readonly DiskExcel _excel = new(@"D:\Training\Excel2.xlsx");
        private readonly ReadExcelWriteDatabase _readerWriter;

        public ReadWriteVersions()
        {

            _readerWriter = new ReadExcelWriteDatabase();

        }

        public IEnumerable<object> GetSheet()
        {
            yield return _excel.GetSheet(1);
        }

        [Benchmark(Baseline =true)]
        [ArgumentsSource(nameof(GetSheet))]
        public async Task ReadWriteWith_CSharp(Worksheet sheet)
        {
              await _readerWriter.ReadWrite(sheet);
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, new DebugInProcessConfig());    
            


        }
    }

}
