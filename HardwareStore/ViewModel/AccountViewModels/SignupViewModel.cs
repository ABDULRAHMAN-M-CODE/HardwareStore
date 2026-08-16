using System.ComponentModel.DataAnnotations;

namespace HardwareStore.ViewModel.AccountViewModels
{
    public class SignupViewModel
    {


        [Required]

        // other solution : I can create custome attribute; [EmailOrPhoneNumber].
        [RegularExpression("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$|(^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$)", ErrorMessage = "Please enter a valid email address or phone number")]
        [Display(Name = "Email or phone number")]
        public  string? EmailOrPhoneNumber { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public    string? Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public   string? ConfirmPassword { get; set; }
       
       
    }
}
