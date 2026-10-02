using System;

public class MyTemplate
{
	public MyTemplate()
	{
        #region Local functions
        /// <summary>
        /// 
        /// <para>
        /// Normal relational sql table contains key attributes, like primary keys and foreign keys,
        /// and non-key attributes. this method  reads data from the specified excel sheet, the data
        /// will be used to populate the non-key attributes.
        /// </para>
        /// <para>
        /// Those entities returned by this method are  not unique
        /// it's up the caller to use LINQ expression to Distinct between the entities
        /// </para>
        /// Constraint : the order of the excel column names must match the order of the properties names
        /// </summary>
        /// <typeparam name="T">d</typeparam>
        /// <param name="relevantEntityPropertiesNames"></param>
        /// <param name="relevantExcelColumnsNames"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        List<T> ReadSpecificColumns<T>(List<string> entityPropertiesNames, List<string> ExcelColumnsNames) where T : IHasEnglishAndArabicName, new()
        {

            int numberOfProperties = entityPropertiesNames.Count;

            #region Safety check

            #endregion

            #region Get the model's properties given there names.

            #endregion

            #region Obtain columns indicies given there names.

            #endregion

            #region Create an in-memory representation of the Excel data.
     
            #endregion


            return result;

        }

        #endregion

    }
}
