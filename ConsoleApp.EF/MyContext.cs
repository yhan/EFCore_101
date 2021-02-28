using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF
{
    public class MyContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        
        
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
    }
}