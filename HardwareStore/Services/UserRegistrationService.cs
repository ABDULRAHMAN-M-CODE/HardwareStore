using HardwareStore.Models;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace HardwareStore.Services
{
    public class AccountService // Generic  class name, even if added additional functionality to the class, the name will still be good 
    {


        // My code, which is considered high-level policy, will use this abstraction instead of using concrete implementation.
        private readonly IUserStore<ApplicationUser> _userStore;        //Explanation: this is built- in abstraction for managing users, it's exampleof the repository pattern just for managing user 
        
        private readonly UserManager<ApplicationUser> _userManager;   //  actually create the user in the database, given password and 
        private SignupViewModel SignupInput { get; set; }
        
        public AccountService(
             
             IUserStore<ApplicationUser> userStore,
             UserManager<ApplicationUser> userManager,
             SignupViewModel signupInput
             

            )
        {


            
            _userManager = userManager;
            _userStore = userStore;
            
            SignupInput = signupInput;
            
        }


        public async Task<bool> RegisterUser() {
            
                //check whether the model is valid or not.
                var context = new ValidationContext(this.SignupInput);
                var results = new List<ValidationResult>();
                bool valid = Validator.TryValidateObject(SignupInput, context, results, validateAllProperties: true);
                if (!valid)
                {
                    return false;
                }
                
                // if model is valid

                ApplicationUser user = new ApplicationUser();
                await _userStore.SetUserNameAsync(user, SignupInput.Email, CancellationToken.None); //CancellationToken parameter is required



            //I need to use this same user store for email operations, so verify it supports email and give me the email-specific interface.
            // check exists to prevent my code from blindly casting a store that might not support email.
            if (!(_userManager.SupportsUserEmail))
                {
                    throw new NotSupportedException("user manager does not support user email");
                }
                var _emailStore=(IUserEmailStore<ApplicationUser>)_userStore;
                await _emailStore.SetEmailAsync(user, SignupInput.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, SignupInput.Password); // only create the user after specifying his name and password.

                if (result.Succeeded)
                {
                    
                    return true;
                }

                else
                {
                    return false;
                }

                
            }
              



        }
    }

