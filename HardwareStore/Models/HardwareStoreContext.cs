
using HardwareStore.Models;
using HardwareStore.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Reflection.Metadata;
namespace HardwareStoreNameSpace
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<BrandSupplier> BrandsSuppliers { get; set; } // manaully configure foregin keys in SSMS using sql

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }


        public DbSet<Country> Countries { get; set; }

        public DbSet<ProductCountry> ProductsCountries { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //some relevant documentation : https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
            base.OnModelCreating(modelBuilder);// When overriding OnModelCreating, base.OnModelCreating should be called first;


            //// documentation : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
            // Supppliers PK, NO FK
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id);
            modelBuilder.Entity<Supplier>().Property(s => s.Id).UseIdentityColumn();

            //Brands PK, NO FK
            modelBuilder.Entity<Brand>().HasKey(b => b.Id); 
            modelBuilder.Entity<Brand>().Property(b => b.Id).UseIdentityColumn();




            //BrandsSuppliers has one composite PK and  two FKs
            modelBuilder.Entity<BrandSupplier>().HasKey(bs => new { bs.SupplierId, bs.BrandId });
            modelBuilder.Entity<BrandSupplier>()
                .HasOne(bs => bs.Supplier)
                .WithMany(s=> s.BrandSuppliers)
                .HasForeignKey(bs=>bs.SupplierId);
            modelBuilder.Entity<BrandSupplier>()
                .HasOne(bs => bs.Brand)
                .WithMany(b => b.BrandSuppliers)
                .HasForeignKey(bs => bs.BrandId);


            //Categories PK, No FK
            modelBuilder.Entity<Category>().HasKey(c => c.Id); 
            modelBuilder.Entity<Category>().Property(c => c.Id).UseIdentityColumn();

            //Units PK, NO Fk
            modelBuilder.Entity<Unit>().HasKey(u => u.Id); 
            modelBuilder.Entity<Unit>().Property(u => u.Id).UseIdentityColumn();


            //// Products  has one PK and five (instead of 3) FK, one of them is composite FK.
            modelBuilder.Entity<Product>().HasKey(p => p.Id); // Primary key with identity
            modelBuilder.Entity<Product>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.BrandSupplier)
                .WithMany(bs => bs.Products)
                .HasForeignKey(p => new { p.SupplierId, p.BrandId  });
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.NoAction);



            //SubCategories has one Idenitity-PK and  one FK
            modelBuilder.Entity<SubCategory>().HasKey(sc=> sc.Id);
            modelBuilder.Entity<SubCategory>().Property(sc=> sc.Id).UseIdentityColumn();
            modelBuilder.Entity<SubCategory>()
            .HasOne(sc => sc.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(sc =>sc.CategoryId);


            // Countries has  Identity-PK, NO FK
            modelBuilder.Entity<Country>().HasKey(c => c.Id);
            modelBuilder.Entity<Country>().Property(c => c.Id).UseIdentityColumn();


            // ProductsCountries has one composite primary key and two FKs (configure Fks on the many side only)
            modelBuilder.Entity<ProductCountry>().HasKey(pc => new { pc.ProductId,pc.CountryId });
            modelBuilder.Entity<ProductCountry>()
                .HasOne(pc => pc.Product)
                .WithMany(p=> p.ProductCountries)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCountry>()
                .HasOne(pc => pc.Country)
                .WithMany(c => c.ProductCountries)
                .HasForeignKey(pc => pc.CountryId);

        }
    }
}