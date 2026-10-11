using HardwareStore.DTOs;

namespace HardwareStore.SeedWork
{
    // all classes that inherit this class can overWrite `some` data storage.
    // What is actually abstracted is the data storage to be overwritten.
    public interface IOverWriter
    {
        public Task overWrite(IData dataDto);
    }
}
