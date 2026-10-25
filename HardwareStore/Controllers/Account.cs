using HardwareStore.Services.UserAccountServices;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{



    public class AccountController(IAccount _account) : Controller
    {
        #region Signup actions        
        public IActionResult Register()
        
        {
            RegistrationViewModel viewModel= new RegistrationViewModel();
            viewModel.signupViewModel=new SignupViewModel();

            viewModel.registrationResult = System.Text.Json.JsonSerializer.Deserialize<RegistrationResult>(
    TempData["RegistrationResult"]?.ToString() ?? "{}");

            
            return View(viewModel);

        }
                 
        public async Task<IActionResult> RegisterAlgorithm([Bind(Prefix = "signupViewModel")] SignupViewModel signupInput)
            
        {   
            RegistrationResult result =  await _account.RegisterUser(signupInput); 
            if (result.IsSucceeded)
            {                        
                return RedirectToAction("SignupSuccessful");                    
            }
            TempData["RegistrationResult"] = System.Text.Json.JsonSerializer.Serialize(result);
            return RedirectToAction("Register");         
        }
                  
        public IActionResult SignupSuccessful()            
        {              
            return View();            
        }

        #endregion

        #region Login methods
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

                return RedirectToAction("AdminPanel","Admin");
            }
                
        }

        #endregion

        public IActionResult Home()
        {

            return View();

        }



    }


}


