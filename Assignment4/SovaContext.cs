using DAL.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SovaContext : DbContext
    {
        //public DbSet<Post> Posts { get; set; }
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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("server=wt-220.ruc.dk;database=raw10;uid=raw10;pwd=raw10");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Comment>().ToTable("comment");
            modelBuilder.Entity<Tag>().ToTable("tag");
            modelBuilder.Entity<Note>().ToTable("note");
            modelBuilder.Entity<History>().ToTable("history");

            //Post
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<Post>().Property(x => x.Id)
                .HasColumnName("post_id");
            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasMany(post => post.Comments).WithOne(comment => comment.Parent);
            modelBuilder.Entity<Post>().HasOne(post => post.Owner);
            modelBuilder.Entity<Post>().Property(post => post.OwnerId).HasColumnName("owner_id");
            modelBuilder.Entity<Post>().Property(post => post.Created).HasColumnName("create_date");
            modelBuilder.Entity<Post>().HasDiscriminator<int>("post_type_id")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

            // Question
            modelBuilder.Entity<Question>().HasMany(post => post.Answers).WithOne(ans => ans.Question);
            modelBuilder.Entity<Question>().Property(x => x.Closed).HasColumnName("closed_date");

            // SearchQuestion
  
            modelBuilder.Entity<SearchQuestion>().Property(x => x.Id).HasColumnName("post_id");
            modelBuilder.Entity<SearchQuestion>().Property(x => x.OwnerName).HasColumnName("user_name");
            modelBuilder.Entity<SearchQuestion>().Property(x => x.AnswerCount).HasColumnName("counts");


            // Answer
            modelBuilder.Entity<Answer>().Property(x => x.QuestionId)
                .HasColumnName("parent_id");

            // Comments
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(x => x.Score)
                .HasColumnName("comment_score");
            modelBuilder.Entity<Comment>().Property(x => x.Created)
                .HasColumnName("comment_create_date");
            modelBuilder.Entity<Comment>().Property(x => x.Id)
               .HasColumnName("comment_id");
            modelBuilder.Entity<Comment>().Property(x => x.Text)
                .HasColumnName("comment_text");
            modelBuilder.Entity<Comment>().Property(x => x.OwnerId)
                .HasColumnName("comment_owner_id");
            modelBuilder.Entity<Comment>().Property(x => x.ParentId)
                .HasColumnName("post_parent_id");

            // Note
            modelBuilder.Entity<Note>().Property(x => x.Id).HasColumnName("note_id");
            modelBuilder.Entity<Note>().Property(x => x.Text).HasColumnName("note_text");
            modelBuilder.Entity<Note>().Property(x => x.PostId).HasColumnName("note_post_id");

            // User
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("user_id");
            modelBuilder.Entity<User>().Property(x => x.Age).HasColumnName("user_age");
            modelBuilder.Entity<User>().Property(x => x.Created).HasColumnName("user_create_date");
            modelBuilder.Entity<User>().Property(x => x.Location).HasColumnName("user_location");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("user_name");

            // Tag
            modelBuilder.Entity<Tag>().Property(tag => tag.Id).HasColumnName("tag_id");
            modelBuilder.Entity<Tag>().Property(tag => tag.Title).HasColumnName("tag_title");

            // QuestionTag
            modelBuilder.Entity<QuestionTag>().ToTable("post_tag");
            modelBuilder.Entity<QuestionTag>().HasKey(qt => new {qt.TagId, qt.QuestionId});
            modelBuilder.Entity<QuestionTag>().Property(qt => qt.TagId).HasColumnName("parent_tag_id");
            modelBuilder.Entity<QuestionTag>().Property(qt => qt.QuestionId).HasColumnName("post_parent_tag_id");
            modelBuilder.Entity<QuestionTag>().HasOne(qt => qt.Tag).WithMany(t => t.Questions).HasForeignKey(qt => qt.TagId);
            modelBuilder.Entity<QuestionTag>().HasOne(qt => qt.Question).WithMany(q => q.QuestionTags).HasForeignKey(qt => qt.QuestionId);

            // MarkedPost
            modelBuilder.Entity<MarkedPost>().ToTable("marked");
            modelBuilder.Entity<MarkedPost>().HasKey(p => p.PostId);
            modelBuilder.Entity<MarkedPost>().Property(p => p.PostId).HasColumnName("favorited_post_id");
            modelBuilder.Entity<MarkedPost>().HasOne(mp => mp.Post).WithOne().HasForeignKey<MarkedPost>(mp => mp.PostId);

            //History
            modelBuilder.Entity<History>().Property(p => p.Id).HasColumnName("history_id");
            modelBuilder.Entity<History>().Property(p => p.Text).HasColumnName("history_text");
            modelBuilder.Entity<History>().Property(p => p.Created).HasColumnName("searched_date");
        }


    }
}