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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
