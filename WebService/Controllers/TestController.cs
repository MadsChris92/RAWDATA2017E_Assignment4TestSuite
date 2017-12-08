using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.DomainObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IDataService _dataService;

        public TestController(IDataService dataService)
        {
            _dataService = dataService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var data = new
            {
                aQuestion = _dataService.GetPost(5821),
                anAnswer = _dataService.GetPost(5822),
                anUser = _dataService.GetUser(1),
                notes = _dataService.GetNotes(5821, 0, 10, out var _)
            };
            return Ok(data);
        }

        [HttpGet("user/{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id=1)
        {
            var data = _dataService.GetUser(id);
            return Ok(data);
        }

        [HttpGet("question/{id}")]
        public IActionResult GetQuestion(int id)
        {
            var question = _dataService.GetQuestionAllData(id);
            JSONObjects.Question result = null;
            if (question != null)
            {
                result = new JSONObjects.Question()
                {
                    Url = Url.Link(nameof(GetPost), question.Id),
                    Body = question.Body,
                    OwnerUrl = Url.Link(nameof(GetUser), question.OwnerId),
                    Created = question.Created,
                    Score = question.Score,
                    Title = question.Title,
                    Closed = question.Closed,
                    Answers = question.Answers.Select(answer => new JSONObjects.Answer
                    {
                        Url = Url.Link(nameof(GetPost), answer.Id),
                        Body = answer.Body,
                        OwnerUrl = Url.Link(nameof(GetUser), answer.OwnerId),
                        Created = answer.Created,
                        Score = answer.Score,
                        Comments = answer.Comments.Select(comment => new JSONObjects.Comment
                        {
                            Created = comment.Created,
                            OwnerUrl = Url.Link(nameof(GetUser), comment.OwnerId),
                            Score = comment.Score,
                            Text = comment.Text
                        }).ToList()
                    }).ToList(),
                    Tags = question.Tags.Select<Tag, string>(tag => tag.Title).ToList(),
                    Comments = question.Comments.Select(comment => new JSONObjects.Comment
                    {
                        Created = comment.Created,
                        OwnerUrl = Url.Link(nameof(GetUser), comment.OwnerId),
                        Score = comment.Score,
                        Text = comment.Text
                    }).ToList()
                };
            }
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        // GET api/values/5
        [HttpGet("post/{id}")]
        public IActionResult GetPost(int id)
        {
            var prod = _dataService.GetPost(id);
            return prod!=null?(IActionResult) Ok(prod):NotFound();
        }
    }
}
