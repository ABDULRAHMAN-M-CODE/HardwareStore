
namespace HardwareStore.Services
{


    //I should use Interface, not abstract class
    // because I know the following in my mind:
    // There is some classes that can read and write data, 
    // 1-There will not be  SHARED CODE between classes.
    // 2-All classes should be able to read and write, but they do it in DIFFERENT WAYS
    //Then
    public interface IOmniReader
    {

        public void  ReadAndWriteData(); // I want to be able to read data from anywhere, but I failed to do it.
        

    }
    public interface IOmniWriter
    {
        public void  WriteData<T>(List<T> entities) where T : class, IHasEnglishAndArabicName { }   // I want to be able to write data to any where
    }


}
