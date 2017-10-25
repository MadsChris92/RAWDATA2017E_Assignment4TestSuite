using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAL
{
    public class DataService : IDataService
    {
        public List<Post> GetPostsByName(string name)
        {

            using (var db = new SovaContext())
            {
                var posts = db.FindPostsByName(name);

                return posts;
            }
        }
        public Post GePost(int id)
        {
            using (var db = new SovaContext())
            {

                return db.posts.FirstOrDefault(x => x.Id == id);
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
    }

    public class Comment
    {
        public int Id { get; set; }
        public int owner_id { get; set; }
        public int score { get; set; }
        public string text { get; set; }
        public DateTime create_date { get; set; }
        public int parent_id { get; set; }
    }
}
