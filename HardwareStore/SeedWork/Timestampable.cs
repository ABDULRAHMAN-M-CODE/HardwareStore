namespace HardwareStore.SeedWork
{

    //abstraact class is better than interface  in this case, code sharing. 
    
    public abstract class HasCreatorAndUpdator
    {
        public string? CreatorId { get; set; }
        public string? UpdaterId { get; set; }

    }
    public abstract class Timestampable:HasCreatorAndUpdator
    {
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        //public  required string CreatedBy { get; set; }//  required : violates new() constraint.
        //public  required string UpdatedBy { get; set; } // required : violates new() constraint.
        //public  string? DeletedBy { get; set; }//  required : violates new() constraint.
        
        public  string?  DeleterId { get; set; }

    }
}
