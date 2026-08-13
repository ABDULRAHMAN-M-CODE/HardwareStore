using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
    }
    

}
