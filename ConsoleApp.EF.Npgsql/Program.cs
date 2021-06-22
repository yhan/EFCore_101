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
            //NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Info, printLevel: true, printConnectorId: true);
                
            Seed();

            //DealWithEfCache();

            await LazyLoading();
        }

        private static async Task LazyLoading()
        {
            // // avoid lazy load
            // await using var context = new MyContext();
            // var query = from p in context.Posts.Include(post => post.Blog)
            //     select new {PostTitle = p.Title, BlogName = p.Blog.Name};
            // _ = await query.ToListAsync();
            
            
            // lazy load
            await using var context2 = new MyContext();
            
            // SELECT p.PostId, p.BlogId, p.Content, p.Title, p.AuthorName_First, p.AuthorName_Last
            // FROM Posts AS p
            foreach (var post in context2.Posts.ToList()) // N plus 1 phenomenon, perf killer
            {
                // SELECT b."Id", b."CreatedTimestamp", b."Name"
                // FROM "Blogs" AS b
                // WHERE b."Id" = @__p_0

                var blogName = post.Blog.Name;
            }
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

                var post = seedContext.Posts.FirstOrDefault(post => post.Title == "Hello Post");
                if (post == null)
                {
                    seedContext.Posts.Add(new Post
                    {
                        AuthorName = new Name("Joe", "Biden"),
                        Blog = testBlog,
                        Content = "bla bla ...",
                        Title = "Dummy post"
                    });
                }

                seedContext.SaveChanges();
            }

            #endregion
        }
    }
}
