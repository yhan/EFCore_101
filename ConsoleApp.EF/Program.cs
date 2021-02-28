using System;
using System.Collections.Generic;

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
}
