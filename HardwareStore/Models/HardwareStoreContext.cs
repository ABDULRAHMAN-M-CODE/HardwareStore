
using HardwareStore.Models;
using HardwareStore.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
namespace HardwareStoreNameSpace
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<BrandSupplier> BrandsSuppliers { get; set; } // manaully configure foregin keys in SSMS using sql

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //some relevant documentation : https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
            base.OnModelCreating(modelBuilder);// When overriding OnModelCreating, base.OnModelCreating should be called first;
            
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id); // Primary key with identity
            modelBuilder.Entity<Supplier>().Property(s => s.Id).UseIdentityColumn();

            modelBuilder.Entity<Brand>().HasKey(b => b.Id); // Primary key with identity
            modelBuilder.Entity<Brand>().Property(b => b.Id).UseIdentityColumn();

            modelBuilder.Entity<BrandSupplier>().HasKey(bs => new { bs.SupplierId, bs.BrandId });

            modelBuilder.Entity<Category>().HasKey(c => c.Id); // Primary key with identity
            modelBuilder.Entity<Category>().Property(c => c.Id).UseIdentityColumn();


            modelBuilder.Entity<Unit>().HasKey(u => u.Id); // Primary key with identity
            modelBuilder.Entity<Unit>().Property(u => u.Id).UseIdentityColumn();


            // Products table configuration
            modelBuilder.Entity<Product>().HasKey(p => p.Id); // Primary key with identity
            modelBuilder.Entity<Product>().Property(p => p.Id).UseIdentityColumn();

            
            modelBuilder.Entity<Product>()
                .HasOne(p =>p.Unit )
                .WithMany(u=> u.Products)

                
                .HasForeignKey(p => p.UnitId);


            // make another foregin key

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c=> c.Products)
                .HasForeignKey(p => p.CategoryId);



            // remember : makecomposite foreing key inside the  Products table
            // documentation : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
            //Compare documentation approach to my approach that I learned from stack overflow using SQL



        }
    }
}