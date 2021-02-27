using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new FinexpayDatabaseContext();
           
            Console.WriteLine("Hello World!");
        }
    }


    public class MyContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        private const string DbName = "StoreDB";
        private const string Schema = "public";

        // Min Pool Size=0;Max Pool Size=100;
        private static readonly string ConnectionString = "User ID=postgres;Password=root;Host=localhost;" +
                                                          $"Port=5433;Database={DbName};Initial Schema={Schema};Pooling=true;" +
                                                          "Connection Lifetime=0;";

        private static readonly string ConnectionString2 = $@"User Id=postgres;Password=root;Host=localhost;Database={DbName};Port=5433;Persist Security Info=True;Initial Schema={Schema};Unicode=True;License Key=wMnQAqtt0hxxsDqJWvW9KxIj97Gok/73AlbVrLT0DD/uMFfe0qmajvZ7hwGLdgDsU0YXAbKCsQdefn/Tk2w4hP6ayVO14kefgaCTThN0Pserh6XISmNJiiEYrDxw/mlJfIbmJZovZts6ABoyLNOe24T/xWFPBVcL/dA8irfcBzxMXYEdH/IHfWenC2oN4DOh7lPTxS8zLJP7KXn1u85/8B6VoQtWnxcwDcLSKYfXHZGoVidjLYm8LiWYB7hTGU3dgx0AQsbDJN1CVVvXyC9BA/mWxvV+LRnXRYzr5jTW50U=";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UsePostgreSql(ConnectionString);
        }
    }

    public class FinexpayDatabaseContext : DbContext
    {
        private const string ConnectionString = "User Id=postgres;Password=root;Host=localhost;Database=db_finexpay_test;Port=5433;Persist Security Info=True;Initial Schema=public;Unicode=True;License Key=wMnQAqtt0hxxsDqJWvW9KxIj97Gok/73AlbVrLT0DD/uMFfe0qmajvZ7hwGLdgDsU0YXAbKCsQdefn/Tk2w4hP6ayVO14kefgaCTThN0Pserh6XISmNJiiEYrDxw/mlJfIbmJZovZts6ABoyLNOe24T/xWFPBVcL/dA8irfcBzxMXYEdH/IHfWenC2oN4DOh7lPTxS8zLJP7KXn1u85/8B6VoQtWnxcwDcLSKYfXHZGoVidjLYm8LiWYB7hTGU3dgx0AQsbDJN1CVVvXyC9BA/mWxvV+LRnXRYzr5jTW50U=";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UsePostgreSql(ConnectionString);
        }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
