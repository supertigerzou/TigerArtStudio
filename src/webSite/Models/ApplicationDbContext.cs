using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using webSite.Models;

namespace webSite.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<MorningNightSharing> MorningNightSharings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Make Blog.Url required
            builder.Entity<Blog>()
                .Property(b => b.Url)
                .IsRequired();

            builder.Entity<MorningNightSharing>()
                .Property(m => m.Author)
                .IsRequired();
        }        

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class MorningNightSharing
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public bool? IsMorning { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string AudioName { get; set; }
    }
}
