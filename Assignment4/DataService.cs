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
        public List<Post> GetPostsByName(string name, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                //var Posts = db.Posts.FromSql("CALL wordSearch({0})", name).Skip(page * pageSize).Take<Post>(pageSize).ToList<Post>();

                var result = db.Posts
                    .Where(post => post
                        .title.ToLower()
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
        public Post GetPost(int id)
        {
            using (var db = new SovaContext())
            {
                var post = db.Posts.Include(p => p.Comments).FirstOrDefault(x => x.Id == id);
                return post;
            }
        }

        public Question GetQuestion(int id)
        {
            using (var db = new SovaContext())
            {

                var fukboi = db.Posts.Include(p => p.Comments).Include(p => ((Question) p).Answers);
                var post = (Question) fukboi.FirstOrDefault(x => x.Id == id);
                return post;
            }

        }
    }

    public class Post
    {
        public int Id { get; set; }
        public int owner_id { get; set; }
        public int post_type_id { get; set; }
        public int? parent_id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int score { get; set; }
        public DateTime? closed_date { get; set; }
        public DateTime create_date { get; set; }

        public virtual IList<Comment> Comments { set; get; }
        
    }

    public class Question : Post
    {
        public virtual IList<Answer> Answers { get; set; }
        
    }

    public class Answer : Post
    {
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
