using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace HardwareStore.Controllers
{


    public class AccountController : Controller
    {
        private readonly IAccount _account; // account may be UserAccount or AdminAccount, right qutaiba ? should I make implementation for AdminAccount?
             

        public AccountController( IAccount account)
            {
                
                _account=account;
           }
            
        
        public IActionResult Home()
            {
                return View();
            }



        // the following 4 Action methods are related to Signup.
            
        public IActionResult Register()
            {

                // the view should get empty model, the model's fields will be populated by the user using a form.
                return View(new SignupViewModel());

            }

        
            
        public async Task<IActionResult> RegisterAlgorithm(SignupViewModel signupInput)
            {

            // JUSTIFICATION : why I used Asyncrouns programming instead of Syncrouns Programming:
            /*
                I treated creating the user as a task, I can create many users at the same time without the need to make this a sequential process.

                When the compiler reaches the line 81, because there is await, the enclosing action method will be suspended - yields control to the caller-.

                The Thread is free to do other work; handling new request, this enhances performance.

             */

                
                bool success =  await _account.RegisterUser(signupInput); // line 81. next lines of code will not be executed unless this line complete it's work, but the thread will be able to handle new request
                  
                // the following 'if-statement' is not considered 'business logic'; it's just a 'presentational (UI) logic', I'm deciding which UI should be rendered based on the result of the 'bussines logic'.
                // so the following if statement should not be inside the Services Folder, it should be inside the Action Method in the controller.
                 if (success)
                    {
                        return RedirectToAction("SignupSuccessful");
                    }
                    return RedirectToAction("SignupError"); 
        }
            
            
        public IActionResult SignupError()
            {
                return View();
            }
            
        public IActionResult SignupSuccessful()
            {
                return View();
            }


        // The following 4 action methods are related to Login

            [HttpGet]
            
        public IActionResult Login()
            {

                // the view should get empty model, the model's fields will be populated by the user using a form.
                return View(new LoginViewModel());
            }

            [HttpPost]
            
        public async Task<IActionResult> ProcessLoginRequest(LoginViewModel loginModel)
            {

                bool success = await _account.LoginUser(loginModel);

                if (success)
                {
                    return RedirectToAction("LoginSuccess");
                }
                
                else
                {
                    return RedirectToAction("LoginError");
                }
                
            }
            
        public IActionResult LoginSuccess()
            {
                return View();
            }
            
        public IActionResult LoginError()
            {
                return View();
            }
        

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPanel( AdminViewModel options)
        {

            
            //The Service should only be responisble for domain logic
            // That is, the service did not return ViewModel, it was only responsible for Data Access.
            //Service should not return ViewModel, because it's UI-specific, application-specific, or presentation-specific.
            IQueryable<ApplicationUser> applicationUsers =_account.GetUsers(options);
            
            // Now I want to materialize the result, so, I want to convert the deferred LINQ sequence to  in-memory collection, such as list. 
            // I used .ToListAsync(); this will execute the underlying deferred expression.

            //Note for my self: 'Email' column in the database currently has either value of  email or phone number, which is wrong, I should separate those two columns.
            
            Task<List<User>> SpecificPageUsers=   applicationUsers.OrderBy(user => user.UserName).Skip((options.PageNumber - 1) * options.PageSize).Take(options.PageSize).Select(user=> new User {Name=user.UserName,EmailOrPhoneNumber=user.Email,Id=user.Id }).ToListAsync();

            

            // Can do some work while this Task finishes, but here I don't need to do any other tasks.....
            
            options.SpecificPageUsers = await SpecificPageUsers;
            
            int numberOfUsersInDatabase= applicationUsers.Count();
            options. TotalPages = (int)Math.Ceiling((double)numberOfUsersInDatabase / options.PageSize);
            options.DeletionSucceed = TempData["UserDeletionSucceed"] as bool? ?? false; // using null-coalescing operator here is perfect.
            return View(options); 
        }


        public IActionResult DeleteUserConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string Id)
        {
            bool result = await  _account.DeleteUser(Id);
            
            if (result)

            {
                // I will use the POST-REDIRECT_GET pattern, I want to pass data - a bool value- between two action methods in the same controller.
                TempData["UserDeletionSucceed"] = true;
                return RedirectToAction("AdminPanel"); // is this what I need ?
            }
            else
            {
                TempData["UserDeletionSucceed"] = false; // maybe this is not needed, for now, unless I want add "Deletion failed Modal"
                return RedirectToAction("CouldNotDeleteUser");
            }
        }

        public IActionResult CouldNotDeleteUser()
        {
            return View();
        }


    }

   
}


