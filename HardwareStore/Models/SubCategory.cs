namespace HardwareStore.Models
{
    public class SubCategory
    {
        //notes for my self in the comments.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public string? ArabicName { get; set; }
        public DateTime UpdatedAt { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public DateTime CreatedAt { get; set; }//  if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public DateTime DeleatedAt { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

        
        // FK related
        public int CategoryId { get; set; } 
        public Category Category { get; set; } = null!;
    }
}
