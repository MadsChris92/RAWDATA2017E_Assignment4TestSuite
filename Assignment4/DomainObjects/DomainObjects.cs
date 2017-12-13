using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.DomainObjects
{


    public class MarkedPost
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }

    /// <summary>
    /// 
    /// http://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
    /// </summary>
    public class QuestionTag
    {
        public virtual int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }

    public class SearchQuestion
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public int AnswerCount { get; set; }
        public string OwnerName { get; set; }
        [NotMapped]
        public List<Tag> Tags { get; set; }

        [NotMapped] public String Url;
    }

    public class RankedSearchQuestion
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public int AnswerCount { get; set; }
        public DateTime Created { get; set; }
        public double Ranking { get; set; }
        public string OwnerName { get; set; }
        [NotMapped]
        public List<Tag> Tags { get; set; }

        [NotMapped] public String Url;
    }

    public class RankedWord
    {
        public int Ranking { get; set; }
        public string Word { get; set; }
    }


    public class RankedWordPair
    {
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public int Occurences { get; set; }
    }

    public class Answer : Post
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public class Tag
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<QuestionTag> Questions { get; set; }
    }

    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public int? Age { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public int Score { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public int ParentId { get; set; }
        public virtual Post Parent { get; set; }
    }

    public class History
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }

    public abstract class Post
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public DateTime Created { get; set; }

        public virtual IList<Comment> Comments { set; get; }
    }

    public class Question : Post
    {
        public string Title { get; set; }
        public DateTime? Closed { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        [NotMapped] public virtual ICollection<Tag> Tags { get; set; }
    }
}