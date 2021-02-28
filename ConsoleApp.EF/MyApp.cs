using System;

namespace ConsoleApp.EF
{
    public class MyApp : IDisposable
    {
        private readonly MyContext _myContext;

        public MyApp(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void Run()
        {
            foreach (var detail in _myContext.OrderDetails)
            {
                Console.WriteLine(detail.Order.OrderID);
            }
        }

        public void Dispose()
        {
            _myContext?.Dispose();
        }
    }
}