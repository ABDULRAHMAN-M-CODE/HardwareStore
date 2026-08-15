using HardwareStore.Models;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStore.Services;
namespace HardwareStore.Controllers
{


    public class AccountController : Controller
    {



            
            //  The reason I used  readonly is to ensure that the instance is
            //  not accidentally modified by any of the controller's methods. 

            private readonly IUserStore<ApplicationUser> _userStore;        // to set the user name
            
            private readonly UserManager<ApplicationUser> _userManager;   //  actually create the user in the database, given password and 

            private readonly SignInManager<ApplicationUser> _signInManager;
        

            public AccountController
                (
                    
                    SignInManager<ApplicationUser> signInManager, 
                    IUserStore<ApplicationUser> userStore,
                    UserManager<ApplicationUser> userManager
                )
            {
                
                _signInManager = signInManager;
                _userManager = userManager;
                _userStore = userStore;
            }
            public IActionResult Index()
            {
                return View();
            }



        // the following 4 Action methods are related to Signup.
            public IActionResult Register()
            {

                // the view should get empty model, the model's fields will be populated by the user using a form.
                return View(new SignupViewModel());

            }

        
            public async Task<IActionResult> RegisterAlgorithm(SignupViewModel SignupInput)
            {

            // JUSTIFICATION : why I used Asyncrouns programming instead of Syncrouns Programming:
            /*
                I treated creating the user as a task, I can create many users at the same time without the need to make this a sequential process.

                When the compiler reaches the line 81, because there is await, the enclosing action method will be suspended - yields control to the caller-.

                The Thread is free to do other work; handling new request, this enhances performance.

             */

                AccountService service = new AccountService(_userStore, _userManager, SignupInput);
                bool success =  await service.RegisterUser(); // line 81. next lines of code will not be executed unless this line complete it's work, but the thread will be able to handle new request
                  
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
            public async Task<IActionResult> ProcessLoginRequest(LoginViewModel model)
            {

                if (ModelState.IsValid)
                {

                    // Note : there is no need to put the following line of code in the Services folder, it's just single line of code.
                    // 'false' parameter here means : don't lockout. it's just a required parameter, I'm forced to specify it, otherwise I will get error, there is no deeper meaning behind why I specified this value
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    // we need options.SignIn.RequireConfirmedAccount =false, otherwise, result will be "NotAllowed"

                    // the following is just a presentational logic
                    if (result.Succeeded)
                    {

                        return RedirectToAction("LoginSuccess");
                   
                    }

                    else
                    {
                 
                    
                        return RedirectToAction("LoginError");
                    }
                }

               // data validation error.
                return RedirectToAction("LoginError");
            }
            public IActionResult LoginSuccess()
            {
                return View();
            }
            public IActionResult LoginError()
            {
                return View();
            }

        
    }

   
}


