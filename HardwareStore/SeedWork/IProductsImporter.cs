namespace HardwareStore.SeedWork
{
    public interface IProductsImporter<T>
    {
        public Task ImportAsync(T source,CancellationToken token);
    }
}
