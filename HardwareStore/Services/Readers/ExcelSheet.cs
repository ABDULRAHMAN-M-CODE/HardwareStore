using HardwareStore.SeedWork;
using Spire.Xls;
using System.Reflection;

namespace HardwareStore.Services.Readers
{

   
    public class ExcelSheet //ExcelWorkSheet or ExcelWorkSheetReader is better?
    {

        //private readonly IWorksheet _workSheet; will not work
        private readonly Worksheet _workSheet; 
    
        public ExcelSheet(Worksheet workSheet)
        {
            _workSheet= workSheet;
        }

        public List<T> ReadSpecificColumns<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T : INonJunctionEntity, ICreator<T>
        {

            int numberOfProperties = entityPropertiesNames.Count;

            #region Safety check
            if (numberOfProperties != ExcelColumnsNames.Count)
            {
                throw new ArgumentException("The number of the excel columns must equal the number of properties names");
            }
            #endregion

            #region Get the model's properties given there names.
            PropertyInfo[] modelProperties = new PropertyInfo[numberOfProperties];
            for (int i = 0; i < numberOfProperties; i++)
            {
                modelProperties[i] = typeof(T).GetProperty(entityPropertiesNames[i]); // wow removing the generic type made me notice; I can remove refelectio now?
            }
            #endregion

            #region Obtain columns indicies given there names. (OPTIMIZE (OR USE OTHER LIBRARY))
            int[] columnsIndicies = new int[numberOfProperties];
            for (int i = 0; i < numberOfProperties; i++)
            {
                columnsIndicies[i] = _workSheet.FindString(ExcelColumnsNames[i], false, false).Column;

            }
            #endregion


            #region Create an in-memory representation of the Excel data.
            List<T> result = new List<T>();
            for (int row = 2; row <= (_workSheet.LastRow); row++)//Scan all the rows. Skip first row assuming it is a header.
            {

                T entity = T.Create();

                //Scan specific columns; populate all the relevant properties of a single Entity  
                for (int i = 0; i < numberOfProperties; i++)
                {
                    var excelCellValue = Convert.ChangeType(_workSheet.Range[row, columnsIndicies[i]].Value, modelProperties[i].PropertyType);
                    modelProperties[i].SetValue(entity, excelCellValue);
                }

                result.Add(entity);
            }
            #endregion


            return result;

        }

    }
}
