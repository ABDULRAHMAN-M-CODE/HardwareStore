using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Identity;




namespace HardwareStore.AtStartup
{


    ///<summary>
    /// Note for Quitba : This class is meant to be used  to create roles in the database and to attach roles to users.
    /// This class can be a safe server-side methodology   to create admin users instead of creating admins using a client-side UI, it's better for security.
    ///</summary> 
    public class CustomRoleManager{

        private readonly IAccount _account;
        private readonly UserManager<ApplicationUser> _userManager;
        private  readonly RoleManager<IdentityRole> _roleManager;
        public CustomRoleManager(IAccount account,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            //serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _account = account;
            _userManager = userManager;
            _roleManager = roleManager;

        }


        ///<summary>
        /// Creates Admin, Store-Manager, and Normal-user Roles if they are not already created.
        /// 
        ///</summary> 
        public async Task<bool> CreateRoles()
        {

            // first we need to create roles if they do not exist.
            string[] roleNames = { "Admin", "Store-Manager", "Normal-User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    
                }
            }

            return true;

        }


        /// <summary>
        /// checks if the user with the given email exist in the database, if it exist,  user will be given a role.
        /// if the user with the given email does not exist in the database, it will be created first. 
        /// Observation : if I changed the  method's signature to be 'async void' instead of 'async Task', the code at line 65  will throw the following error:Invalid operation. The connection is closed.
        /// Constraint : the name should be unique; for example, if there is already user with name "abd" in the database, you can't call this method with that name.
        /// 
        /// </summary>
        public async Task AddRoleToUser(string email , string role, string name,string password)
        {
            
            var foundUserWithGivenEmail = await _userManager.FindByEmailAsync(email); // throws error if I changed method's signature to 'async void'.
            
            
            if (foundUserWithGivenEmail == null)
            {


                // Manually Creating the model, because this code is not bound to http request.
                SignupViewModel model = new SignupViewModel
                {
                    Name=name,
                    EmailOrPhoneNumber = email,
                    Password = password,
                    ConfirmPassword = password
                };
                bool success =  await _account.RegisterUser(model);

                if  (success)
                {
                    //here we tie the  user to the role
                    var createdUser= await _userManager.FindByEmailAsync(email);
                    await _userManager.AddToRoleAsync(createdUser, role);

                }
            }

            else
            {
                await _userManager.AddToRoleAsync(foundUserWithGivenEmail, role);
            }
        
        }

    }


}
