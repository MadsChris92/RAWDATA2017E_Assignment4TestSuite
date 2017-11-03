using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
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
                aPost = _dataService.GetPost(321468),
                anUser = _dataService.GetUser(1),
                aNote = _dataService.GetNotes(321468)
            };
            return Ok(data);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUser(int id=1)
        {
            var data = _dataService.GetUser(id);
            return Ok(data);
        }

        [HttpGet("question/{id}")]
        public IActionResult GetQuestion(int id)
        {
            var prod = _dataService.GetQuestionAllData(id);
            return prod != null ? (IActionResult)Ok(prod) : NotFound();
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
