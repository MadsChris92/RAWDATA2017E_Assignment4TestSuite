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

        [HttpGet("words/related/{terms}", Name = nameof(SearchWordsOccur))]
        public IActionResult SearchWordsOccur([FromRoute] string terms, int page = 0, int pageSize = 50, bool firstPage = false)
        {
            if (firstPage) _dataService.AddHistory(terms);
            var posts = _dataService.GetRelatedWords(terms, page, pageSize, out var totalResults);

            var nodes = posts.SelectMany(post => new List<string>(new[]
            {
                post.Word1.ToLower(),
                post.Word2.ToLower()
            }))
            .GroupBy(word => word)
            .Select((group, index) => new
            {
                Label = group.Key,
                R = 5+Math.Log(group.Count())*3,
                Index = index
            }).ToList();
            var links = posts.Select(post => new
            {
                Source = nodes.First(node => node.Label.Equals(post.Word1.ToLower())).Index,
                Target = nodes.First(node => node.Label.Equals(post.Word2.ToLower())).Index,
                Value = post.Occurences
            });
            

            var result = new 
            {
                TotalResults = totalResults,
                ShowingResults = $"{page}",
                PreviousPage = page > 0
                    ? Url.Link(nameof(SearchPosts), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(SearchPosts), new { page = page + 1, pageSize })
                    : null,
                Results = new
                {
                    Nodes = nodes,
                    Links = links
                }
            };

            return Ok(result);
        }

        [HttpGet("words/ranked/{terms}", Name = nameof(SearchWordsWeighted))]
        public IActionResult SearchWordsWeighted([FromRoute] string terms, int page = 0, int pageSize = 50, bool firstPage = false)
        {
            if (firstPage) _dataService.AddHistory(terms);
            var posts = _dataService.GetWeightedWordList(terms, page, pageSize, out var totalResults);

            var result = new PaginatedResult<RankedWord>
            {
                TotalResults = totalResults,
                ShowingResults = "" + page,
                PreviousPage = page > 0
                    ? Url.Link(nameof(SearchPosts), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(SearchPosts), new { page = page + 1, pageSize })
                    : null,
                Results = posts
            };

            return Ok(result);
        }


        [HttpGet("", Name = nameof(GetPostsByScore))]
        public IActionResult GetPostsByScore(string name, int page = 0, int pageSize = 5, bool firstPage = false)
        {
            var questions = _dataService.GetPostsHighestScore(page, pageSize, out var totalResults);
            List<RankedSearchQuestion> posts;

            if (questions != null)
            {
                posts = questions.Select(que => new RankedSearchQuestion
                {
                    Url = Url.Link(nameof(PostsController.GetPost), new {que.Id, controller="Posts"}),
                    Title = que.Title,
                    OwnerName = que.Owner.Name,
                    AnswerCount = que.Answers.Count,
                    Created = que.Created,
                    Score = que.Score,
                    Tags = que.Tags.ToList()
                }).ToList();
            }
            else
            {
                return NotFound();
            }

            var result = new PaginatedResult<RankedSearchQuestion>
            {
                TotalResults = totalResults,
                ShowingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                PreviousPage = page > 0
                    ? Url.Link(nameof(GetPostsByScore), new { page = page - 1, pageSize })
                    : null,
                NextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetPostsByScore), new { page = page + 1, pageSize })
                    : null,
                Results = posts
            };

            return Ok(result);
        }

    }
}