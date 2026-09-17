using AutoMapper;
using HardwareStore.Models;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.Extensions.Options;


namespace HardwareStore.Services.AdminServices
{

    public interface IAdmin // I can't find good name for it yet
    {

        public AdminViewModel ModifyAdminPanelOptions(IQueryable<ApplicationUser> applicationUsers, AdminViewModel options);


    }
    public class AdminPanel:IAdmin
    {
        private readonly IMapper _mapper;
        public AdminPanel(IMapper mapper)
        {
            _mapper = mapper;
        }


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
            int numberOfUsersInDatabase = applicationUsers.Count();
            // In math , 11/10 = 1,10.
            // But in programming, 11/10 = 1, because a division of int/int = int, not a double. this is integer division.
            // So let's use floating-point division : (double)11/10=1.1 
            options.TotalPages = (int)Math.Ceiling((double)numberOfUsersInDatabase / options.PageSize);
            
            
            
            return options;
            

        }

    }
}
