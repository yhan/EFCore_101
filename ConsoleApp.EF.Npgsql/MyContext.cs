using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF.Npgsql
{
    public class MyContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        private const string DbName = "StoreDB";
        private const string Schema = "public";
        
        private static readonly string ConnectionStringNpgsql = $"Host=localhost;Port=5433;Database={DbName};Username=postgres;Password=root";
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionStringNpgsql);
        }
    }
}