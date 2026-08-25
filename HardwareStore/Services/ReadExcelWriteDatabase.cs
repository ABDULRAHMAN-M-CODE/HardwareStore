namespace HardwareStore.Services
{
    public class ReadExcelWriteDatabase:IOmniReader,IOmniWriter
    {

        public object ReadData()
        {
            return "data is read from excel file";
        }

        public void WriteData()
        {
            Console.WriteLine("Data is written to database");
        }
    }
}
