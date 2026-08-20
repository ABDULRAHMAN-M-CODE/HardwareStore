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
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
    var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    UserAccount userAccount = new UserAccount(userStore, userManager,signInManager, roleManager,context);
    CustomRoleManager customeRoleManager = new CustomRoleManager(userAccount,userManager, scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>() ); 
     Task<bool> success= customeRoleManager.CreateRoles();
    if (await success)
    {
        await customeRoleManager.AddRoleToUser("abd@gmai.com", "Admin", "abd","2811998@Ma@7799NeonShadowX1ADMINTlaonAniviaZedLeagueOfLegends");
    }
       
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



