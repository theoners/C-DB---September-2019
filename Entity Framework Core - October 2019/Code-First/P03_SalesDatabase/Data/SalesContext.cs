using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    public class SalesContext: DbContext
    {
        public SalesContext()
        {
            
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                    .HasMany(p => p.Sales)
                    .WithOne(s => s.Product)
                    .HasForeignKey(s => s.ProductId);

                entity
                    .Property(p => p.Description)
                    .HasDefaultValue("No description");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity
                    .Property(s => s.Date)
                    .HasDefaultValueSql("GetDate()");

                entity.HasOne(s => s.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(s => s.ProductId);
            });
        }
    }
}
