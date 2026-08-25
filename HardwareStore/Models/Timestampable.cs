namespace HardwareStore.Models
{
    public class Timestampable
    {
        public DateTime UpdatedAt { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public DateTime CreatedAt { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public DateTime DeleatedAt { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
    }
}
