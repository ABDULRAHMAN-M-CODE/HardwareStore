using HardwareStore.Models;
namespace HardwareStore.ViewModel.AccountViewModels
{
    public class AdminViewModel 
    {
        public  int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1; // clicking <a> correctly guides the user to the correct page, changing the PageNumber correctly, also clicking the Previouse page correctly decreases the PageNumber by 1, but clicking Next does not work correctly,
        public string? KeyWord { get; set; } // populated by Client
        
        
        public  List<User> SpecificPageUsers { get; set; }// populated by backend
        public int TotalPages;
        public bool DeletionSucceed;



    }
}
