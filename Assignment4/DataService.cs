using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAL
{
    public class DataService : IDataService
    {
        public List<Question> GetPostsByName(string name, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                //var Posts = db.Posts.FromSql("CALL wordSearch({0})", name).Skip(page * pageSize).Take<Post>(pageSize).ToList<Post>();

                var result = db.Questions
                    .Where(post => post
                        .Title.ToLower()
                        .Contains(name.ToLower()));

                Debug.WriteLine(result);
                totalResults = result.Count();

                var posts = result
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
                return posts;
            }
        }
        public Question GetPost(int id)
        {
            using (var db = new SovaContext())
            {
                var post = GetQuestion(id);//db.Questions.Include(p => p.Comments).FirstOrDefault(x => x.Id == id);
                
                return post;
            }
        }

        public Question GetQuestion(int id)
        {
            using (var db = new SovaContext())
            {

               
                var post =  db.Questions.FirstOrDefault(x => x.Id == id);
                post.FillAnswers(post.Id);
                return post;
            }

        }
    }

    public abstract class Post
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        
        //public int PostTypeId { get; protected set; }
        //public int? parent_id { get; set; }
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

        public void FillAnswers(int id)
        {
            using (var db = new SovaContext())
            {
                var answers = db.Answers.Where(x => x.QuestionId == id).ToList();
                Answers = answers;
            }
        }
        
    }

    public class Answer : Post
    {

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
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
        public int owner_id { get; set; }
        public int score { get; set; }
        public string text { get; set; }
        public DateTime create_date { get; set; }

        public int parent_id { get; set; }
        public virtual Post Parent { get; set; }
    }
}
