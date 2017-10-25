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
        /*
        [HttpGet("{id}")]
        public IActionResult GetCategoryByID(int id)
        {
            var cat = _dataService.GetCategory(id);

            if (cat != null)
            {
                return Ok(cat);
            }

            

            return NotFound(new Category());
        }

        [HttpGet()]
        public IActionResult GetCategories()
        {
            var cat = _dataService.GetCategories();

            if (cat.Any())
            {
                return Ok(cat);
            }

            return NotFound(cat);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody]Category body) //Vi smider inputtet i en klasse da den kun kan modtage et input
        {
            var cat = _dataService.CreateCategory(body.Name, body.Description);
            var debug = new { Name = body.Name, body.Description };
            return Created("api/categories/" + cat.Id, cat);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody]Category body)
        {
            if (_dataService.UpdateCategory(id, body.Name, body.Description))
            {
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            if (_dataService.DeleteCategory(id))
            {
                return Ok();
            }
            return NotFound();
        }
        */
    }
}
