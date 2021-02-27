using System;

namespace ConsoleApp.EF.Npgsql
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MyContext();
            foreach (var blog in context.Blogs)
            {
                
            }
        }
    }
}
