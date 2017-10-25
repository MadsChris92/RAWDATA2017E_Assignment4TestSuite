using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace DAL
{
    class SovaContext : DbContext
    {
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySql("server=192.168.1.4;database=northwind;uid=marinus;pwd=agergaard"); //martinus
            //optionsBuilder.UseMySql("server=localhost;database=northwind;uid=root;pwd=frans"); //mads
            optionsBuilder.UseMySql("server=wt-220.ruc.dk;database=raw10;uid=raw10;pwd=raw10"); //alex
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Comment>().Property(x => x.score)
                .HasColumnName("comment_score");

            modelBuilder.Entity<Comment>().Property(x => x.create_date)
                .HasColumnName("comment_create_date");

            modelBuilder.Entity<Comment>().Property(x => x.text)
                .HasColumnName("comment_text");
           

            //Post
            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasColumnName("post_id");


        }
        
        public List<Post> FindPostsByName(string name)
        {
            var posts = this.posts.FromSql("CALL wordSearch({0})", name).Take<Post>(10).ToList<Post>();
            Console.WriteLine(posts);

            return posts;
        }
        
    }
   
}