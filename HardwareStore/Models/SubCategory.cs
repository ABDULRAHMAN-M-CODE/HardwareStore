using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class SubCategory : Timestampable,IHasEnglishAndArabicName
    {
        //notes for my self in the comments.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? ArabicName { get; set; }
     

        
        // FK related
        public int CategoryId { get; set; } 
        public Category Category { get; set; } = null!;
    }
}
