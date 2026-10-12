namespace HardwareStore.SeedWork
{

    public interface ISourceToDistinationDataSyncronizer<DataSource>
    {

        public Task Syncronize(DataSource dataSource,CancellationToken cancellationToken);

    }


}
