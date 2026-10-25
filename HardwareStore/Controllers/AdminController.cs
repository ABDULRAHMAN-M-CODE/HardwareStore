using HardwareStore.Models;
using HardwareStore.Services.AdminServices;
using HardwareStore.Services.AdminServices.ImportProductsFromExcelFile;
using HardwareStore.Services.ProductsRepositoryServices;
using HardwareStore.Services.UserAccountServices;
using HardwareStore.ViewModel;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HardwareStore.Controllers
    
{

    public class GetProductsPayload
    {

        public int PageNumber { get; set; }

    }
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        //Reddit question..
        private readonly ISourceToDistinationDataSyncronizer<Stream> _syncronizer;

        private readonly IAccount _account;
        private readonly IAdmin _admin;
        private readonly IProductsRepository _productsRepository;
        public AdminController(

            ISourceToDistinationDataSyncronizer<Stream> syncronizer,
            
            IAccount account, 
            IAdmin admin,
            IProductsRepository productsRepository
            )
        {

            _syncronizer= syncronizer;
            _account = account;
            _admin=admin;

            _productsRepository = productsRepository;
        }

        public async Task<IActionResult> AdminPanel(AdminViewModel options)
        {


           
            IQueryable<ApplicationUser> applicationUsers = await _account.GetUsers(options);

            
            
            options = _admin.ModifyAdminPanelOptions(applicationUsers,options);


            //Note : the following line of code cannot be moved to a service. because I used TempData
            options.DeletionSucceed = TempData["UserDeletionSucceed"] as bool? ?? false; 
             
 
            return View(options);
        }
 

        public async Task<IActionResult> EditUser(string Id)
        {

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
        [Route("Admin/Import")]
        public  async Task<IActionResult> Import(IFormFile file,CancellationToken cancellationToken) 
        {

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"){
                return BadRequest();
            }

            await _syncronizer.SyncronizeAsync(file.OpenReadStream(), cancellationToken);


            return Ok();
        
        }



        [HttpPost]
        [Route("Admin/GetProducts")]
        public async Task<IActionResult> GetProducts( [FromBody] GetProductsPayload payload)
        {


            IQueryable<Product> allProducts=_productsRepository.GetAll();
            IEnumerable<ProductViewModel> result=_admin.GetSpecificPageProducts(allProducts,PageNumber:payload.PageNumber);//PageNumber should be 5, but it's zero.
            return Ok(result);

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
