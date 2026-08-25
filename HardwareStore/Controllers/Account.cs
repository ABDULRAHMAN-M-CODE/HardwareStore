using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;

using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{


    public class AccountController : Controller
    {
        private readonly IAccount _account;
             

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



        // The following 5 action methods are related to Login
        [HttpGet]    
        public IActionResult Login()
            {

                // the view should get empty model, the model's fields will be populated by the user using a form.
                return View(new LoginViewModel());
            }
        [HttpPost]
        public async Task<IActionResult> ProcessLoginRequest(LoginViewModel loginModel)
            {

                (  bool success,  bool isAdmin) = await _account.LoginUser(loginModel);

                if (success)
                {
                    if (isAdmin)
                    {
                        return RedirectToAction("AdminPanel", "Admin");
                      
                    }
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

        public  async Task<IActionResult> Logout()
        {
            Task<bool> success = _account.Logout();
            if (await success)
            {
                return RedirectToAction("Login");
            }
            else
            {
                // use the tempData to show a modal that says "Logout was not successful"
                return RedirectToAction("AdminPanel","Admin");
            }
                
        }







    }

   
}


