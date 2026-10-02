namespace HardwareStore.SeedWork
{
    public interface INonJunctionEntity : IEntity
    {
        public string? EnglishName { get; set; }
        public int Id { get; set; }
    }
}
