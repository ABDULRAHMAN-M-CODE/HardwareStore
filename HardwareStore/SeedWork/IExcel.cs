using Microsoft.Extensions.FileProviders;
using Spire.Xls;

namespace HardwareStore.SeedWork
{
    public interface IExcel
    {
        public Worksheet GetSheet(int sheetNumber);
    }
}
