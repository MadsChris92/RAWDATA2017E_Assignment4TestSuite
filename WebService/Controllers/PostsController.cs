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

    }
}
