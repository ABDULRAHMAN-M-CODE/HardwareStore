namespace HardwareStore.ViewModel
{
    public class ProductsPageViewModel
    {
        public List<ProductsToView> SpecificPageProducts { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumberOfProducts { get; set; }
    }
}
