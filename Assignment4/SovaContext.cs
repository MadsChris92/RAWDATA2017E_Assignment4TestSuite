using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        public DbSet<Note> Notes { get; set; }


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
            modelBuilder.Entity<Note>().ToTable("note");

            modelBuilder.Entity<Post>().HasMany(post => post.Comments).WithOne(comment => comment.Parent);
            modelBuilder.Entity<Post>().HasOne(post => post.Owner);
            modelBuilder.Entity<Post>().Property(post => post.OwnerId).HasColumnName("owner_id");
            modelBuilder.Entity<Post>().Property(post => post.Created).HasColumnName("create_date");
            //modelBuilder.Entity<Post>().Property(post => post.PostTypeId).HasColumnName("post_type_id");

            modelBuilder.Entity<Question>().HasMany(post => post.Answers).WithOne(ans => ans.Question);
            modelBuilder.Entity<Question>().Property(x => x.Closed).HasColumnName("closed_date");


            modelBuilder.Entity<Comment>().Property(x => x.score)
                .HasColumnName("comment_score");

            modelBuilder.Entity<Post>().HasDiscriminator<int>("post_type_id")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

            modelBuilder.Entity<Comment>().Property(x => x.create_date)
                .HasColumnName("comment_create_date");

            modelBuilder.Entity<Comment>().Property(x => x.Id)
               .HasColumnName("comment_id");
            modelBuilder.Entity<Comment>().Property(x => x.text)
                .HasColumnName("comment_text");
            modelBuilder.Entity<Comment>().Property(x => x.owner_id)
                .HasColumnName("comment_owner_id");
            modelBuilder.Entity<Comment>().Property(x => x.ParentId)
                 .HasColumnName("post_parent_id");


            modelBuilder.Entity<Comment>().HasKey(c => c.Id);

            //Post
            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasColumnName("post_id");

            //modelBuilder.Entity<Answer>().Property(x => x.parent_id)
            //    .HasColumnName("parent_id");
            modelBuilder.Entity<Answer>().Property(x => x.QuestionId)
                .HasColumnName("parent_id");
            //modelBuilder.Entity<Question>().Property(x => x.parent_id)
            //    .HasColumnName("parent_id");

            modelBuilder.Entity<Post>().HasKey(x => x.Id);

        }
    }
   
}