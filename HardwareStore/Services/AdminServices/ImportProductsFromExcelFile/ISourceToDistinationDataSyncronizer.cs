namespace HardwareStore.Services.AdminServices.ImportProductsFromExcelFile
{

    public interface ISourceToDistinationDataSyncronizer<DataSource>
    {

        public Task SyncronizeAsync(DataSource dataSource,CancellationToken cancellationToken);

    }


}
