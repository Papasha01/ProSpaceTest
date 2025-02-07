using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Configurations;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess
{
    public class ShopDbContext : DbContext
    {
        public string DbPath { get; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            :base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "shop.db");
        }

        public required DbSet<CustomerEntity> Customer { get; set; }
        public required DbSet<ItemEntity> Item { get; set; }
        public required DbSet<OrderEntity> Order { get; set; }
        public required DbSet<OrderItemEntity> OrderItem { get; set; }
        public required DbSet<UserEntity>  User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
