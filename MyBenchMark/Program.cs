using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using HardwareStore.Services.ExcelServices;
using HardwareStore.Models;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;

namespace MyBenchmarks
{


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
            // Don't use the Switcher, refer to the documentation and use other varient..
            var summary = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, new DebugInProcessConfig());    
            


        }
    }

}
