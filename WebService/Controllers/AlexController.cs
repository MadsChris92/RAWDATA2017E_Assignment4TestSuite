using DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    public class AlexController : Controller
    {

        private IDataService _dataService;

        public AlexController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}", Name = nameof(GetPost1))]
        public IActionResult GetPost1(int id)
        {

            var post = _dataService.GetQuestionAllData(id);

            return post != null ?
                (IActionResult)Ok(post) : NotFound();
        }

        [HttpGet("title/{name}", Name = nameof(GetPostsByName1))]
        public IActionResult GetPostsByName1(string name, int page = 0, int pageSize = 5)
        {
            var posts = _dataService.GetPostsByName(name, page, pageSize, out var totalResults);

            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                                ? Url.Link(nameof(GetPostsByName1), new { page = page - 1, pageSize })
                                : null,
                nextPage = (page + 1) * pageSize < totalResults
                                ? Url.Link(nameof(GetPostsByName1), new { page = page + 1, pageSize })
                                : null,
                posts = posts.Select(post => new
                {
                    Title = post.Title,
                    Score = post.Score,
                    Url = Url.Link(nameof(GetPost1), new { post.Id })
                })
            };

            return Ok(result);
        }
        
    }

   

}

