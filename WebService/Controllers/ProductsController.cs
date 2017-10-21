using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IDataService _dataService;

        public ProductsController(IDataService dataService)
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
            var prod = _dataService.GetProduct(id);

            if (prod != null)
            {
                var produ = new
                {
                    productName = prod.Name
                };

                return Ok(prod);
            }

            return NotFound(new Product());
        }

        [HttpGet("category/{id}")]
        public IActionResult GetProductByCategory(int id)
        {
            var prod = _dataService.GetProductByCategory(id);

            if (prod.Any())
            {
                return Ok(prod.Select(produ => new {name = produ.Name, categoryName = produ.Category.Name})); // #lortenavn
            }

            return NotFound(prod);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetProductByName(string name)
        {
            var prod = _dataService.GetProductByName(name);

            if (prod.Any())
            { 
                return Ok(prod.Select(produ => new {productName = produ.Name })); // Vi laver et nyt objekt der matcher det forventede input i testen #lortenavn
            }

            return NotFound(prod);
        }

    }
}
