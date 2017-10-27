using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private IDataService _dataService;

        public PostsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}", Name = nameof(GetPost))]
        public IActionResult GetPost(int id)
        {
            var post = _dataService.GetPost(id);
            return post != null?
                (IActionResult) Ok(post):NotFound();
        }

        [HttpGet("title/{name}", Name = nameof(GetPostsByName))]
        public IActionResult GetPostsByName(string name, int page=0, int pageSize=5)
        {
            var posts = _dataService.GetPostsByName(name, page, pageSize, out var totalResults);

            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                                ? Url.Link(nameof(GetPostsByName), new { page=page-1, pageSize })
                                : null,
                nextPage = (page+1)*pageSize < totalResults
                                ? Url.Link(nameof(GetPostsByName), new { page=page+1, pageSize })
                                : null,
                posts = posts.Select(post => new
                {
                    Title = post.title,
                    Score = post.score,
                    Url   = Url.Link(nameof(GetPost), new {post.Id})
                })
            };

            return Ok(result);
        }

    }
}
