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
        public   IOmniReader writer;
        private readonly IAccount _account;
        private readonly ApplicationDbContext _context;
        public AdminController(IOmniReader reader, IOmniReader writer, IAccount account, ApplicationDbContext context)
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
        public  async Task<IActionResult> Import([FromForm] IFormFile file)
        {

            Console.Write("Action method is called\n");

            //// don't forget to refactor the code.
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid Excel file.");
            }





            // Excel processing logic goes here
            var extension = Path.GetExtension(file.FileName);

            if (extension != ".xlsx" && extension != ".xls")
            {
                return BadRequest("Only Excel files are supported.");
            }


            // I learned that In C#, the using statement is a
            // construct that is used for objects that implement
            // the IDisposable interface. Its main purpose is resource
            // cleanup, that is, properly releasing the resources
            // (such as files, streams, network connections) of an
            // object after it is used.
            using var stream = file.OpenReadStream(); // using = 'with open' keyword in python, conceptually
            Workbook workbook = new Workbook();
            workbook.LoadFromStream(stream);

            Worksheet sheet = workbook.Worksheets[0];

            //////////////////////////////////////////////////////////
            /// ######Populate  Units#####
            /// 
            List<string> relevantUnitPropertiesNames = new List<string>
            {
                "EnglishName",

            };

           List<Unit> units = new List<Unit>(); // MAKE Type Generic, genralise variable name
            List<object> unitsNamesDuplicates = new List<object>();
            // Logic : I want loop over all the rows
            for (int row = 2; row <= sheet.LastRow; row++)//assuming row 1 is header, want skip it.
            {

                Unit unit = new Unit(); // MAKE MAKE this type generic
                Type type = typeof(Unit); // MAKE type generic

                // Populate all the relevant properties of a single Entity
                for (int i = 0; i < relevantUnitPropertiesNames.Count; i++)// method parameter
                {


                    string relevantPropertyName = relevantUnitPropertiesNames[i];//Update
                    var property = type.GetProperty(relevantPropertyName);
                    // Inserts a space before any uppercase letter that follows a lowercase letter

                    //string relevantHeaderName = Regex.Replace(relevantPropertyName, "(?<=[a-z])(?=[A-Z])", " ");

                    //int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column;
                    int relevantColumnNumber = sheet.FindString("Unit", false, false).Column; // Make string as parameter

                    
                  
                    var value = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, property!.PropertyType);


                    property.SetValue(unit, value); // MAKE this generic



                } // end of inner for loop



                if (!unitsNamesDuplicates.Contains(unit.EnglishName!))
                {
                    units.Add(unit);
                    unitsNamesDuplicates.Add(unit.EnglishName!);
                }



            }// end of outermost for loop


            foreach (Unit unit in units)
            {

                // check if the entity already in the database or not
                var foundEntity = await _context.Units
.FirstOrDefaultAsync(u => u.EnglishName == unit.EnglishName);

                if (foundEntity == null)
                {

                  
                        

                        _context.Units.Add(unit);
                      


                }
            }


            await _context.SaveChangesAsync(); 
            /// # Populate Units END
            //////////////////////
          

            // ####Populate suppliers#########
            List<string> relevantSupplierPropertiesNames = new List<string>
            {
                "EnglishName",

            };
           

            List<Supplier> suppliers = new List<Supplier>();
            List<object> suppliersDuplicates = new List<object>();
            // Logic : I want loop over all the rows
            for (int row = 2; row <= sheet.LastRow; row++)//assuming row 1 is header, want skip it.
            {

                Supplier supplier = new Supplier(); // MAKE MAKE this type generic
                Type type = typeof(Supplier); // MAKE type generic

                // Logic : I'm trying to populate the Product object, I'm not interested about all the columns
                for (int i = 0; i < relevantSupplierPropertiesNames.Count; i++)//MAKE this method parameter
                {


                    string relevantPropertyName = relevantSupplierPropertiesNames[i];//MAKE this method parameter

                    // Inserts a space before any uppercase letter that follows a lowercase letter

                    //string relevantHeaderName = Regex.Replace(relevantPropertyName, "(?<=[a-z])(?=[A-Z])", " ");

                    //int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column;
                    int relevantColumnNumber = sheet.FindString("Supplier", false, false).Column; // Make string as parameter

                    var property = type.GetProperty(relevantPropertyName);
                    // value is the english name.
                    var value = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, property!.PropertyType);


                    property.SetValue(supplier, value); // MAKE this generic



                } // end of inner for loop


                if (suppliersDuplicates.Contains(supplier.EnglishName) is false)
                {
                    suppliers.Add(supplier);
                    suppliersDuplicates.Add(supplier.EnglishName);
                }



            }// end of outermost for loop

            foreach (Supplier supplier in suppliers) // MAKE MAKE MAKE
            {

                //_context.Products.Add(brandSupplier);
                _context.Suppliers.Add(supplier);
            }

            
            // populate supplpiers END
            //////////////////////////
            
            List<string> relevantBrandPropertiesNames = new List<string>
            {
                "EnglishName",

            };

            List<Brand> brands = new List<Brand>();
            List<object> duplicates = new List<object>();
            // Logic : I want loop over all the rows
            for (int row = 2; row <= sheet.LastRow; row++)//assuming row 1 is header, want skip it.
            {

                Brand brand = new Brand(); // MAKE this type generic
                Type type = typeof(Brand); // MAKE type generic

                // Logic : I'm trying to populate the Product object, I'm not interested about all the columns
                for (int i = 0; i < relevantBrandPropertiesNames.Count; i++)//MAKE this method parameter
                {


                    string relevantPropertyName = relevantBrandPropertiesNames[i];//MAKE this method parameter

                    // Inserts a space before any uppercase letter that follows a lowercase letter

                    //string relevantHeaderName = Regex.Replace(relevantPropertyName, "(?<=[a-z])(?=[A-Z])", " ");

                    //int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column;
                    int relevantColumnNumber = sheet.FindString("Brand", false, false).Column; // Make string as parameter

                    var property = type.GetProperty(relevantPropertyName);
                    // value is the english name.
                    var value = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, property!.PropertyType);


                    property.SetValue(brand, value); // MAKE this generic



                }


                if (duplicates.Contains(brand.EnglishName) is false)
                {
                    brands.Add(brand);
                    duplicates.Add(brand.EnglishName);
                }



            }



            foreach (Brand brand in brands)
            {

                //_context.Products.Add(brandSupplier);
                _context.Brands.Add(brand);
            }


            await _context.SaveChangesAsync();



            for (int i=0;i<brands.Count;i++)
            {
                BrandSupplier bs = new BrandSupplier
                {
                    BrandId = brands[i].Id,
                    SupplierId = suppliers[i].Id
                };
                _context.BrandsSuppliers.Add(bs);

            }
            await _context.SaveChangesAsync();



            ///*relevantPropertiesNames excludes the navigation properties and the Foreign keys in the Product Model */
            //List<string> relevantProductPropertiesNames = new List<string>
            //{
            //    "SKU",
            //    "Barcode",
            //    "Manufacturer",
            //    "EnglishName",
            //    "ArabicName",
            //    "MinStock",
            //    "ReorderQty",
            //    "Bin",
            //    "Description",
            //    "Status",
            //    "VAT",

            //};

            ////List<Product> products = new List<Product>();// Old approach
            //List<Product> products = new List<Product>();




            //// Logic : I want loop over all the rows.
            //for (int row = 2; row <= sheet.LastRow; row++)//assuming row 1 is header, want skip it.
            //{

            //    Product product = new Product();
            //    Type productType = typeof(Product);

            //    // Logic : I'm trying to populate the Product object, I'm not interested about all the columns
            //    for (int i = 0; i < relevantProductPropertiesNames.Count; i++)
            //    {


            //        string relevantPropertyName = relevantProductPropertiesNames[i];

            //        string relevantHeaderName = relevantPropertyName;// code readability

            //        // Problem : bad.  that line of code required changing the names of the columns in the excel, I want to make this dynamic
            //        // Capitalize each word in relevantHeaderName
            //        // remove spaces in relevantHeaderName

            //        int relevantColumnNumber = sheet.FindString(relevantHeaderName, false, false).Column; 
            //        // I learned that Dynamic property (at run time) does not work in c#, reflection  concept is the solution.
            //        //Line 5 : product.propertyName= sheet.Range[row, column].Text;



            //        // productType.GetProperty(columnName)?.SetValue(product, sheet.Range[row, column].Text); // causes types issues.
            //        var property = productType.GetProperty(relevantPropertyName);
            //           // now the new problem is : given relevantHeaderName, how to get column number?
            //            var value = Convert.ChangeType(sheet.Range[row, relevantColumnNumber].Value, property!.PropertyType);

            //            property.SetValue(product, value);
            //    }



            //    products.Add(product);

            //}
            //foreach (Product product in products)
            //{

            //    _context.Products.Add(product);
            //    //Console.WriteLine(product.EnglishName);
            //  }

            //await _context.SaveChangesAsync();

            // finally  redirect the user to the ProductsPage.
            return Ok();


        }



    }
}
