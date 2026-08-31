using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        // additional column besides the ones that are inherited from IdentityUser
        public DateTime RegistrationDate { get; set; }




        //Naviagation properties
        public IEnumerable<Brand>Brands { get; } = new List<Brand>();
        public IEnumerable<Category> Categories { get; } = new List<Category>();
        public IEnumerable<Country> Countries { get; } = new List<Country>();
        public IEnumerable<Product> Products { get; } = new List<Product>();

        public IEnumerable<SubCategory> SubCategories { get; } = new List<SubCategory>();
        public IEnumerable<Supplier> Suppliers { get; } = new List<Supplier>();

        public IEnumerable<Unit> Units { get; } = new List<Unit>();



    }


}
