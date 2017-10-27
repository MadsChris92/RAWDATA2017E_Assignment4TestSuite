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
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var data = _dataService.GetPost(321468);
            return Ok(data);
        }

        [HttpGet("user/")]
        public IActionResult GetUser()
        {
            var data = _dataService.GetUser();
            return Ok(data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetProductByID(int id)
        {
            var prod = _dataService.GetPost(id);
            return prod!=null?(IActionResult) Ok(prod):NotFound();
        }
    }
}
