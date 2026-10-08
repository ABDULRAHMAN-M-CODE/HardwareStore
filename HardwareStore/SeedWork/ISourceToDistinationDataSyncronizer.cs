namespace HardwareStore.SeedWork
{


    //I should use Interface, not abstract class
    // because I know the following in my mind:
    // There is some classes that can read and write data, 
    // 1-There will not be  SHARED CODE between classes.
    // 2-All classes should be able to read and write, but they do it in DIFFERENT WAYS
    using HardwareStore.DTOs;
    public interface IOmniReader
    {

        public  Task<IDataDto> Read(object dataSource);

    }
    /// <summary>
    ///  Abstracts the destination to which data is written, regardless of  whether the destination is a file, database, or any other kind of storage.
    /// </summary>
    public interface IOmniWriter
    {
        public Task Write(IDataDto dataDto); 
    }
    
    /// <summary>
    /// Abstracts the destination tables in the Sql Server; may write to all tables in the database or some of them;
    /// </summary>
    public interface ISqlServerWriter:IOmniWriter
    {

    }

  



    public interface ISourceToDistinationDataSyncronizer<T>
    {

        public  Task<T> ReadAllData();
        public Task OverWrite(T sourceData);

    }


}
