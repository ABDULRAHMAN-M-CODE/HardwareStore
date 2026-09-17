using AutoMapper;
using HardwareStore.Models;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;


namespace HardwareStore.Services
{
    public interface IAccount
    {
        //public async Task<bool> RegisterUser(SignupViewModel SignupInput); // I will  get error : async should be used with methods that have body
        //  async Task<bool> is wrong.
        Task<bool> RegisterUser(SignupViewModel SignupInput);
        Task<(bool,bool)> LoginUser(LoginViewModel loginInput); // THe second bool value indicates whether the logged-in user is admin or not.
        Task<IQueryable<ApplicationUser>> GetUsers(AdminViewModel options);
        Task<bool> DeleteUser(string id);
        Task<bool> Logout();
        //Task<ApplicationUser?> FindByIdAsync(string Id); : Old
        Task<User?> GetUserForEdit(string id); // new.
        Task<bool> EditUser(User user);



    }

    public class UserAccount : IAccount 
    {


        // My code, which is considered high-level policy, will use this abstraction instead of using concrete implementation.
        private readonly IUserStore<ApplicationUser> _userStore;        //Explanation: this is built- in abstraction for managing users, it's exampleof the repository pattern just for managing user 
        private readonly UserManager<ApplicationUser> _userManager;   //  actually create the user in the database, given password and 
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserAccount(IUserStore<ApplicationUser> userStore, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context,
            IMapper mapper)
        
        {

            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _roleManager= roleManager;
            _context = context;
            _mapper = mapper;

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
            await _userStore.SetUserNameAsync(user, SignupInput.Name, CancellationToken.None); //CancellationToken parameter is just required, there is no deeper meaning behind why I used it, it's just a required parameter, so I choosed to set it as null, as I only care about setting the name.



            //I need to use this same user store for email operations, so verify it supports email and give me the email-specific interface.
            // check exists to prevent my code from blindly casting a store that might not support email.
            if (!(_userManager.SupportsUserEmail))
            {
                throw new NotSupportedException("user manager does not support user email");
            }
            var _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            await _emailStore.SetEmailAsync(user, SignupInput.EmailOrPhoneNumber, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, SignupInput.Password); // only create the user after specifying his name and password.

            await _userManager.AddToRoleAsync(user, "Normal-User");
            if (result.Succeeded)
            {

                return true;
            }

            else
            {
                return false;
            }


        }


        public async Task<(bool,bool)> LoginUser(LoginViewModel loginInput)
        {   

            //check whether the model is valid or not.
            var context = new ValidationContext(loginInput);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(loginInput, context, results, validateAllProperties: true);
            if (!valid)
            {
                return (false,false);
            }

            // if model is valid

            // 'false' parameter here means : don't lockout. it's just a required parameter, I'm forced to specify it, otherwise I will get error, there is no deeper meaning behind why I specified this value
            // in Program.cs we need options.SignIn.RequireConfirmedAccount =false, otherwise, result will be "NotAllowed"
            ApplicationUser? au=await _userManager.FindByEmailAsync(loginInput.EmailOrPhoneNumber);

            if (au is not null) // why this is needed : If user try to log-in with non-existing email, then the following error will occur :  "Value cannot be null. (Parameter  'user')"
            {
                var result = await _signInManager.PasswordSignInAsync(au, loginInput.Password, loginInput.RememberMe, false);


                if (result.Succeeded)
                {
                    // further check if logged-in   user is admin or not.


                    // I changed the approach; instead of getting admins and checking if the logged-in user is one of them, let's check the logged-in user directly.
                    Task<bool> isAdmin = _userManager.IsInRoleAsync(au, "Admin");
                    if (await isAdmin)
                    {
                        return (true, true);
                    }
                    else
                    {
                        return (true, false);
                    }

                }
                else
                {
                    return (false, false);
                }
            }

            else
            {
                return (false, false);
            }
            
            
            


        }



        /// <summary>
        ///  This method  returns either all the non-admin users in the database or some of the non-admin users in the database based on a filtering policy.
        /// </summary>
        public  async Task<IQueryable<ApplicationUser>> GetUsers(AdminViewModel options)
        {
            ArgumentNullException.ThrowIfNull(options);
            // page size can be constant, while the page number should be dynamic.
            if (options.PageSize < 0 || options.PageNumber <=0)
            {
                throw new InvalidOperationException("Page size and/or Page Number cannot be less than zero.");
            }

            //wantAllUsers will be true when options.keyWord is null or empty.
            var wantAllUsers = string.IsNullOrWhiteSpace(options.KeyWord);
            // I do not want to show the admin user on the Admin panel, because I do not want the admin to delete him self
            // if admins array contains some user, then that user is admin, I do not want to return him.
            // Deferred execution, Where clause returns IQueryable<ApplicationUser>, the enclosing method should return IQueryable<ApplicationUser>, that's better.




            string adminId=await _roleManager.GetRoleIdAsync(await _roleManager.FindByNameAsync("Admin"));

            // Syntax source is the following article "https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries#example---mixed-query-and-method-syntax"


            DbSet<IdentityUserRole<string>> usersRoles = _context.UserRoles;
    //        IQueryable<ApplicationUser> nonAdminUsers = from user in _context.Users // PROBLEM 2000: Enumeration yielded no results.
    //                            join userRole in usersRoles
    //                            .Where(user => !_context.UserRoles
    //.                           Any(ur => ur.UserId == user.Id && ur.RoleId == adminId))// Instead of hardcoding the value, because the Id of the Admin might change, I  think I should do that.
    //                            select new ApplicationUser  // if I do not explicitly specify the object type to be ApplicatonUser , anonymous type will be used by default, I do not want that.
    //                            {
    //                                UserName = user.UserName,
    //                                Email = user.Email
    //                            };
            IQueryable<ApplicationUser> nonAdminUsers =
                    from user in _context.Users
                    join userRole in usersRoles
                        on user.Id equals userRole.UserId into roles
                    where !roles.Any(r => r.RoleId == adminId)
                    select new ApplicationUser
                    {
                        Id=user.Id,
                        UserName = user.UserName,
                        Email = user.Email
                    };



            if (!wantAllUsers)
            {

                // options.keyWord can't be null or empty, because wantAllUsers is false 

                // users is still  IQueryable<ApplicationUser>, no SQL was executed  yet.
                nonAdminUsers = nonAdminUsers.Where(nau => nau.UserName.Contains(options.KeyWord!));
            }
            



            return nonAdminUsers; 

        }




        /// <summary>
        ///  a user with the specified id is deleted if exist in the database.
        /// </summary>
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
   
    
        // Logout does not need the Id of the user to log the user out.
        public async Task<bool> Logout()
        {
            try
            {
                //SignOutAsync() DOES NOT Delete the ".AspNetCore.Antiforgery.dHaNJbBC9RM"  cookie. I saw this in the browser's Dev tools.
                //SignOutAsync() Deletes the ".AspNetCore.Identity.Application." cookie. I saw this in the browser's Dev tools.
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Logout failed\n");
                Debug.WriteLine("Logout failed\n"+e+"\n");
                return false;
            }
            
            
        } 

        public async Task<ApplicationUser?> FindByIdAsync( string Id)
        {
            
            
             return await _userManager.FindByIdAsync(Id);
           
        }


        
        public  async Task<bool> EditUser(User user)
        {
            // fetch the data.
            ApplicationUser? au= await _userManager.FindByIdAsync(user.Id);
            if (au is not null)
            {
                // modify the data
                au.UserName = user.Name;
                au.Email = user.EmailOrPhoneNumber;

                //Write-back to disk. just like cache concepts in computer engineering.
                IdentityResult result =await _userManager.UpdateAsync(au);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // if user is not found, return false.
            return false;
        }

        public async Task<User?> GetUserForEdit(string id) // PROBLEM 5000: this method get different id each time it's called, even though we want to edit the same user
        {
            ApplicationUser? au = await _userManager.FindByIdAsync(id);

            if (au is null)
                return null;
            //Two notes for myself:
            // 1-User is the destination or the required type
            // 2-au is the source or the domain specific object.
            return _mapper.Map<User>(au); 
        }


    } // this is the End of the class

} // this is the  End of the name