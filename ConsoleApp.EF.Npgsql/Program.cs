using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF.Npgsql
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new MyContext();
            IAsyncEnumerable<Blog> blogs = context.Blogs.AsAsyncEnumerable();

            Console.WriteLine(await blogs.CountAsync());
        }
    }
}
