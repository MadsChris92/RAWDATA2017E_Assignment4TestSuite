using DAL.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SovaContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<SearchQuestion> SearchQuestions { get; set; }
        public DbSet<MarkedPost> Marked { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<RankedSearchQuestion> RankedSearchQuestion { get; set; }
        public DbSet<RankedWord> RankedWord { get; set; }
        public DbSet<RankedWordPair> RankedWordPair { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("server=wt-220.ruc.dk;database=raw10;uid=raw10;pwd=raw10");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Comment>().ToTable("comments");
            modelBuilder.Entity<Tag>().ToTable("tags");
            modelBuilder.Entity<Note>().ToTable("notes");
            modelBuilder.Entity<History>().ToTable("history");

            //Post
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasColumnName("id");
            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasMany(post => post.Comments).WithOne(comment => comment.Parent);
            modelBuilder.Entity<Post>().HasOne(post => post.Owner);
            modelBuilder.Entity<Post>().Property(post => post.OwnerId).HasColumnName("author_id");
            modelBuilder.Entity<Post>().Property(post => post.Created).HasColumnName("creation_date");
            modelBuilder.Entity<Post>().HasDiscriminator<int>("post_type")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

            // Question
            modelBuilder.Entity<Question>().HasMany(post => post.Answers).WithOne(ans => ans.Question);
            modelBuilder.Entity<Question>().Property(x => x.Closed).HasColumnName("closed_date");

            // SearchQuestion
            modelBuilder.Entity<SearchQuestion>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<SearchQuestion>().Property(x => x.OwnerName).HasColumnName("ownername");
            modelBuilder.Entity<SearchQuestion>().Property(x => x.AnswerCount).HasColumnName("answers");

            // RankedSearchQuestion
            modelBuilder.Entity<RankedSearchQuestion>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<RankedSearchQuestion>().Property(x => x.OwnerName).HasColumnName("ownername");
            modelBuilder.Entity<RankedSearchQuestion>().Property(x => x.AnswerCount).HasColumnName("answers");

            // Ranked Word
            modelBuilder.Entity<RankedWord>().HasKey(c => c.Word);

            // Ranked Word Pair
            modelBuilder.Entity<RankedWordPair>().HasKey(c => new {c.Word1, c.Word2} );


            // Answer
            modelBuilder.Entity<Answer>().Property(x => x.QuestionId)
                .HasColumnName("parent_id");

            // Comments
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(x => x.Score)
                .HasColumnName("score");
            modelBuilder.Entity<Comment>().Property(x => x.Created)
                .HasColumnName("creation_date");
            modelBuilder.Entity<Comment>().Property(x => x.Id)
               .HasColumnName("id");
            modelBuilder.Entity<Comment>().Property(x => x.Text)
                .HasColumnName("text");
            modelBuilder.Entity<Comment>().Property(x => x.OwnerId)
                .HasColumnName("author_id");
            modelBuilder.Entity<Comment>().Property(x => x.ParentId)
                .HasColumnName("post_id");

            // Note
            modelBuilder.Entity<Note>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Note>().Property(x => x.Text).HasColumnName("text");
            modelBuilder.Entity<Note>().Property(x => x.PostId).HasColumnName("post_id");

            // User
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<User>().Property(x => x.Age).HasColumnName("age");
            modelBuilder.Entity<User>().Property(x => x.Created).HasColumnName("creation_date");
            modelBuilder.Entity<User>().Property(x => x.Location).HasColumnName("location");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("display_name");

            // Tag
            //modelBuilder.Entity<Tag>().Property(tag => tag.Id).HasColumnName("tag_id");
            modelBuilder.Entity<Tag>().Property(tag => tag.Title).HasColumnName("tag");
            modelBuilder.Entity<Tag>().HasKey(tag => tag.Title);

            // QuestionTag
            modelBuilder.Entity<QuestionTag>().ToTable("post_tag");
            modelBuilder.Entity<QuestionTag>().HasKey(qt => new {qt.TagId, qt.QuestionId});
            modelBuilder.Entity<QuestionTag>().Property(qt => qt.TagId).HasColumnName("tag");
            modelBuilder.Entity<QuestionTag>().Property(qt => qt.QuestionId).HasColumnName("post_id");
            modelBuilder.Entity<QuestionTag>().HasOne(qt => qt.Tag).WithMany(t => t.Questions).HasForeignKey(qt => qt.TagId);
            modelBuilder.Entity<QuestionTag>().HasOne(qt => qt.Question).WithMany(q => q.QuestionTags).HasForeignKey(qt => qt.QuestionId);

            // MarkedPost
            modelBuilder.Entity<MarkedPost>().ToTable("marked");
            modelBuilder.Entity<MarkedPost>().HasKey(p => p.PostId);
            modelBuilder.Entity<MarkedPost>().Property(p => p.PostId).HasColumnName("favorited_post_id");
            modelBuilder.Entity<MarkedPost>().HasOne(mp => mp.Post).WithOne().HasForeignKey<MarkedPost>(mp => mp.PostId);

            //History
            modelBuilder.Entity<History>().Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<History>().Property(p => p.Text).HasColumnName("history_text");
            modelBuilder.Entity<History>().Property(p => p.Created).HasColumnName("searched_date");
        }


    }
}