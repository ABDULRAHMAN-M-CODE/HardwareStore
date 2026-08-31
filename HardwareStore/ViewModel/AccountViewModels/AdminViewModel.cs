
namespace HardwareStore.ViewModel.AccountViewModels
{
    public class AdminViewModel 
    {
        // fields that are sent to the view 
        public bool DeletionSucceed;
        public int TotalPages;
        public List<User> SpecificPageUsers { get; set; }

        //  fields that are  sent to  the action method 
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? KeyWord { get; set; } // populated by Client
    }
}
