using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace ConsoleApp.EF.Tests
{
    public class MyContextShould
    {
        [Test]
        public void Test1()
        {
            using var myContext = DbHelper.CreateMyContext();
            Check.That(myContext.OrderDetails).IsEmpty();
        }

        [Test]
        public async Task AddOrder()
        {

        }
    }
}