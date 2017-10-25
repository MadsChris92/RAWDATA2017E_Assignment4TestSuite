﻿using System;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class SovaContext : DbContext
    {
        /*
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        */

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySql("server=192.168.1.4;database=northwind;uid=marinus;pwd=agergaard"); //martinus
            //optionsBuilder.UseMySql("server=localhost;database=northwind;uid=root;pwd=frans"); //mads
            optionsBuilder.UseMySql("server=wt-220.ruc.dk:3306;database=raw10;uid=raw10;pwd=raw10"); //alex
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Comments
            modelBuilder.Entity<Comment>().Property(x => x.Id)
                .HasColumnName("comment_id");

            modelBuilder.Entity<Comment>().Property(x => x.owner_id)
                .HasColumnName("comment_owner_id");

            modelBuilder.Entity<Comment>().Property(x => x.parent_id)
                .HasColumnName("post_parent_id");

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

        protected void FindPostsByName(string name)
        {
            var Posts
           // this.Database.ExecuteSqlCommand("wordSearch @name", name);

        }
    }
   
}