namespace HardwareStore.SeedWork
{

    //abstraact class is better than interface  in this case, code sharing. 
    
    public interface IHasCreatorAndUpdator
    {
        public string? CreatorId { get; set; }
        public string? UpdaterId { get; set; }
        public string? DeleterId { get; set; }
    }
    //AuditableEntity
    public interface ITimestampable
    {
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

    }
    //abstract class cannot extend more than one abstract class; define the parents as interfaces instead.
    public abstract class AuditableEntity:ITimestampable,IHasCreatorAndUpdator
    {
        public string? CreatorId { get; set; }
        public string? UpdaterId { get; set; }
        public string? DeleterId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        
    }
}
