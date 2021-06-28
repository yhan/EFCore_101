using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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

            await TwoThreadsTwoDbContextsWithThreadsInParallel();
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
            Console.WriteLine("Save finished");
        }

        /// <summary>
        /// Final solution
        /// </summary>
        private static async Task TwoThreadsTwoDbContextsWithThreadsInParallel()
        {
            using var scope = new TransactionScope(asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled
                /* With this, transaction scope does not have to be disposed on the same thread that it is created*/);
            
            Console.WriteLine($"[debug][create TS]Current thread: {Thread.CurrentThread.ManagedThreadId}");
            var t1 = Task.Run(async () =>
            {
                await using var context = new MyContext();
                var blog1 = context.Blogs.Add(new Blog
                {
                    Name = "blog1",
                    CreatedTimestamp = DateTime.Now
                });
                await context.SaveChangesAsync();
                blog1.State = EntityState.Detached;
            });

            var t2 = Task.Run(async () =>
            {
                await using var context = new MyContext();
                var blog2= context.Blogs.Add(new Blog
                {
                    Name = "blog2",
                    CreatedTimestamp = DateTime.Now
                });
                await context.SaveChangesAsync();
                blog2.State = EntityState.Detached;
                
            });

            await Task.WhenAll(t1, t2);

            Console.WriteLine($"[debug][dispose TS]Current thread: {Environment.CurrentManagedThreadId}");
            scope.Complete();
        }
        
        private static async Task TwoThreadsTwoDbContextsWithThreads_Sequentially()
        {
            // Fixed:
            // System.InvalidOperationException:
            //
            // A TransactionScope must be disposed on the same thread that it was created.

            using var scope = new TransactionScope(asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);
            
            await using var context = new MyContext();
            var blog1 = new Blog
            {
                Name = "blog1",
                CreatedTimestamp = DateTime.Now
            };
            var addedBlog1 = context.Blogs.Add(blog1);
            await context.SaveChangesAsync();
            addedBlog1.State = EntityState.Detached;

            var addedBlog2 = context.Blogs.Add(new Blog
            {
                Name = "blog2",
                CreatedTimestamp = DateTime.Now
            });
            await context.SaveChangesAsync();
            addedBlog2.State = EntityState.Detached;
            
            scope.Complete();
        }


        private static async Task ThreadsCanNotShareDbContext()
        {
            // System.InvalidOperationException:
            //
            // An attempt was made to use the context instance while it is being configured.
            // A DbContext instance cannot be used inside 'OnConfiguring' since it is still being configured at this point.
            // This can happen if a second operation is started on this context instance before a previous operation completed.
            // Any instance members are not guaranteed to be thread safe.
            
            var context = new MyContext();

            var t1 = Task.Run(() =>
            {
                context.Blogs.Add(new Blog
                {
                    Name = "blog1",
                    CreatedTimestamp = DateTime.Now
                });
            });

            var t2 = Task.Run(() =>
            {
                context.Blogs.Add(new Blog
                {
                    Name = "blog2",
                    CreatedTimestamp = DateTime.Now
                });
            });

            await Task.WhenAll(t1, t2);
        }
        
        
        private static async Task ThreadsCanNotShareDbContext_UsingTwoDbContext_WithoutTransaction()
        {
            // System.InvalidOperationException:
            //
            // An attempt was made to use the context instance while it is being configured.
            // A DbContext instance cannot be used inside 'OnConfiguring' since it is still being configured at this point.
            // This can happen if a second operation is started on this context instance before a previous operation completed.
            // Any instance members are not guaranteed to be thread safe.
            
            var t1 = Task.Run(async () =>
            {
                var context = new MyContext();
                context.Blogs.Add(new Blog
                {
                    Name = "blog1",
                    CreatedTimestamp = DateTime.Now
                });
                await context.SaveChangesAsync();
            });

            var t2 = Task.Run(async () =>
            {
                var context = new MyContext();
                context.Blogs.Add(new Blog
                {
                    Name = "blog2",
                    CreatedTimestamp = DateTime.Now
                });
                await context.SaveChangesAsync();
            });

            await Task.WhenAll(t1, t2);
        }
    }
}
