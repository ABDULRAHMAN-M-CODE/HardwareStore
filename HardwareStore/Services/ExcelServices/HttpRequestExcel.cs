using HardwareStore.SeedWork;
using Spire.Xls;
using Spire.Xls.Core;

namespace HardwareStore.Services.ExcelServices
{
    
    public class HttpContextExcel:IExcel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextExcel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Worksheet GetSheet(int sheetNumber)
        {

            var w = new Workbook();
            w.LoadFromStream(_httpContextAccessor.HttpContext.Request.Form.Files["file"].OpenReadStream());
            return w.Worksheets[sheetNumber];

        }
    }
}
