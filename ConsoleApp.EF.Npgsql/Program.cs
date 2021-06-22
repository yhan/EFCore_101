using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.Logging;

namespace ConsoleApp.EF.Npgsql
{
    class Program
    {
        static async Task Main(string[] args)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace, true, true);
                
            //Seed();

            DealWithEfCache();
        }

        
        private static void DealWithEfCache()
        {
            //https://codethug.com/2016/02/19/Entity-Framework-Cache-Busting/
            PreventEfCachingWithAsNoTracking();

            PreventEfCachingWithDetach();
        }

        private static void PreventEfCachingWithDetach()
        {
            using var context = new MyContext();
            var blog = context.Blogs.AsNoTracking().Single();

            Debug.Assert(blog.Name == "Hello world");
            context.Entry(blog).State = EntityState.Detached;
            
            // do the manual update


            var blogAgain = context.Blogs.Single();
            Debug.Assert(blogAgain.Name == "Updated");

        }

        private static void PreventEfCachingWithAsNoTracking()
        {
            using var context = new MyContext();
            // without AsNoTracking(), ef will cache the .Blogs,
            // afterwards, if we retrieve blogs from the same DbContext, 
            // when blogs are updated outside the program
            // context.Blogs will return the old Blog value
            var blogs = context.Blogs.AsNoTracking().ToList();

            Debug.Assert(blogs.Single().Name == "Hello world");

            // Do the manual udpate

            // select again
            blogs = context.Blogs.ToList();
            Debug.Assert(blogs.Single().Name == "Updated");
        }

        private static void Seed()
        {
            #region CustomSeeding

            using (var seedContext = new MyContext())
            {
                seedContext.Database.EnsureCreated();

                var testBlog = seedContext.Blogs.FirstOrDefault(b => b.Name == "Hello world");
                if (testBlog == null)
                {
                    seedContext.Blogs.Add(new Blog {Name = "Hello world"});
                }

                seedContext.SaveChanges();
            }

            #endregion
        }
    }
}
