using HardwareStore.Models;
using Spire.Xls;

namespace HardwareStore.Services
{
    public class ReadExcelWriteDatabase : IOmniReader, IOmniWriter
    {
        //private readonly Worksheet _sheet;
        //public ReadExcelWriteDatabase(Worksheet sheet)
        //{
        //    _sheet = sheet;
        //}
        public object ReadData()
        {
            return "data is read from excel file";
        }

        public void WriteData()
        {
            Console.WriteLine("Data is written to database");
        }

        //public async Task<List<T>> ReadSpecificColumns<T>(List<string> relevantUnitPropertiesNames)
        //{


        //    List<T> result = new List<T>();
        //    List<object> englishNameDuplicates = new List<object>();
        //    // Logic : I want loop over all the rows
        //    for (int row = 2; row <= this._sheet.LastRow; row++)//assuming row 1 is header, want skip it.
        //    {

        //        T singlRow = new T(); // MAKE MAKE this type generic
        //        Type type = typeof(Unit); // MAKE type generic

        //        // Populate all the relevant properties of a single Entity
        //        for (int i = 0; i < relevantUnitPropertiesNames.Count; i++)// method parameter
        //        {


        //            string relevantPropertyName = relevantUnitPropertiesNames[i];//Update
        //            var property = type.GetProperty(relevantPropertyName);
        //            // Inserts a space before any uppercase letter that follows a lowercase letter

        //            //string relevantHeaderName = Regex.Replace(relevantPropertyName, "(?<=[a-z])(?=[A-Z])", " ");

        //            //int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column;
        //            int relevantColumnNumber = sheet.FindString("Unit", false, false).Column; // Make string as parameter



        //            var value = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, property!.PropertyType);


        //            property.SetValue(unit, value); // MAKE this generic



        //        } // end of inner for loop



        //        if (!unitsNamesDuplicates.Contains(unit.EnglishName!))
        //        {
        //            units.Add(unit);
        //            unitsNamesDuplicates.Add(unit.EnglishName!);
        //        }



        //    }// end of outermost for loop

        //}
    }
}
