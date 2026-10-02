using HardwareStore.SeedWork;
using Spire.Xls;

namespace HardwareStore.Services.ExcelServices
{
    public class DiskExcel:IExcel
    {

        public readonly string _filePath;
        public DiskExcel(string filePath){
            _filePath = filePath;
        }


        #region Interface methods (signature should not be changed at all)

        public Worksheet GetSheet(int sheetNumber)
        {
            var w = new Workbook();
            using (FileStream fs = File.OpenRead(_filePath))
            {
                w.LoadFromStream(fs);
            }
                
            return w.Worksheets[sheetNumber];

        }
        #endregion

    }
}
