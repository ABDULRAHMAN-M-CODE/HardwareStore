namespace HardwareStore.ViewModel
{
    public class ProductsToView
    {
        public string? Image { get; set; } // path  for the image inside the wwwroot folder. there is other approach, I can use byte[] if I want to store the image in the database.
        public int Id { get; set; }
        public string? Name { get; set; }
        public string?Description { get; set; }
        public decimal? Price { get; set; }
    }
}
