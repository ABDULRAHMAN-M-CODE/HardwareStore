using HardwareStore.Models;
using HardwareStore.ViewModel.AccountViewModels;

namespace HardwareStore.Services.UserAccountServices
{
    public interface IAccount
    {
        //public async Task<bool> RegisterUser(SignupViewModel SignupInput); // I will  get error : async should be used with methods that have body
        //  async Task<bool> is wrong.
        Task<bool> RegisterUser(SignupViewModel SignupInput);
        Task<(bool, bool)> LoginUser(LoginViewModel loginInput); // THe second bool value indicates whether the logged-in user is admin or not.
        Task<IQueryable<ApplicationUser>> GetUsers(AdminViewModel options);
        Task<bool> DeleteUser(string id);
        Task<bool> Logout();
        //Task<ApplicationUser?> FindByIdAsync(string Id); : Old
        Task<User?> GetUserForEdit(string id); // new.
        Task<bool> EditUser(User user);



    }
}
