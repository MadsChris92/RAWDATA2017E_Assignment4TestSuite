using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DAL.DomainObjects;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DataService : IDataService
    {
        public List<SearchQuestion> GetQuestionByTitle(string title, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                var returnPosts = db.SearchQuestions.FromSql("CALL bestmatch({0})", title).Paginated(page, pageSize, out totalResults).ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }

        public List<RankedSearchQuestion> GetRankedQuestionByTitle(string title, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                var returnPosts = db.RankedSearchQuestion.FromSql("CALL bestmatch({0})", title).Paginated(page, pageSize, out totalResults).ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }


        public Question GetQuestionAllData(int id)
        {
            using (var db = new SovaContext())
            {
                var question = db.Questions.Include(q => q.Owner)
                    .Include(q => q.Comments).ThenInclude(c => c.Owner)
                    .Include(q => q.Answers).ThenInclude(a => a.Owner)
                    .Include(q => q.Answers).ThenInclude(a => a.Comments).ThenInclude(c => c.Owner)
                    .FirstOrDefault(x => x.Id == id);
                if (question == null) return null;
                question.FillTags();
                return question;
            }

        }

        public Post GetPost(int id)
        {
            using (var db = new SovaContext())
            {
                return db.Answers.Select(ans => (Post)ans).Concat(db.Questions.Select(que => (Post)que))
                    .FirstOrDefault(post => post.Id == id);
            }
        }

        public User GetUser(int id)
        {
            using (var db = new SovaContext())
            {
                return db.Users.FirstOrDefault(user => user.Id == id);
            }
        }

        public List<RankedSearchQuestion> GetQuestionByTag(string tag, int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext())
            {
                var returnPosts = db.RankedSearchQuestion.FromSql("CALL tagSearch({0})", tag).Paginated(page, pageSize, out totalResults).ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }

        public List<Question> GetPostsHighestScore(int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext()) {

                List<Question> returnQuestions = db.Questions
                    .Include(x => x.Answers)
                    .ThenInclude(y => y.Comments)
                    .Include(x => x.Comments)
                    .OrderBy(x => x.Score)
                    .Paginated(page, pageSize, out totalResults)
                    .ToList();
                returnQuestions.ForEach(post => post.FillTags());
                return returnQuestions;

            }     
        }
    

        public Boolean MarkPost(int id)
        {
            using (var db = new SovaContext())
            {

                if (db.Marked.Any(mp => mp.PostId == id)) return true;


                try
                {
                    db.Database.ExecuteSqlCommand("CALL setMarkedPost({0})", id);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public List<MarkedPost> GetMarkedPosts(int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext())
            {
                return db.Marked.Include(mp => mp.Post).Paginated(page, pageSize, out totalResults).ToList();
            }
        }

        public bool UnmarkPost(int id)
        {
            using (var db = new SovaContext())
            {


                var result = db.Database.ExecuteSqlCommand("CALL removeMarkedPost({0})", id);
                Debug.WriteLine(result);
                return result > 0;

            }
        }

        public bool AddHistory(string searchWord)
        {
            if (searchWord == null) return false;
            using (var db = new SovaContext())
            {

                try
                {
                    db.Database.ExecuteSqlCommand("CALL addHistory({0})", searchWord);

                    return true;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }

        public Note GetNote(int noteId)
        {
            using (var db = new SovaContext())
            {
                return db.Notes.FirstOrDefault(note => note.Id == noteId);
            }
        }

        public List<Note> GetNotes(int postId, int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext())
            {
                if (db.Posts.Any(post => post.Id == postId))
                {
                    return db.Notes.Where(note => note.PostId == postId).Paginated(page, pageSize, out totalResults).ToList();
                }
                else
                {
                    totalResults = 0;
                    return null;
                }
            }
        }

        public Note CreateNote(int postId, string text)
        {
            using (var db = new SovaContext())
            {
                try
                {
                    var result = db.Notes.FromSql("CALL createNote({0}, {1})", postId, text).FirstOrDefault();
                    return result;
                }
                catch (MySqlException)
                {

                }

                return null;
            }
        }

        public Boolean DeleteNote(int noteId)
        {
            using (var db = new SovaContext())
            {


                var result = db.Database.ExecuteSqlCommand("CALL removeNote({0})", noteId);
                Debug.WriteLine(result);
                return result > 0;

            }
        }

        public Boolean UpdateNote(int noteId, string text)
        {
            using (var db = new SovaContext())
            {


                var result = db.Database.ExecuteSqlCommand("CALL updateNote({0}, {1})", noteId, text);
                Debug.WriteLine(result);
                return result > 0;

            }
        }

        public bool ClearHistory()
        {
            using (var db = new SovaContext())
            {


                db.Database.ExecuteSqlCommand("CALL clearHistory()");

                return true;

            }
        }

        public List<History> GetHistory(int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext())
            {

                var history = db.History.Paginated(page, pageSize, out totalResults).ToList();

                return history;

            }
        }

        public List<RankedWord> GetWeightedWordList(string name, int page, int pageSize, out int totalResults)
        {
            using (var db = new SovaContext())
            {

                var returnPosts = db.RankedWord.FromSql("CALL weightedWordList({0})", name).Paginated(page, pageSize, out totalResults).ToList();

                return returnPosts;

            }
        }

        public List<RankedSearchQuestion> SearchPostsOrderedByScore(string title, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                var returnPosts = db.RankedSearchQuestion.FromSql("CALL bestmatch({0})", title)
                    .OrderByDescending(q => q.Score)
                    .Paginated(page, pageSize, out totalResults)
                    .ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }

        public List<RankedSearchQuestion> SearchPostsOrderedByNewest(string title, int page, int pageSize, string orderBy, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                var returnPosts = db.RankedSearchQuestion.FromSql("CALL bestmatch({0})", title)
                    .OrderByDescending(q => q.Created)
                    .Paginated(page, pageSize, out totalResults)
                    .ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }
        public List<RankedSearchQuestion> SearchPostsOrderedByRanking(string title, int page, int pageSize, out int totalResults)
        {

            using (var db = new SovaContext())
            {
                var returnPosts = db.RankedSearchQuestion.FromSql("CALL bestmatch({0})", title)
                    .Paginated(page, pageSize, out totalResults)
                    .ToList();
                foreach (var post in returnPosts)
                {
                    post.FillTags();
                }
                return returnPosts;
            }
        }

        public List<RankedSearchQuestion> SearchPosts(string terms, int page, int pageSize, string orderBy, out int totalResults)
        {
            using (var db = new SovaContext())
            {
                var searchResult = db.RankedSearchQuestion.FromSql("CALL bestmatch({0})", terms);
                switch (orderBy)
                {
                    case "age":
                        searchResult = searchResult.OrderByDescending(item => item.Created);
                        break;
                    case "score":
                        searchResult = searchResult.OrderByDescending(item => item.Score);
                        break;
                }
                var currentPage = searchResult.Paginated(page, pageSize, out totalResults).ToList();
                currentPage.ForEach(post => post.FillTags());
                return currentPage;
            }
        }
    }

    static class Util{
        internal static IQueryable<T> Paginated<T>(this IQueryable<T> queryable, int page, int pageSize, out int totalResults)
        {
            page = Math.Max(page, 0);
            pageSize = Math.Clamp(pageSize, 1, 100);
            totalResults = queryable.Count();
            return queryable.Skip(page * pageSize).Take(pageSize);
        }

        internal static void FillTags(this SearchQuestion searchQuestion)
        {
            using (var db = new SovaContext())
            {
                searchQuestion.Tags = db.QuestionTags.Include(qt => qt.Tag).Where(x => x.QuestionId == searchQuestion.Id).Select(qt => qt.Tag).ToList();
            }
        }

        internal static void FillTags(this RankedSearchQuestion searchQuestion)
        {
            using (var db = new SovaContext())
            {
                searchQuestion.Tags = db.QuestionTags.Include(qt => qt.Tag).Where(x => x.QuestionId == searchQuestion.Id).Select(qt => qt.Tag).ToList();
            }
        }

        internal static void FillAnswers(this Question question)
        {
            using (var db = new SovaContext())
            {
                var answers = db.Answers.Where(x => x.QuestionId == question.Id).ToList();
                question.Answers = answers;
            }
        }

        internal static void FillComments(this Post post)
        {
            using (var db = new SovaContext())
            {
                var comments = db.Comments.Include(c => c.Owner).Where(x => x.ParentId == post.Id).ToList();
                post.Comments = comments;
            }
        }

        internal static void FillTags(this Question question)
        {
            using (var db = new SovaContext())
            {
                question.Tags = db.QuestionTags.Include(qt => qt.Tag).Where(x => x.QuestionId == question.Id).Select(qt => qt.Tag).ToList();
            }
        }
    }
}
