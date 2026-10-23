namespace HardwareStore.SeedWork
{
    public abstract class AuditableEntity
    {
        public string? CreatorId { get; set; }
        public string? UpdaterId { get; set; }
        public string? DeleterId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        
    }
}
