using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NFluent;
using NUnit.Framework;

namespace ConsoleApp.EF.Tests
{
    [TestFixture]
    public class GroupByShould
    {
        [Test]
        public void Mouadh()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var list = new List<MyModel>
            {
                new MyModel
                {
                    Id = id1,
                    Name = "ti"
                },
                new MyModel
                {
                    Id = id1,
                    Name = "ti2"
                },
                new MyModel
                {
                    Id = id1,
                    Name = "ti3"
                },
                new MyModel
                {
                    Id = id2,
                    Name = "ti2"
                },
                new MyModel
                {
                    Id = id2,
                    Name = "ti3"
                }
            };
            var dd = list.GroupBy(x => x.Id)
                .Select(g => new
                {
                    Id = g.First(),
                    Names = g.Select(a => a.Name).ToList()
                }).ToList();

            var dd2 = list.GroupBy(x => x.Id)
                .Select(g => new
                {
                    Id = g.Key,
                    Names = g.Select(a => a.Name).ToList()
                }).ToList();

            foreach (var d in dd2)
            {
                TestContext.WriteLine(d.Id);
            }

            foreach (var d in dd)
            {
                TestContext.WriteLine(d.Id.Id);
            }
        }

        public class MyModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }


    }

}