using System;

namespace ConsoleApp.EF.Npgsql
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public DateTime CreatedTimestamp { get; set; }
    }
    
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public Name AuthorName { get; set; }
    }
    
    public class Name
    {
        public virtual string First { get; set; }
        public virtual string Last { get; set; }
    }
}