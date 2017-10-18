using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(
                "server=localhost;database=northwind;uid=root;pwd=frans");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Categories
            modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("CategoryName");
            modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("CategoryId");

            //Products
            modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("ProductName");
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("ProductId");
            modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("QuantityUnit");

            //Orders
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("OrderId");
            modelBuilder.Entity<Order>().Property(x => x.Date).HasColumnName("OrderDate");
            modelBuilder.Entity<Order>().Property(x => x.Required).HasColumnName("RequiredDate");



        }
    }
}