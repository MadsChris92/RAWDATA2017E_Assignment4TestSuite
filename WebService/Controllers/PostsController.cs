using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.DomainObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebService.JSONObjects;

namespace WebService.Controllers
{

    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IDataService _dataService;

        public PostsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}", Name = nameof(GetPost))]
        public IActionResult GetPost(int id)
        {
            var que = _dataService.GetQuestionAllData(id);
            if (que == null) return NotFound();
            var marked = _dataService.IsPostMarked(id);
            var quest = new JSONObjects.Question
            {
                Url = Url.Link(nameof(GetPost), que.Id),
                Body = que.Body,
                Owner = que.Owner,
                Created = que.Created,
                Score = que.Score,
                Title = que.Title,
                Closed = que.Closed,
                Marked = marked,
                NotesUrl = Url.Link(nameof(GetNotes), new {pid=que.Id}),
                Id = que.Id,
                Answers = que.Answers.Select(answer => new JSONObjects.Answer
                {
                    Url = Url.Link(nameof(GetPost), answer.Id),
                    Body = answer.Body,
                    Owner = answer.Owner,
                    Created = answer.Created,
                    Score = answer.Score,
                    Comments = answer.Comments.Select(comment => new JSONObjects.Comment
                    {
                        Created = comment.Created,
                        Owner = comment.Owner,
                        Score = comment.Score,
                        Text = comment.Text
                    }).ToList()
                }).ToList(),

                Tags = que.Tags.Select<Tag, string>(tag => tag.Title).ToList(),
                Comments = que.Comments.Select(comment => new JSONObjects.Comment
                {
                    Created = comment.Created,
                    Owner = comment.Owner,
                    Score = comment.Score,
                    Text = comment.Text
                }).ToList()
            };
            return Ok(quest);
        }

        [HttpGet("words/{name}", Name = nameof(GetPostsWords))]
        public IActionResult GetPostsWords(string name, int page = 0, int pageSize = 5, bool firstPage = false)
        {

            if (firstPage) _dataService.AddHistory(name);

            var posts = _dataService.GetWeightedWordList(name, page, pageSize, out var totalResults);

            var result = new PaginatedResult<RankedWord>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetPostsWords), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetPostsWords), new { page = page + 1, pageSize })
                    : null,
                Results = posts
            };

            return Ok(result);
        }

        [HttpGet("title/{name}", Name = nameof(GetRankedPostsByName))]
        public IActionResult GetRankedPostsByName(string name, int page = 0, int pageSize = 5, bool firstPage = false)
        {

            if (firstPage) _dataService.AddHistory(name);

            var posts = _dataService.GetRankedQuestionByTitle(name, page, pageSize, out var totalResults);
            posts.ForEach(post => post.Url = Url.Link(nameof(GetPost), new { id = post.Id }));

            var result = new PaginatedResult<RankedSearchQuestion>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetRankedPostsByName), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetRankedPostsByName), new { page = page + 1, pageSize })
                    : null,
                Results = posts
            };

            return Ok(result);
        }


        [HttpGet("title/score", Name = nameof(GetRankedPostsByScore))]
        public IActionResult GetRankedPostsByScore(int page = 0, int pageSize = 5, bool firstPage = false)
        {

            var posts = _dataService.SearchPostsOrderedByScore("java", page, pageSize, out var totalResults);
            posts.ForEach(post => post.Url = Url.Link(nameof(GetPost), new { id = post.Id }));

            var result = new PaginatedResult<RankedSearchQuestion>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetRankedPostsByName), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetRankedPostsByName), new { page = page + 1, pageSize })
                    : null,
                Results = posts
            };

            return Ok(result);
        }

        [HttpGet("tag/{name}", Name = nameof(GetPostsByTag))]
        public IActionResult GetPostsByTag(string name, int page = 0, int pageSize = 5, bool firstPage=false)
        {

            if (firstPage) _dataService.AddHistory(name);

            var posts = _dataService.GetQuestionByTag(name, page, pageSize, out var totalResults);
            posts.ForEach(post => post.Url = Url.Link(nameof(GetPost), new { id = post.Id }));

            var result = new PaginatedResult<RankedSearchQuestion>
            {
                TotalResults = totalResults, ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetPostsByTag), new {page = page - 1, pageSize})
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetPostsByTag), new {page = page + 1, pageSize})
                    : null,
                Results = posts
            };

            return Ok(result);
        }

        [HttpGet("mark/{id}", Name = nameof(MarkPost))]
        public IActionResult MarkPost(int id)
        {

            var result= new
            {
                status = _dataService.MarkPost(id),

            };


            return result.status != false ?
                (IActionResult)Ok(result) : NotFound();
        }

        [HttpDelete("mark/{id}", Name = nameof(UnmarkPost))]
        public IActionResult UnmarkPost(int id)
        {

            var result = _dataService.UnmarkPost(id);


            return result != false ?
                (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("mark", Name = nameof(GetMarkedPosts))]
        public IActionResult GetMarkedPosts(int page = 0, int pageSize = 5)
        {
            var posts = _dataService.GetMarkedPosts(page, pageSize, out var totalResults);

            List<RankedSearchQuestion> questions = posts.Select(que => new RankedSearchQuestion
            {
                Url = Url.Link(nameof(GetPost), new { que.Post.Id}),
                Title = que.Post.Title,
                OwnerName = que.Post.Owner.Name,
                AnswerCount = que.Post.Answers.Count,
                Created = que.Post.Created,
                Score = que.Post.Score,
                Tags = que.Post.Tags.ToList()
            }).ToList();

            var result = new
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetMarkedPosts), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetMarkedPosts), new { page = page + 1, pageSize })
                    : null,
                    Results = questions
            };


            return Ok(result);
        }

        [HttpPost("{id}/note", Name = nameof(CreateNote))]
        public IActionResult CreateNote(int id, [FromBody] TextGetter note)
        {
            var resultNote = _dataService.CreateNote(id, note.Text);

            if (resultNote == null) return NotFound();

            var result = new
            {
                note.Text,
                PostUrl = Url.Link(nameof(GetPost), new { Id = resultNote.PostId }),
                Url = Url.Link(nameof(UpdateNote), new { pid = resultNote.PostId, id = resultNote.Id })
            };


            return Ok(result);
        }


        public class TextGetter
        {
            public string Text { get; set; }
        }

        [HttpDelete("{pid}/note/{id}", Name = nameof(DeleteNote))]
        public IActionResult DeleteNote(int pid, int id)
        {

            var result = _dataService.DeleteNote(id);


            return result != false ?
                (IActionResult)Ok(result) : NotFound();
        }

        [HttpPut("{pid}/note/{id}", Name = nameof(UpdateNote))]
        public IActionResult UpdateNote(int pid, int id, [FromBody] TextGetter note)
        {

            var result = _dataService.UpdateNote(id, note.Text);


            return result != false ?
                (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("{pid}/notes", Name = nameof(GetNotes))]
        public IActionResult GetNotes(int pid)
        {
            var notes = _dataService.GetNotes(pid);

            var result = notes.Select(note => new
            {
                note.Text,
                PostUrl = Url.Link(nameof(GetPost), new {Id = note.PostId}),
                Url = Url.Link(nameof(UpdateNote), new { pid = note.PostId, id = note.Id})
            });


            return Ok(result);
        }

        [HttpGet("{pid}/notes/paginated")]
        public IActionResult GetNotesPaginated(int pid, int page = 0, int pageSize = 10)
        {
            var notes = _dataService.GetNotes(pid, page, pageSize, out var totalResults);
            var result = new PaginatedResult<Note>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetNotes), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetNotes), new { page = page + 1, pageSize })
                    : null,
                Results = notes
            };

            return Ok(result);
        }

    }
}
