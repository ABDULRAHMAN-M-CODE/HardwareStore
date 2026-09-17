
namespace HardwareStore.Controllers
    
{
    using HardwareStore.Models;
    using HardwareStore.SeedWork;
    using HardwareStore.Services;
    using HardwareStore.Services.AdminServices;
    using HardwareStore.ViewModel.AccountViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StackExchange.Profiling;

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public IOmniReaderWriter _readerWriter;
        private readonly IAccount _account;
        private readonly IAdmin _admin;
        public AdminController(
            IAccount account, 
            IAdmin admin, 
            IOmniReaderWriter readerWriter
            )
        {
            _readerWriter = readerWriter;
            _account = account;
             _admin=admin;
        }

        public async Task<IActionResult> AdminPanel(AdminViewModel options)
        {


           
            IQueryable<ApplicationUser> applicationUsers = await _account.GetUsers(options);

            
            
            options = _admin.ModifyAdminPanelOptions(applicationUsers,options);


            //Note : the following line of code cannot be moved to a service. because I used TempData
            options.DeletionSucceed = TempData["UserDeletionSucceed"] as bool? ?? false; 
             
            return View(options);
        }

        
        
        /// <summary>
        /// This function Initiates the user editing process. it does not edit anything yet.
        /// </summary>
        /// <param name="id">The unique identifier of the user, mapped automatically from the URL route or query string via Model Binding.</param>
        /// <returns>An asynchronous task that renders the user edit view.</returns>
        public async Task<IActionResult> EditUser(string Id)
        {
            //Task<User?> GetUserForEdit(string id);
            User? user = await _account.GetUserForEdit(Id);


            if (user != null)
            {
                return View(user);
            }
            return NotFound();

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
        [Route("Admin/Import")] //URL in the fetch should be /Admin/Import
        public  async Task<IActionResult> Import() 
        {



            using (MiniProfiler.Current.Step("ReadWrite"))
            {
                Task<bool> success = _readerWriter.ReadWrite();
                if (await success)
                {
                    return Ok();
                }
                return BadRequest();
            }


            


        }
        public IActionResult ImportResult()
        {

            return View();
        }

        public IActionResult ImportError()
        {
            return View();
        }



    }
}
