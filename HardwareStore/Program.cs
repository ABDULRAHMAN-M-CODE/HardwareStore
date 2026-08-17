using HardwareStore.AtStartup;
using HardwareStore.Models;
using HardwareStore.Services;
using HardwareStore.ViewModel.AccountViewModels;
using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//Mental Model about the DI container: 
/*
 
 // Register service along with it's configurations or options, options are not executed right away.

// When some piece of code depends on service from the DI container, two things happen:

    1- Resolve the Service.
    2- Execute the deferred callback to configure the options.
 */

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// the options are callback; they are not executed now, when some code needs the service, the options will be executed.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HardwareDB")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>
    (options => options.SignIn.RequireConfirmedAccount = false)// I intentionally used value of false, because I do not require the user to be confirmed
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();




// I think we can't use  AddSingleton  because it may cause race condition ?
builder.Services.AddScoped<IAccount, UserAccount>(); // fresh  instance of the UserAccount per request, avoiding race condition.

builder.Services.AddScoped<SignupViewModel>();
builder.Services.AddRazorPages();



builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

var app = builder.Build(); // but this returns a configured webApplication, not IserviceProvider



using (var scope = app.Services.CreateScope())
{
    UserAccount userAccount = new UserAccount(
    scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>(), // Here I want specific service, not  the service provider.
    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),// Here I want specific service, not  the service provider.
    scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>()// Here I want specific service, not  the service provider.
);
    myClass x = new myClass(userAccount ); // Note for myself : x is not defined outside the scope.
    await x.CreateRoles(scope.ServiceProvider); // Here, I want the service provider it self, not a specific service.
       
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Home}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

app.Run();



