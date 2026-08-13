using Microsoft.AspNetCore.Authentication;
using HardwareStore.ViewModel.AccountViewModels;

namespace HardwareStore.ViewModel.AccountViewModels
{
    public class LoginViewModel
    {
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
