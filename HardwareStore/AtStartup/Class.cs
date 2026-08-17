using AspNetCoreGeneratedDocument;
using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace HardwareStore.AtStartup
{


    public class myClass{

        private readonly IAccount _account;
        public myClass(IAccount account)
        {
            _account = account;
            
        }
        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // first we need to create roles if they do not exist.

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();//  when I step into this using debugger, the debugger jumps to the line of code that creates the options for the DbOContext.
            Debug.WriteLine("RoleManager is :"+ RoleManager);
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Debug.WriteLine("UserManager is  :"+UserManager);
            string[] roleNames = { "Admin", "Store-Manager", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                Debug.WriteLine("does this role exist ?+"+ roleExist);
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    Debug.WriteLine("result after creating the role is  :"+ roleResult);
                }
            }

            // find the user with the admin email 
            var _user = await UserManager.FindByEmailAsync("admin@email.com");
            Debug.WriteLine("The Admin user is :"+ _user);

            
            // if there is not Admin in the database, then, let's create one.
            if (_user == null)
            {

                
                // Manually Creating the model, because this code is not bound to http request.
              SignupViewModel model = new SignupViewModel
              {
                  EmailOrPhoneNumber = "Qutaiba@gmail.com",
                  Password = "2811998@MNeonShadowX1IsAdmin",
                  ConfirmPassword = "2811998@MNeonShadowX1IsAdmin"
              };
                Task<bool> result = _account.RegisterUser(model);

                if ( await result)
                {
                    //here we tie the new user to the role
                    var poweruser = await UserManager.FindByEmailAsync("Qutaiba@gmail.com");
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }

    }


}
