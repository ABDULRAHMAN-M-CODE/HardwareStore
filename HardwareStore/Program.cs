using AutoMapper;
using HardwareStore.AtStartup;
using HardwareStore.Models;
using HardwareStore.SeedWork;
using HardwareStore.Services;
using HardwareStore.Services.AdminServices;
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
string amlickey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODE5NTg0MDAwIiwiaWF0IjoiMTc4ODExMTAwNiIsImFjY291bnRfaWQiOiIwMWEwNTM3M2YxOTg3OTE0YTUxZWUwMWJkOTcwYzBmYiIsImN1c3RvbWVyX2lkIjoiMDFhMDUzNzNmMTk4NzkxNGE1MWVlMDFiZDk3MGMwZmIiLCJzdWJfaWQiOiItIiwiZWRpdGlvbiI6IjAiLCJ0eXBlIjoiMiJ9.o3aACPQzkt57aSJTef6XmOGI-h9vLnsYjCjeG8yhQl7EXQLuLBbLLxhnBo5NSy8Xufa9I4IDAPOzFxfmV6FUEUrFpxYSRDhud9lLe69k4EimmTXOi5zvCgd5XgLtc_ir_RqMwXL7toFKFAwMpJyxMwPXg1WIu9DS9xDfi_6QdINZO2BdlBkVkwM-QEaKzFcJGM8opN2pH3I6Cyv2HrZv7mOfzBfKrta2jF2ozWWTRRr2nHDiRub6tkRnylvbys5QNxeG-4HCoysrrPVZUZWL35s4WUS1GyBIKZScXBo6dEy9RFxVkW7ikg5crsJKskD6O8SSp0sa1gxQzur6R2scvg";



builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = amlickey, typeof(Program));
builder.Services.AddControllersWithViews();
builder.Services.AddMiniProfiler();

// the options are callback; they are not executed now, when some code needs the service, the options will be executed.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HardwareDB") , 
    providerOptions => providerOptions.EnableRetryOnFailure())
    );

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>
    (options => options.SignIn.RequireConfirmedAccount = false)// I intentionally used value of false, because I do not require the user to be confirmed
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddHttpContextAccessor();
// I think we can't use  AddSingleton  because it may cause race condition ?
builder.Services.AddScoped<IAccount, UserAccount>(); // fresh  instance of the UserAccount per request, avoiding race condition.




// wrong : _sheet will not be shared between the instances
//builder.Services.AddScoped<IOmniReader, ReadExcelWriteDatabase>();
//builder.Services.AddScoped<IOmniWriter, ReadExcelWriteDatabase>();

builder.Services.AddScoped<ReadExcelWriteDatabase>();
builder.Services.AddScoped<IOmniReaderWriter>(sp =>sp.GetRequiredService<ReadExcelWriteDatabase>());
//builder.Services.AddScoped<IOmniReader>(sp =>sp.GetRequiredService<ReadExcelWriteDatabase>());
//builder.Services.AddScoped<IOmniWriter>(sp =>sp.GetRequiredService<ReadExcelWriteDatabase>());

builder.Services.AddScoped<IAdmin, AdminPanel>();

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
    IMapper mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    UserAccount userAccount = new UserAccount(userStore, userManager,signInManager, roleManager,context,mapper);
    CustomRoleManager customeRoleManager = new CustomRoleManager(userAccount,userManager, scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>() ); 
     Task<bool> success= customeRoleManager.CreateRoles();
    if (await success)
    {
       
        await customeRoleManager.AddRoleToUser("abd@gmail.com", "Admin", "abdulrahman","2811998@Ma@7799NeonShadowX1ADMINTlaonAniviaZedLeagueOfLegends");
    }
       
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();


app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfiler();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

app.Run();



