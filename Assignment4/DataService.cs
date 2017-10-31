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
        public List<SearchQuestion> GetPostsByName(string name, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {

                var posts = db.SearchQuestions.FromSql("CALL wordSearch({0})", name);
                totalResults = posts.Count();
                
                var returnPosts = posts.Skip(page * pageSize).Take<SearchQuestion>(pageSize).ToList<SearchQuestion>();

                return returnPosts;
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

   

}
