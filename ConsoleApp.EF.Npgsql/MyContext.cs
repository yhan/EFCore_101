using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF.Npgsql
{
    public class MyContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        
        public DbSet<Post> Posts { get; set; }

        private const string DbName = "StoreDB2";
        private const string Schema = "public";
        
        private static readonly string ConnectionStringNpgsql = $"Host=localhost;Port=5433;Database={DbName};Username=postgres;Password=root";
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseNpgsql(ConnectionStringNpgsql);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity => { entity.Property(e => e.Name).IsRequired(); });

            // modelBuilder.Entity<Blog>().HasData(new Blog { Id = 1, Name = "Hello world" });   => Fail: seed did nothing

            modelBuilder.Entity<Post>().OwnsOne(post => post.AuthorName);
        }
    }
}