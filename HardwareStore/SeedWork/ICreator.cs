namespace HardwareStore.SeedWork
{
    public interface ICreator<TSelf> where TSelf : ICreator<TSelf>
    {
        static abstract TSelf Create();
    }

}
