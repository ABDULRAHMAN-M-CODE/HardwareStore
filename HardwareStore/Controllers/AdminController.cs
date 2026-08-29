using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using Spire.Xls;
using System.CodeDom;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace HardwareStore.Controllers
    
{


    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public IOmniReader reader;
        public   IOmniWriter writer;
        private readonly IAccount _account;
        private readonly ApplicationDbContext _context;
        public AdminController(IOmniReader reader, IOmniWriter writer, IAccount account, ApplicationDbContext context)
        {

            this. reader = reader;
            this.writer = writer;
            _account = account;
            _context = context;
        }

        public async Task<IActionResult> AdminPanel(AdminViewModel options)
        {


            //The Service should only be responisble for domain logic
            // That is, the service did not return ViewModel, it was only responsible for Data Access.
            //Service should not return ViewModel, because it's UI-specific, application-specific, or presentation-specific.
            IQueryable<ApplicationUser> applicationUsers = await _account.GetUsers(options);

            // Now I want to materialize the result, so, I want to convert the deferred LINQ sequence to  in-memory collection, such as list. 
            // I used .ToListAsync(); this will execute the underlying deferred expression.


            //Note for my self: 'Email' column in the database currently has either value of  email or phone number, which is wrong, I should separate those two columns.

            Task<List<User>> SpecificPageUsers = applicationUsers
                .OrderBy(user => user.UserName)
                .Skip((options.PageNumber - 1) * options.PageSize)
                .Take(options.PageSize)
                .Select(user => new User { Name = user.UserName, EmailOrPhoneNumber = user.Email, Id = user.Id })
                .ToListAsync();



            // Can do some work while this Task finishes, but here I don't need to do any other tasks.....

            options.SpecificPageUsers = await SpecificPageUsers;

            int numberOfUsersInDatabase = applicationUsers.Count();
            options.TotalPages = (int)Math.Ceiling((double)numberOfUsersInDatabase / options.PageSize);
            options.DeletionSucceed = TempData["UserDeletionSucceed"] as bool? ?? false; // using null-coalescing operator here is perfect.
            return View(options);
        }

        /// <summary>
        /// This function Initiates the user editing process. it does not edit anything yet.
        /// </summary>
        /// <param name="id">The unique identifier of the user, mapped automatically from the URL route or query string via Model Binding.</param>
        /// <returns>An asynchronous task that renders the user edit view.</returns>
        public async Task<IActionResult> EditUser(string Id)
        {
            ApplicationUser? au = await _account.FindByIdAsync(Id);
            User user = new User
            {
                Name = au?.UserName,
                EmailOrPhoneNumber = au?.Email, // currently, Email in the database holds either Email or phone number, I will change that so that Email will contain the email only if it's email, other wise I will use phoneNumber column.
                Id = au?.Id
            };

            return View(user);
        }

        public async Task<IActionResult> EditConfirmed(User user)
        {

            bool success = await _account.EditUser(user);
            if (success)
            {
                return RedirectToAction("AdminPanel", "Admin"); // NOTE:  maybe I can decide if I want to  use the modal or not  with tempData
            }
            else
            {
                return RedirectToAction("EditUser"); // NOTE: I can also use TempData to show Error message on the UI, for better user experience.
            }

        }


        /// <summary>
        /// 
        /// User confirmed the Deletion by clicking a button inside some Modal\n.  
        /// There was no 'Action method ' that was responsible of showing the Modal to the user.  
        /// 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string Id)
        {
            bool result = await _account.DeleteUser(Id);

            if (result)

            {
                // I will use the POST-REDIRECT_GET pattern, I want to pass data - a bool value- between two action methods in the same controller.
                TempData["UserDeletionSucceed"] = true;
                return RedirectToAction("AdminPanel", "Admin"); // is this what I need ?
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

        public IActionResult ProductsPage()
        {
            return View();
        }


        [HttpPost]
        [Route("Admin/Import")] // but the URL in the fetch should be /Admin/Import
        public  async Task<IActionResult> Import([FromForm] IFormFile file) // PROBLEM : THIs method is no longer reached by the request
        {

            Debug.WriteLine("I'm executed");


            reader.ReadAndWriteData();

            Debug.WriteLine("Success");


           

            
            return Ok();


        }



    }
}
