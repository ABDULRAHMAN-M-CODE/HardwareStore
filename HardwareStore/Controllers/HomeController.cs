using HardwareStore.Models;
using HardwareStore.ViewModel;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

// The following  imports are required for Login IDentity User
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Core;

namespace HardwareStore.Controllers
{


    public class AccountController : Controller
    {

            private readonly ApplicationDbContext _context;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ILogger<AccountController> _logger;
            private readonly IUserStore<ApplicationUser> _userStore;
            private readonly IUserEmailStore<ApplicationUser> _emailStore;
            private readonly IEmailSender _emailSender;
            private readonly UserManager<ApplicationUser> _userManager;
        
        
             /// <Note>
             ///    THe following properties  supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </Note>

            public string ReturnUrl { get; set; }
            [BindProperty]
            public InputModel Input { get; set; } //related to Login, not signup.
            public SignupViewModel SignupInput { get; set; }
            public IList<AuthenticationScheme> ExternalLogins { get; set; }
  
            [TempData]
            public string ErrorMessage { get; set; }


            public AccountController(ApplicationDbContext context, 
                SignInManager<ApplicationUser> signInManager, 
                ILogger<AccountController> logger,
                IUserStore<ApplicationUser> userStore,
                IEmailSender emailSender,
                UserManager<ApplicationUser> userManager){
                _context = context;
                _userStore = userStore;
                _signInManager = signInManager;
                _logger = logger;
                
                _emailSender=emailSender;
                _userManager = userManager;
                _emailStore = GetEmailStore();
               
            }
            public IActionResult Index()
            {
                return View();
            }
            [HttpGet]
            public async Task<IActionResult> Login(string returnUrl = null)
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                returnUrl ??= Url.Content("~/");

                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
                return View( new LoginViewModel());  
            }

            [HttpPost]
            public async Task<IActionResult> ViewAfterProcessingLoginRequest(string returnUrl = null)
            {
                //returnUrl ??= Url.Content("~/");

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                    return RedirectToAction("LoginSuccesful");
                        //return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction("LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction("Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return RedirectToAction("InvalidLoginAttempt");
                }
                }

                // If we got this far, something failed, redisplay form
                return View();
            }
            public IActionResult InvalidLoginAttempt()
            {
                return View();
            }
            public IActionResult ForgotPassword()
            {
                return View();
            }
            public IActionResult ResendEmailConfirmation()
            {
                return View();
            }

            public IActionResult Lockout()
            {
                return View();
            } 
            public IActionResult LoginWith2fa()
            {
                return View();
            }

            public IActionResult LoginSuccesful()
            {
                return View();
            }

            #nullable disable
            [HttpGet]
            public IActionResult Logout()
             {
                return View();
             }

            [HttpPost]
            public async Task<IActionResult> LogoutConfirmed()
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");

                return RedirectToAction("Login");

            }


            // The following are Action methods related to Signup 
            public async  Task<IActionResult>    Register (string returnUrl = null)
            {
                ReturnUrl = returnUrl;
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                return View(new SignupViewModel() );
            }

            public async Task<IActionResult> RegisterAlgorithm(SignupViewModel SignupInput, string returnUrl = null)
            {
                returnUrl ??= Url.Content("~/");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                
                if (ModelState.IsValid) 
                {
                    var user = CreateUser();
               
                     await _userStore.SetUserNameAsync(user, SignupInput.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, SignupInput.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, SignupInput.Password);


                // Note : the following code does not work, it's still uses some Razor-pages specific syntax like the RedirectToPage functions
                if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(SignupInput.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("/RegisterConfirmation", new { email = SignupInput.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                    // If we got this far, something failed, redisplay form or show Error UI
                    return RedirectToAction("SignupError");
            }

            private ApplicationUser CreateUser()
            {
                try
                {
                    return Activator.CreateInstance<ApplicationUser>();
                }
                catch
                {
                    throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                        $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }

            private IUserEmailStore<ApplicationUser> GetEmailStore()
            {
                if (!(_userManager.SupportsUserEmail))
                {
                    throw new NotSupportedException("The default UI requires a user store with email support.");
                }
                return (IUserEmailStore<ApplicationUser>)_userStore;
            }
        
            public IActionResult SignupError()
            {
                return View();
            }



        // End of AccountController
    }

//End of the namespace
}


