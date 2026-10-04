using HardwareStore.SeedWork;
using Sylvan.Data.Excel;
using System.Data;
using System.Reflection;


namespace PlayGround
{

public class MyProduct
{
        public string? EnglishName { get; set; }
        public string? Category { get; set; }
}


public class Experiment

{

    
        public List<T> BindExcelDataToObjects<T>(List<string> propertiesNames, List<string> ExcelColumnsNames) where T :ICreator<T>
    
        {



            // move the following two lines outside this method
            string path1 = @"d:\Training\NETLEARNING\EF_learning\HardwareStore\Available_Products\Excel2.xlsx";
            
            using ExcelDataReader edr = ExcelDataReader.Create(path1);
            //public abstract class ExcelDataReader : DbDataReader, which means that I can pass it to the SqlBulkCopy Class
            // but that means the following logic should be removed from the application and moved to the server using sql

            // Can the following code be performed on the server side using SQL?
            PropertyInfo[] propertiesInfos= new PropertyInfo[propertiesNames.Count];
            for (int i= 0;i<propertiesNames.Count ;i++)
            {
                propertiesInfos[i] = typeof(T).GetProperty(propertiesNames[i]);
            }            
            List<T> result = new List<T>();
            while (edr.Read())
              
            {
                T obj = T.Create();

               foreach(var propertyInfo in propertiesInfos)
                {
                    
                    propertyInfo.SetValue(obj, edr.GetFieldValue<string>(propertyInfo.Name));
                }
                
            }

            return  result ;


        }

}
    class Program
    {
        static void Main()
        {
 
            List<MyProduct> myProducts= new Experiment().BindExcelDataToObjects();
            foreach (var p in myProducts)
            {
                Console.WriteLine(p.EnglishName+" "+p.Category);
            }

        }
    }

}


