
namespace HardwareStoreNameSpace
{
    using HardwareStore.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Data;


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }


        // Junction tables
        public DbSet<ProductCountry> ProductsCountries { get; set; }
        public DbSet<BrandSupplier> BrandsSuppliers { get; set; }
        public DbSet<ProductBrand> ProductsBrands { get; set; }
        public DbSet<ProductSupplier> ProductsSuppliers { get; set; }
        public DbSet<ProductBin> ProductsBins { get; set; }
        public DbSet<ProductCategory> ProductsCategories { get; set; }
        public DbSet<ProductUnit> ProductsUnits { get; set; }
        public DbSet<ProductManufacturer> ProductsManufacturers { get; set; }




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
            return await base.SaveChangesAsync(); // Note ZZZ: This line caused the following error "Microsoft.Data.SqlClient.SqlException: 'The MERGE statement conflicted with the FOREIGN KEY constraint "FK_SubCategories_Categories_CategoryId". The conflict occurred in database "HardwareDB", table "dbo.Categories", column 'Id'.'"
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
            //Supppliers has PK, NO FK
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id);
            modelBuilder.Entity<Supplier>().Property(s => s.Id).UseIdentityColumn();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.EnglishName).IsUnique();
            modelBuilder.Entity<Supplier>()
                  .HasOne(s => s.Creator)
                .WithMany(u => u.CreatedSuppliers)
                .HasForeignKey(s =>s.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Supplier>()
                  .HasOne(s => s.Updater)
                .WithMany(u => u.UpdatedSuppliers)
                .HasForeignKey(s => s.UpdaterId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Supplier>()
                  .HasOne(s => s.Deleter)
                .WithMany(u => u.DeletedSuppliers)
                .HasForeignKey(s => s.DeleterId)
                .OnDelete(DeleteBehavior.NoAction);

            //Brands has PK, NO FK
            modelBuilder.Entity<Brand>().HasKey(b => b.Id); 
            modelBuilder.Entity<Brand>().Property(b => b.Id).UseIdentityColumn();
            modelBuilder.Entity<Brand>().HasIndex(b => b.EnglishName).IsUnique();
            modelBuilder.Entity<Brand>().Property(b => b.EnglishName).IsRequired();
            modelBuilder.Entity<Brand>()
              .HasOne(b => b.Creator)
            .WithMany(u => u.CreatedBrands)
            .HasForeignKey(b => b.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Brand>()
              .HasOne(b => b.Updater)
            .WithMany(u => u.UpdatedBrands)
            .HasForeignKey(b => b.UpdaterId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Brand>()
              .HasOne(b => b.Deleter)
            .WithMany(u => u.DeletedBrands)
            .HasForeignKey(b => b.DeleterId)
            .OnDelete(DeleteBehavior.NoAction);






            //Categories: 1 PK
            modelBuilder.Entity<Category>().HasKey(c => c.Id); 
            modelBuilder.Entity<Category>().Property(c => c.Id).UseIdentityColumn();
            modelBuilder.Entity<Category>().HasIndex(c => c.EnglishName).IsUnique();
            modelBuilder.Entity<Category>()
                  .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCategories)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Category>()
                  .HasOne(c => c.Updater)
                .WithMany(u => u.UpdatedCategories)
                .HasForeignKey(c => c.UpdaterId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Category>()
                  .HasOne(c => c.Deleter)
                .WithMany(u => u.DeletedCategories)
                .HasForeignKey(c => c.DeleterId)
                .OnDelete(DeleteBehavior.NoAction);
            
            
            //Units: 1 PK
            modelBuilder.Entity<Unit>().HasKey(u => u.Id); 
            modelBuilder.Entity<Unit>().Property(u => u.Id).UseIdentityColumn();
            modelBuilder.Entity<Unit>().HasIndex(u => u.EnglishName).IsUnique();
            modelBuilder.Entity<Unit>()
                  .HasOne(u => u.Creator)
                .WithMany(u => u.CreatedUnits)
                .HasForeignKey(u => u.CreatorId);
            modelBuilder.Entity<Unit>()
                  .HasOne(u => u.Updater)
                .WithMany(u => u.UpdatedUnits)
                .HasForeignKey(u => u.UpdaterId);
            modelBuilder.Entity<Unit>()
                  .HasOne(u => u.Deleter)
                .WithMany(u => u.DeletedUnits)
                .HasForeignKey(u => u.DeleterId);


            ////Products: 1 Identity PK,  and 3 FKs. 
            modelBuilder.Entity<Product>().HasKey(p => p.Id); 
            modelBuilder.Entity<Product>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<Product>()
                  .HasOne(p => p.Creator)
                .WithMany(u => u.CreatedProducts)
                .HasForeignKey(p => p.CreatorId);
            modelBuilder.Entity<Product>()
                  .HasOne(p => p.Updater)
                .WithMany(u => u.UpdatedProducts)
                .HasForeignKey(p => p.UpdaterId);
            modelBuilder.Entity<Product>()
                  .HasOne(p => p.Deleter)
                .WithMany(u => u.DeletedProducts)
                .HasForeignKey(p => p.DeleterId);


            //SubCategories has one Idenitity-PK and  one FK
            modelBuilder.Entity<SubCategory>().HasKey(sc=> sc.Id);
            modelBuilder.Entity<SubCategory>().Property(sc=> sc.Id).UseIdentityColumn();
            modelBuilder.Entity<SubCategory>()
            .HasOne(sc => sc.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(sc =>sc.CategoryId)
            .OnDelete(DeleteBehavior.NoAction); 
            modelBuilder.Entity<SubCategory>().HasIndex(sc =>  new { sc.CategoryId, sc.EnglishName }).IsUnique();
            modelBuilder.Entity<SubCategory>()
                  .HasOne(sc => sc.Creator)
                .WithMany(u => u.CreatedSubCategories)
                .HasForeignKey(sc => sc.CreatorId);
            modelBuilder.Entity<SubCategory>()
                  .HasOne(sc => sc.Updater)
                .WithMany(u => u.UpdatedSubCategories)
                .HasForeignKey(sc => sc.UpdaterId);
            modelBuilder.Entity<SubCategory>()
                  .HasOne(sc => sc.Deleter)
                .WithMany(u => u.DeletedSubCategories)
                .HasForeignKey(sc => sc.DeleterId);

            // Countries has  Identity-PK, NO FK
            modelBuilder.Entity<Country>().HasKey(c => c.Id);
            modelBuilder.Entity<Country>().Property(c => c.Id).UseIdentityColumn();
            modelBuilder.Entity<Country>().HasIndex(c => c.EnglishName).IsUnique();
            modelBuilder.Entity<Country>()
                  .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCountries)
                .HasForeignKey(c => c.CreatorId);
            modelBuilder.Entity<Country>()
                  .HasOne(c => c.Updater)
                .WithMany(u => u.UpdatedCountries)
                .HasForeignKey(c => c.UpdaterId);
            modelBuilder.Entity<Country>()
                  .HasOne(c => c.Deleter)
                .WithMany(u => u.DeletedCountries)
                .HasForeignKey(c => c.DeleterId);

            //Manufacturers: 1 PK, 3 FKs
            modelBuilder.Entity<Manufacturer>().HasKey(m => m.Id);
            modelBuilder.Entity<Manufacturer>().Property(m => m.Id).UseIdentityColumn();
            modelBuilder.Entity<Manufacturer>().HasIndex(m => m.EnglishName).IsUnique();
            modelBuilder.Entity<Manufacturer>()
                  .HasOne(m => m.Creator)
                .WithMany(u => u.CreatedManufacturers)
                .HasForeignKey(m => m.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Manufacturer>()
                  .HasOne(m => m.Updater)
                .WithMany(u => u.UpdatedManufacturers)
                .HasForeignKey(m => m.UpdaterId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Manufacturer>()
                  .HasOne(m => m.Deleter)
                .WithMany(u => u.DeletedManufacturers)
                .HasForeignKey(m => m.DeleterId)
                .OnDelete(DeleteBehavior.NoAction);

            //Bins: 1 PK, 3 FKs
            modelBuilder.Entity<Bin>().HasKey(b => b.Id);
            modelBuilder.Entity<Bin>().Property(b => b.Id).UseIdentityColumn();
            modelBuilder.Entity<Bin>().HasIndex(b => b.EnglishName).IsUnique();
            modelBuilder.Entity<Bin>()
                  .HasOne(b => b.Creator)
                .WithMany(u => u.CreatedBins)
                .HasForeignKey(b => b.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bin>()
                  .HasOne(b => b.Updater)
                .WithMany(u => u.UpdatedBins)
                .HasForeignKey(b => b.UpdaterId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bin>()
                  .HasOne(b => b.Deleter)
                .WithMany(u => u.DeletedBins)
                .HasForeignKey(b => b.DeleterId)
                .OnDelete(DeleteBehavior.NoAction);



            //BrandsSuppliers has one composite PK and  two FKs
            modelBuilder.Entity<BrandSupplier>().HasKey(bs => new { bs.SupplierId, bs.BrandId });
            modelBuilder.Entity<BrandSupplier>()
                .HasOne(bs => bs.Supplier)
                .WithMany(s => s.BrandSuppliers)
                .HasForeignKey(bs => bs.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BrandSupplier>()
                .HasOne(bs => bs.Brand)
                .WithMany(b => b.BrandSuppliers)
                .HasForeignKey(bs => bs.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            // ProductsCountries has one composite primary key and two FKs (configure Fks on the many side only)
            modelBuilder.Entity<ProductCountry>().HasKey(pc => new { pc.ProductId,pc.CountryId });
            modelBuilder.Entity<ProductCountry>()
                .HasOne(pc => pc.Product)
                .WithMany(p=> p.ProductCountries)
                .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductCountry>()
                .HasOne(pc => pc.Country)
                .WithMany(c => c.ProductCountries)
                .HasForeignKey(pc => pc.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductManufacturer>().HasKey(pm => new { pm.ProductId, pm.ManufacturerId });
            modelBuilder.Entity<ProductManufacturer>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductManufacturers)
                .HasForeignKey(pm => pm.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductManufacturer>()
                .HasOne(pm => pm.Manufacturer)
                .WithMany(m => m.ProductManufacturers)
                .HasForeignKey(pm => pm.ManufacturerId)
                .OnDelete(DeleteBehavior.NoAction);



            //ProductSupplier has one composite PK and two FK
            modelBuilder.Entity<ProductSupplier>().HasKey(ps => new { ps.ProductId, ps.SupplierId });
            modelBuilder.Entity<ProductSupplier>()
                .HasOne(p => p.Product)
                .WithMany(ps => ps.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductSupplier>() 
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);// note 1000 : but I'm already doing that

            modelBuilder.Entity<ProductBrand>().HasKey(pb => new { pb.ProductId, pb.BrandId });
            modelBuilder.Entity<ProductBrand>()
                .HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBrands)
                .HasForeignKey(pb => pb.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductBrand>()
                .HasOne(pb => pb.Brand)
                .WithMany(b => b.ProductBrands)
                .HasForeignKey(pb => pb.BrandId)
                .OnDelete(DeleteBehavior.NoAction);


            // ProductsBins has one composite primary key and two FKs
            modelBuilder.Entity<ProductBin>().HasKey(pb => new { pb.ProductId, pb.BinId });
            modelBuilder.Entity<ProductBin>()
                .HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBins)
                .HasForeignKey(pb => pb.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductBin>()
                .HasOne(pb => pb.Bin)
                .WithMany(b => b.ProductBins)
                .HasForeignKey(pb => pb.BinId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductUnit>().
             HasKey(pu => new { pu.ProductId, pu.UnitId });
            modelBuilder.Entity<ProductUnit>()
            .HasOne(pu => pu.Unit)
            .WithMany(u => u.ProductUnits)
            .HasForeignKey(pu => pu.UnitId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductUnit>()
            .HasOne(pu => pu.Product)
            .WithMany(p => p.ProductUnits)
            .HasForeignKey(pu => pu.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

            //ProductsCategories : 1 composite PK, 2 FKs
            modelBuilder.Entity<ProductCategory>().
             HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
           
           


        }
    }
}