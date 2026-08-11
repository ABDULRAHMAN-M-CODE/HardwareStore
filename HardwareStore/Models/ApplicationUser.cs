using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
    }
}
