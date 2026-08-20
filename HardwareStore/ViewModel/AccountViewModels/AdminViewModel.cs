using HardwareStore.Models;
namespace HardwareStore.ViewModel.AccountViewModels
{
    public class AdminViewModel 
    {
        public  int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? KeyWord { get; set; } // populated by Client
        
        
        public  List<User> SpecificPageUsers { get; set; }// populated by backend
        public int TotalPages;
        public bool DeletionSucceed;



    }
}
