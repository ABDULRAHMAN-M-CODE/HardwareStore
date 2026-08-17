using HardwareStore.Models;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;


namespace HardwareStore.Services
{
    public interface IAccount
    {
        //public async Task<bool> RegisterUser(SignupViewModel SignupInput); // I will  get error : async should be used with methods that have body
        //  async Task<bool> is wrong.
        Task<bool> RegisterUser(SignupViewModel SignupInput);
        Task<bool> LoginUser(LoginViewModel loginInput);
        IQueryable<ApplicationUser> GetUsers(AdminViewModel options);
        Task<bool> DeleteUser(string id);
    }

    public class UserAccount : IAccount //We can implement AdminAccount
    {


        // My code, which is considered high-level policy, will use this abstraction instead of using concrete implementation.
        private readonly IUserStore<ApplicationUser> _userStore;        //Explanation: this is built- in abstraction for managing users, it's exampleof the repository pattern just for managing user 

        private readonly UserManager<ApplicationUser> _userManager;   //  actually create the user in the database, given password and 
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserAccount(IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {

            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;

        }


        public async Task<bool> RegisterUser(SignupViewModel SignupInput)
        {



            //check whether the model is valid or not.
            var context = new ValidationContext(SignupInput);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(SignupInput, context, results, validateAllProperties: true);
            if (!valid)
            {
                return false;
            }

            // if model is valid


            ApplicationUser user = new ApplicationUser();
            await _userStore.SetUserNameAsync(user, SignupInput.EmailOrPhoneNumber, CancellationToken.None); //CancellationToken parameter is required



            //I need to use this same user store for email operations, so verify it supports email and give me the email-specific interface.
            // check exists to prevent my code from blindly casting a store that might not support email.
            if (!(_userManager.SupportsUserEmail))
            {
                throw new NotSupportedException("user manager does not support user email");
            }
            var _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            await _emailStore.SetEmailAsync(user, SignupInput.EmailOrPhoneNumber, CancellationToken.None);
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


        public async Task<bool> LoginUser(LoginViewModel loginInput)
        {

            //check whether the model is valid or not.
            var context = new ValidationContext(loginInput);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(loginInput, context, results, validateAllProperties: true);
            if (!valid)
            {
                return false;
            }

            // if model is valid
            // Note : there is no need to put the following line of code in the Services folder, it's just single line of code.
            // 'false' parameter here means : don't lockout. it's just a required parameter, I'm forced to specify it, otherwise I will get error, there is no deeper meaning behind why I specified this value
            // in Program.cs we need options.SignIn.RequireConfirmedAccount =false, otherwise, result will be "NotAllowed"
            var result = await _signInManager.PasswordSignInAsync(loginInput.EmailOrPhoneNumber, loginInput.Password, loginInput.RememberMe, false);




            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        ///  this method  returns either all the users in the database or some of the users based on filtering policy.
        /// </summary>
        public IQueryable<ApplicationUser> GetUsers(AdminViewModel options)
        {
            ArgumentNullException.ThrowIfNull(options);
            // page size can be constant, while the page number should be dynamic.
            if (options.PageSize < 0 || options.PageNumber <=0)
            {
                throw new InvalidOperationException("Page size and/or Page Number cannot be less than zero.");
            }

            //wantAllUsers will be true when options.keyWord is null or empty.
            var wantAllUsers = string.IsNullOrWhiteSpace(options.KeyWord);
            IQueryable<ApplicationUser> applicationUsers = _userManager.Users; // Deferred execution

            if (!wantAllUsers)
            {

                // options.keyWord can't be null or empty, because wantAllUsers is false 

                // users is still  IQueryable<ApplicationUser>, no SQL was executed  yet.
                applicationUsers = applicationUsers.Where(au => au.UserName.Contains(options.KeyWord!));
            }
            



            return applicationUsers; 

        }

        
        public async Task<bool> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                return false;

            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;

                
            }
        }
    }

}