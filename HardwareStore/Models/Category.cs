using HardwareStore.Services;

namespace HardwareStore.Models
{
    public class Category: Timestampable,IHasEnglishAndArabicName,IHasIdentification
    {
        //notes for my self in the comments.
        public string? EnglishName { get; set; } // if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.
        public int Id { get; set; }// if I do not use  property , this column will not be created in the database unless I explicitly specifiy it using Fluent API configuration.

        public string? ArabicName { get; set; }


        // that means category has many products, so the product is the 'many side' in the relation
        public IEnumerable<Product> Products{ get; } = new List<Product>(); // Collection navigation 
        // that means category has many subcategories, WHICH MEANS the subcategories is the 'many side' in the relation
        public IEnumerable<SubCategory> SubCategories { get; }=new List<SubCategory>();
        public ApplicationUser User { get; set; } = null!;
    }
}
