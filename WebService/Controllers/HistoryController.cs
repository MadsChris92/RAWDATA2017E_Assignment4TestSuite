using DAL.DomainObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {

        private IDataService _dataService;

        public HistoryController(IDataService dataService)
        {
            _dataService = dataService;
        }



        [HttpDelete(Name = nameof(ClearHistory))]
        public IActionResult ClearHistory()
        {

            var result = _dataService.ClearHistory();


            return result != false ?
                (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet(Name = nameof(GetHistory))]
        public IActionResult GetHistory(int page=0, int pageSize=10)
        {

            var history = _dataService.GetHistory(page, pageSize, out var totalResults);

            var result = new
            {
                totalResults,
                showingResults = "Showing results " + (page * pageSize + 1) + "-" + (page + 1) * pageSize + ".",
                previousPage = page > 0
                    ? Url.Link(nameof(GetHistory), new { page = page - 1, pageSize })
                    : null,
                nextPage = (page + 1) * pageSize < totalResults
                    ? Url.Link(nameof(GetHistory), new { page = page + 1, pageSize })
                    : null,
                history
            };
            return Ok(result);
        }

    }

   

}

