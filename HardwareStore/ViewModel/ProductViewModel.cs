namespace HardwareStore.ViewModel
{
    public class ProductViewModel
    {
        public string? Image { get; set; } // path  for the image inside the wwwroot folder. there is other approach, I can use byte[] if I want to store the image in the database.
        public string? Name { get; set; }
        public string?Description { get; set; }
        public decimal? Price { get; set; } // decimal has better precision and smaller range  compared to floating point type.
    }
}
