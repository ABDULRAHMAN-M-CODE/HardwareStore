
using HardwareStore.Migrations;
using HardwareStore.Models;
using HardwareStore.ViewModel;
using HardwareStore.ViewModel.AccountViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Contracts;
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

        // Overriding the methods does not force me to generate a migration
        
        // The main problem the following methods solve is the following : I want to automate the process of populating the UpdatedAt Field 
        public override int SaveChanges()
        {
            AddTimestamps();// I Added this functionality, that's why I override the method
            return base.SaveChanges();// Functionality of the Base stays the same
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();// I Added this functionality, that's why I override the method
            return await base.SaveChangesAsync(); // Functionality of the base stays the same.
        }

        /// <summary>
        /// 
        /// This method either populates 'CreatedAt' or 'UpdatedAt' properties or both of them. 
        /// 
        /// if Entity is added, it  populates  CreatedAt and UpdatedAt.
        /// 
        /// if Entity is only Modified but not added, the method will  populate  UpdatedAt   only
        /// 
        /// if Unchanged : neither.
        /// 
        /// if Deleted : neither
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is Timestampable && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((Timestampable)entity.Entity).CreatedAt = now;
                }
                ((Timestampable)entity.Entity).UpdatedAt = now;
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //some relevant documentation : https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
            base.OnModelCreating(modelBuilder);// When overriding OnModelCreating, base.OnModelCreating should be called first;


            //// documentation : https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
            // Supppliers PK, NO FK
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id);
            modelBuilder.Entity<Supplier>().Property(s => s.Id).UseIdentityColumn();

            modelBuilder.Entity<Supplier>().HasIndex(s => s.EnglishName).IsUnique();

            //Brands PK, NO FK
            modelBuilder.Entity<Brand>().HasKey(b => b.Id); 
            modelBuilder.Entity<Brand>().Property(b => b.Id).UseIdentityColumn();


            modelBuilder.Entity<Brand>().HasIndex(b => b.EnglishName).IsUnique();

            modelBuilder.Entity<Brand>().Property(b => b.EnglishName).IsRequired();




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

            modelBuilder.Entity<Category>().HasIndex(c => c.EnglishName).IsUnique();

            //Units PK, NO Fk
            modelBuilder.Entity<Unit>().HasKey(u => u.Id); 
            modelBuilder.Entity<Unit>().Property(u => u.Id).UseIdentityColumn();
 
            modelBuilder.Entity<Unit>().HasIndex(u => u.EnglishName).IsUnique();


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
            modelBuilder.Entity<Product>().HasIndex(p => new { p.UnitId, p.EnglishName }).IsUnique();// my bussiness rules, product must have only one unit, or, product must be unique within the Unit
            modelBuilder.Entity<Product>().HasIndex(p => new { p.CategoryId, p.EnglishName }).IsUnique(); //my bussiness rules,  same logic
            modelBuilder.Entity<Product>().HasIndex(p => new { p.CategoryId, p.EnglishName }).IsUnique();//my bussiness rules, same logic
            modelBuilder.Entity<Product>().HasIndex(p => new { p.SupplierId, p.EnglishName }).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => new { p.BrandId, p.EnglishName }).IsUnique();



            //SubCategories has one Idenitity-PK and  one FK
            modelBuilder.Entity<SubCategory>().HasKey(sc=> sc.Id);
            modelBuilder.Entity<SubCategory>().Property(sc=> sc.Id).UseIdentityColumn();
            modelBuilder.Entity<SubCategory>()
            .HasOne(sc => sc.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(sc =>sc.CategoryId);

            modelBuilder.Entity<SubCategory>().HasIndex(sc =>  new { sc.CategoryId, sc.EnglishName }).IsUnique();


            // Countries has  Identity-PK, NO FK
            modelBuilder.Entity<Country>().HasKey(c => c.Id);
            modelBuilder.Entity<Country>().Property(c => c.Id).UseIdentityColumn();

            modelBuilder.Entity<Country>().HasIndex(c => c.EnglishName).IsUnique();


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