using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace DAL
{
    class SovaContext : DbContext
    {
        //public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }


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

            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Comment>().ToTable("comment");
            modelBuilder.Entity<Tag>().ToTable("tag");

            modelBuilder.Entity<Post>().HasMany(post => post.Comments).WithOne(comment => comment.Parent);
            modelBuilder.Entity<Question>().HasMany(post => post.Answers).WithOne(ans => ans.Question);

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(x => x.score)
                .HasColumnName("comment_score");

            modelBuilder.Entity<Post>().HasDiscriminator<int>("post_type_id")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

            //.Map<Question>(m => m.Requires("post_type_id").HasValue(0))
            //.Map<Answer>(m => m.Requires("post_type_id").HasValue(1));

            modelBuilder.Entity<Comment>().Property(x => x.create_date)
                .HasColumnName("comment_create_date");

            modelBuilder.Entity<Comment>().Property(x => x.text)
                .HasColumnName("comment_text");
           

            //Post
            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasColumnName("post_id");

            modelBuilder.Entity<Answer>().Property(x => x.parent_id)
                .HasColumnName("parent_id");
            modelBuilder.Entity<Answer>().Property(x => x.QuestionId)
                .HasColumnName("parent_id");
            modelBuilder.Entity<Question>().Property(x => x.parent_id)
                .HasColumnName("parent_id");

            modelBuilder.Entity<Post>().HasKey(x => x.Id);

        }
    }
   
}