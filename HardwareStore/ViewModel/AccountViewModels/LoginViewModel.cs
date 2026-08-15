using System.ComponentModel.DataAnnotations;

namespace HardwareStore.ViewModel.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]                          // required here means that field must not be null during the http request
        [EmailAddress]
        public string Email { get; set; } // I want it to be nullable because I want to pass empty model  from the Login Action method to the Login.cshtml view
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // I want it to be nullable because I want to pass empty model from the Login Action method to the Login.cshtml view
        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } // default value is false. no need for it to be null.


    }
}
