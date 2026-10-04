using HardwareStore.SeedWork;
using Spire.Xls;

namespace HardwareStore.Services.ExcelServices
{
    public class FormExcel:IExcel
    {
        private readonly  IFormFile _formFile;
        public FormExcel(IFormFile formFile)
        {
            _formFile=formFile;
        }
        public Worksheet GetSheet(int sheetNumber)
        {

            using var stream = _formFile.OpenReadStream();
            var w = new Workbook();
            w.LoadFromStream(stream);
            return  w.Worksheets[sheetNumber];
        }
    }
}
