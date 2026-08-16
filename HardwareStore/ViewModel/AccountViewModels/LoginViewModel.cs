using System.ComponentModel.DataAnnotations;

namespace HardwareStore.ViewModel.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]     // required here means that field must not be null during the http request.
        
        // other solution : I can create custome attribute; [EmailOrPhoneNumber].
        [RegularExpression("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$|(^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$)", ErrorMessage = "Please enter a valid email address or phone number")]

        [Display(Name = "Email or phone number")]
        public string EmailOrPhoneNumber { get; set; } // I want it to be nullable because I want to pass empty model  from the Login Action method to the Login.cshtml view
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } // I want it to be nullable because I want to pass empty model from the Login Action method to the Login.cshtml view
        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } // default value is false. no need for it to be null.


    }
}
