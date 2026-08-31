namespace HardwareStore.Models
{

    //abstraact class is better than interface  in this case, code sharing. 
    public abstract class Timestampable
    {
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        //public  required string CreatedBy { get; set; }//  required : violates new() constraint.
        //public  required string UpdatedBy { get; set; } // required : violates new() constraint.
        //public  string? DeletedBy { get; set; }//  required : violates new() constraint.
        
        
        public   string? CreatedBy { get; set; }
        public   string? UpdatedBy { get; set; } 
        public  string? DeletedBy { get; set; }

    }
}
