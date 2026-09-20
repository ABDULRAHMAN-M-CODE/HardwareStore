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

        public  IDataDto Read();

    }
    public interface IOmniWriter
    {
        public void  Write(IDataDto dataDto) { }  
    }
    public interface IOmniReaderWriter
    {

        public  Task ReadWrite();

    }


}
