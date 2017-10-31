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
                var post = GetQuestionAllData(id);//db.Questions.Include(p => p.Comments).FirstOrDefault(x => x.Id == id);
                
                return post;
            }
        }

        public Question GetQuestionAllData(int id)
        {
            using (var db = new SovaContext())
            {
                var question =  db.Questions.FirstOrDefault(x => x.Id == id);
                question.FillAnswers();
                question.FillComments();
                question.FillTags();

                foreach(Answer answer in question.Answers)
                {
                    answer.FillComments();
                }
                return question;
            }

        }

        public User GetUser(int id)
        {
            using (var db = new SovaContext())
            {
                return db.Users.FirstOrDefault(user => user.Id == id);
            }
        }

        public List<Question> GetPostsByTagId(int tagId)
        {
            throw new NotImplementedException();
        }

        public bool MarkPost(int id)
        {
            throw new NotImplementedException();
        }

        public bool UnmarkPost(int id)
        {
            throw new NotImplementedException();
        }

        public bool AddHistory(string searchWord)
        {
            throw new NotImplementedException();
        }

        public Note GetNote(int postId)
        {
            using (var db = new SovaContext())
            {
                return db.Notes.FirstOrDefault(note => note.PostId == postId);
            }
        }

        public Note CreateNote(int postId, string text)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNote(int noteId)
        {
            throw new NotImplementedException();
        }

        public bool ClearHistory()
        {
            throw new NotImplementedException();
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
        
        public void FillComments()
        {
            using (var db = new SovaContext())
            {
                var comments = db.Comments.Include(c => c.Owner).Where(x => x.ParentId == Id).ToList();
                Comments = comments;
            }
        }
    }

    public class Question : Post
    {
        public string Title { get; set; }
        public DateTime? Closed { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        [NotMapped]public virtual ICollection<Tag> Tags { get; set; }

        public void FillAnswers()
        {
            using (var db = new SovaContext())
            {
                var answers = db.Answers.Where(x => x.QuestionId == Id).ToList();
                Answers = answers;
            }
        }

        public void FillTags()
        {
            using (var db = new SovaContext())
            {
                Tags = db.QuestionTags.Include(qt => qt.Tag).Where(x => x.QuestionId == Id).Select(qt => qt.Tag).ToList();
            }
        }
    }

}
