
using HardwareStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
// add a reference to System.ComponentModel.DataAnnotations DLL
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using System;

namespace HardwareStoreNameSpace
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        // Code was taken from  documentation :https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#use-seeding-method
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        
        .UseSeeding((context, _) =>
        {
            var testBlog = context.Set<ApplicationUser>().FirstOrDefault();
            if (testBlog == null)
            {
                context.Set<ApplicationUser>().Add(new ApplicationUser { Id = "0",RegistrationDate=DateTime.Now, UserName="abd",Email="abdklaib233@gmail.com",PhoneNumber="0785768300",PhoneNumberConfirmed=true,TwoFactorEnabled=true,LockoutEnabled=true});
                context.SaveChanges();
            }
        })
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            var testBlog = await context.Set<ApplicationUser>().FirstOrDefaultAsync(applicationUser => applicationUser.UserName == "TestUserName", cancellationToken);
            if (testBlog == null)
            {
                context.Set<ApplicationUser>().Add(new ApplicationUser { Id = "0", RegistrationDate = DateTime.Now, UserName = "abd", Email = "abdklaib233@gmail.com", PhoneNumber = "0785768300", PhoneNumberConfirmed = true, TwoFactorEnabled = true, LockoutEnabled = true });
                await context.SaveChangesAsync(cancellationToken);
            }
        });

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }


}