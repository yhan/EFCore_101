using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MyContext();
            foreach (var orderDetail in context.OrderDetails)
            {
                
            }

            Console.WriteLine("Hello World!");
        }
    }


    public class MyContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbName = "StoreDB";
            var schema = "public";
            optionsBuilder.UsePostgreSql($@"User Id=postgres;Password=root;Host=localhost;Database={dbName};Port=5433;Persist Security Info=True;Initial Schema={schema};Unicode=True;License Key=wMnQAqtt0hxxsDqJWvW9KxIj97Gok/73AlbVrLT0DD/uMFfe0qmajvZ7hwGLdgDsU0YXAbKCsQdefn/Tk2w4hP6ayVO14kefgaCTThN0Pserh6XISmNJiiEYrDxw/mlJfIbmJZovZts6ABoyLNOe24T/xWFPBVcL/dA8irfcBzxMXYEdH/IHfWenC2oN4DOh7lPTxS8zLJP7KXn1u85/8B6VoQtWnxcwDcLSKYfXHZGoVidjLYm8LiWYB7hTGU3dgx0AQsbDJN1CVVvXyC9BA/mWxvV+LRnXRYzr5jTW50U=");
        }
    }
}
