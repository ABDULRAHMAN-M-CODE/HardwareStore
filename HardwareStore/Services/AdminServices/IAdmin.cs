using AutoMapper;
using HardwareStore.Models;
using HardwareStore.ViewModel;
using HardwareStore.ViewModel.AccountViewModels;




namespace HardwareStore.Services.AdminServices
{



    public interface IAdmin // I can't find good name for it yet
    {


        public AdminViewModel ModifyAdminPanelOptions(IQueryable<ApplicationUser> applicationUsers, AdminViewModel options);
        public IEnumerable<ProductViewModel> GetSpecificPageProducts(IQueryable<Product> allProducts, int PageSize = 8, int PageNumber = 1);



    }
    public class AdminPanel(IMapper _mapper) : IAdmin
    {



        //PROBLEM FOR LATER : Change `ModifyAdminPanelOptions` to match the archeticture of `GetSpecificPageProducts`
        public AdminViewModel ModifyAdminPanelOptions(IQueryable<ApplicationUser> applicationUsers, AdminViewModel options)
        {


            List<ApplicationUser> SpecificPageApplicationUsers = applicationUsers
                .OrderBy(au => au.UserName)
                .Skip((options.PageNumber - 1) * options.PageSize)
                .Take(options.PageSize).
                ToList();
           

            List<User> SpecificPageUsers = new List<User>();
            foreach (ApplicationUser au in SpecificPageApplicationUsers)
            {
                SpecificPageUsers.Add(_mapper.Map<User>(au));
            }
       
            options.SpecificPageUsers = SpecificPageUsers;

            // In math , 11/10 = 1,10.
            // But in programming, 11/10 = 1, because a division of int/int = int, not a double. this is integer division.
            // So let's use floating-point division : (double)11/10=1.1 
            options.TotalPages = (int)Math.Ceiling((double)applicationUsers.Count() / options.PageSize);
            //users per page= page size= users/number of pagespag


            return options;
            

        }
       
        

        public IEnumerable<ProductViewModel> GetSpecificPageProducts(IQueryable<Product> applicationProducts, int PageSize =8, int PageNumber = 1)
        {
            List<Product> SpecificPageApplicationProducts = applicationProducts
                .OrderBy(ap => ap.EnglishName)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize).
                ToList();


            var result = new List<ProductViewModel>();
            foreach (Product spp in SpecificPageApplicationProducts)
            {
                result.Add(_mapper.Map<ProductViewModel>(spp));
            }

            return result;



        }
    }


    
}
