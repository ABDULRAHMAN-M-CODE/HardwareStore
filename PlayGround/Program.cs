using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace PlayGround
{

    public class SignupViewModel
    {



        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$|(^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$)", ErrorMessage = "Please enter a valid email address or phone number")]
        [Display(Name = "Email or phone number")]
        public string? EmailOrPhoneNumber { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }



    }
    public record RegistrationResult(bool IsSucceeded, List<string> Errors);

    public class ApplicationUser : IdentityUser
    {

    }
    public interface IAccount
    {
        public Task<RegistrationResult> RegisterUser(SignupViewModel SignupInput);
    }
    public class UserAccount : IAccount
    {
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserAccount(IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager)

        {
            _userManager = userManager;
            _userStore = userStore;
        }


        // HOW TO TEST THE `RegisterUser` METHOD ALONE WITHOUT RUNNING THE WHOLE  APPLICATION ?
        // I WANT TO TRIGGER ALL THE PATHES OF THE `RegisterUser` METHOD TO VERIFY THAT EACH PATH IS TRIGGERED WHEN IT IS NEEDED.
        public async Task<RegistrationResult> RegisterUser(SignupViewModel SignupInput)
        {
            // PATH 1
            var validationContext = new ValidationContext(SignupInput);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(SignupInput, validationContext, results, validateAllProperties: true);
            if (!valid)
            {
                // PATH 2
                return new RegistrationResult(false, ["The sent data is not valid"]);
            }


            ApplicationUser user = new ApplicationUser();
            await _userStore.SetUserNameAsync(user, SignupInput.Name, CancellationToken.None);

            if (!(_userManager.SupportsUserEmail))
            {
                //PATH 3
                throw new NotSupportedException("user manager does not support user email");
            }

            var _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            await _emailStore.SetEmailAsync(user, SignupInput.EmailOrPhoneNumber, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, SignupInput.Password);
            List<IdentityError> identityErrors = result.Errors.ToList();

            if (identityErrors.Count != 0)
            {
                //PATH 4
                List<string> errorsStrings = new List<string>();

                foreach (IdentityError identityError in identityErrors)
                {
                    errorsStrings.Add(identityError.Description);
                }

                return new RegistrationResult(false, errorsStrings);



            }

            await _userManager.AddToRoleAsync(user, "Normal-User");
            return new RegistrationResult(true, []);

        }

    }

}