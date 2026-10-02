using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using HardwareStore.DTOs;
using HardwareStore.Services.AdminServices;
using HardwareStore.Services.ExcelServices;
using Spire.Xls;

namespace MyBenchmarks
{
    [MinColumn, MaxColumn]
    public class ReadVersions
    {

        private readonly DiskExcel _excel = new(@"D:\Training\Excel2.xlsx");
        private readonly ReadExcelWriteDatabase _readerWriter;

        public ReadVersions()
        {

            _readerWriter = new ReadExcelWriteDatabase(_excel);

        }

        public IEnumerable<object> GetSheet()
        {
            yield return _excel.GetSheet(1);
        }

        [Benchmark(Baseline =true)]
        [ArgumentsSource(nameof(GetSheet))]
        public async Task<IDataDto> Read_CompiledExpressionApproach(Worksheet sheet)
        {
            return await _readerWriter.Read(sheet);
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, new DebugInProcessConfig());
            // Source - https://stackoverflow.com/a/79679146
            // Posted by petesramek, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-10-01, License - CC BY-SA 4.0



            foreach (var item in summary)
            {
                for (int i = 0; i < item.Reports.Length; i++)
                {
                    if (!long.TryParse(item.Table.Columns.Where(c => c.Header == "WarmupCount").FirstOrDefault()?.Content[i], out var warmupCount))
                    {
                        warmupCount = item.Reports[i].AllMeasurements
                            .Where(m => m.IterationMode == IterationMode.Workload && m.IterationStage == IterationStage.Warmup)
                            .Count();
                    }

                    if (!long.TryParse(item.Table.Columns.Where(c => c.Header == "IterationCount").FirstOrDefault()?.Content[i], out var iterationCount))
                    {
                        iterationCount = item.Reports[i].AllMeasurements
                            .Where(m => m.IterationMode == IterationMode.Workload && m.IterationStage == IterationStage.Actual)
                            .Count();
                    }

                    if (!long.TryParse(item.Table.Columns.Where(c => c.Header == "InvocationCount").FirstOrDefault()?.Content[i], out var invocationCount))
                    {
                        invocationCount = item.Reports[i].AllMeasurements
                            .Where(m => m.IterationMode == IterationMode.Workload && m.IterationStage == IterationStage.Pilot)
                            .LastOrDefault().Operations;
                    }

                    Console.WriteLine($"{item.Title} - {item.Reports[i].BenchmarkCase.Descriptor.WorkloadMethod.Name}");

                    Console.WriteLine($"WarmupCount: {warmupCount}");
                    Console.WriteLine($"IterationCount: {iterationCount}");
                    Console.WriteLine($"InvocationCount: {invocationCount}");
                }
            }

        }
    }

}
