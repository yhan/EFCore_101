using System.Collections.Generic;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace ConsoleApp.EF.Tests
{
    [TestFixture]
    public class NullEnumerableShould
    {
        [Test]
        public void Iterate_on_null_enumerable()
        {
            IEnumerable<int> enumerable = null;
            var @where = enumerable?.Where(x => x == 1).ToArray();
            Check.That(@where).IsNull();
        }


        public interface IDefaultArgDemo
        {
            string Write(string message = "hello");
        }

        public class DefaultArgDemo : IDefaultArgDemo
        {
            public string Write(string message = "world")
            {
                return message;
            }
        }

        //[Test]
        //public void DefaultValue()
        //{
        //    var demo = new DefaultArgDemo();
        //    demo.Write() // no compile pas
        //}


        [Test]
        public void Default_value2()
        {
            var demo = new DefaultArgDemo();
            var write = demo.Write();

            Check.That(write).IsEqualTo("world");
        }

        [Test]
        public void Default_value_explicit_interface()
        {
            IDefaultArgDemo demo = new DefaultArgDemo();
            var write = demo.Write();

            Check.That(write).IsEqualTo("hello");
        }


        [Test]
        public void Default_value_explicit_interface_no_default_value_in_implementation()
        {
            IDefaultArgDemo demo = new DefaultArgDemo();
            var write = demo.Write();

            Check.That(write).IsEqualTo("hello");
        }

    }
}