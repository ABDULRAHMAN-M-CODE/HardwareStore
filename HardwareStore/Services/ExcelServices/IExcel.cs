using Microsoft.Extensions.FileProviders;
using Spire.Xls;

namespace HardwareStore.Services.ExcelServices
{
    public interface IExcel
    {
        public Worksheet GetSheet(int sheetNumber);
    }
}
