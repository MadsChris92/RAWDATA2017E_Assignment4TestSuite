using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            var post = _dataService.GetPost(id);
            if (post != null)
                return Ok(post);
            else
                return NotFound();
        }

        [HttpGet("title/{name}", Name = nameof(GetPostsByName))]
        public IActionResult GetPostsByName(string name, int page=0, int pageSize=5, bool firstPage=false)
        {

            if (firstPage) _dataService.AddHistory(name);

            var posts = _dataService.GetPostsByName(name, page, pageSize, out var totalResults);

            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                                ? Url.Link(nameof(GetPostsByName), new { page = page - 1, pageSize })
                                : null,
                nextPage = (page + 1) * pageSize < totalResults
                                ? Url.Link(nameof(GetPostsByName), new { page = page + 1, pageSize })
                                : null,
                posts
            };

            return Ok(result);
        }

        [HttpGet("tag/{name}", Name = nameof(GetPostsByTag))]
        public IActionResult GetPostsByTag(string name, int page = 0, int pageSize = 5, bool firstPage=false)
        {

            if (firstPage) _dataService.AddHistory(name);

            var posts = _dataService.GetPostsByTagTitle(name, page, pageSize, out var totalResults);

            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                    ? Url.Link(nameof(GetPostsByName), new {page = page - 1, pageSize})
                    : null,
                nextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetPostsByName), new {page = page + 1, pageSize})
                    : null,
                posts
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
        
        [HttpPost("{id}/note", Name = nameof(CreateNote))]
        public IActionResult CreateNote(int id, [FromBody] TextGetter note)
        {

            var result = new
            {
                Note = _dataService.CreateNote(id, note.Text),
                PostUrl = Url.Link(nameof(GetPost), id)
            };


            return result.Note != null ?
                (IActionResult)Ok(result) : NotFound();
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

        [HttpGet("{pid}/note", Name = nameof(GetNotes))]
        public IActionResult GetNotes(int pid, int page=0, int pageSize=10)
        {
            var notes = _dataService.GetNotes(pid, page, pageSize, out var totalResults);
            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                    ? Url.Link(nameof(GetNotes), new { page = page - 1, pageSize })
                    : null,
                nextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetNotes), new { page = page + 1, pageSize })
                    : null,
                notes
            };


            return Ok(result);
        }

    }
}
