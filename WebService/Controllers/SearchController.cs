using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DomainObjects;
using WebService.JSONObjects;

namespace WebService.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        private readonly IDataService _dataService;

        public SearchController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/Search/posts/java
        [HttpGet("posts/{terms}", Name = nameof(SearchPosts))]
        public IActionResult SearchPosts([FromRoute] string terms, int page = 0, int pageSize = 5, bool firstPage = false, string orderBy = "ranking")
        {
            if (firstPage) _dataService.AddHistory(terms);
            var posts = _dataService.SearchPosts(terms, page, pageSize, orderBy, out var totalResults);
            posts.ForEach(post => post.Url = Url.Link(nameof(PostsController.GetPost), new { id = post.Id }));

            var result = new PaginatedResult<RankedSearchQuestion>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results ordered by "+ orderBy + " " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(SearchPosts), new { page = page - 1, pageSize, orderBy })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(SearchPosts), new { page = page + 1, pageSize, orderBy })
                    : null,
                Results = posts
            };

            return Ok(result);
        }
    }
}