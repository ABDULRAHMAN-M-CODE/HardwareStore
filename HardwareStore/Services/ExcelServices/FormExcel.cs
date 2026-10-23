using Spire.Xls;

namespace HardwareStore.Services.ExcelServices
{
    public class ExcelStream : IExcel
    {
        private readonly Stream _stream;
        public ExcelStream(Stream stream)
        {
            _stream = stream;
        }
        public Worksheet GetSheet(int sheetNumber)
        {

            var w = new Workbook();
            w.LoadFromStream(_stream);
            return w.Worksheets[sheetNumber];
        }
    }
}
