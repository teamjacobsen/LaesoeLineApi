using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features.Timetable
{
    [Route("[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly DepartureCache _cache;
        private readonly CrawlDeparturesProcessor _crawlDeparturesProcessor;

        public TimetableController(DepartureCache cache, CrawlDeparturesProcessor crawlDeparturesProcessor)
        {
            _cache = cache;
            _crawlDeparturesProcessor = crawlDeparturesProcessor;
        }

        [HttpGet("Crossings/{crossing}/Departures")]
        public async Task<IActionResult> GetDepartures(Crossing crossing, DateTime date, int days = 1)
        {
            if (Request.GetTypedHeaders().CacheControl?.NoCache != true)
            {
                var departures = await _cache.GetDeparturesAsync(crossing, date, days, false, HttpContext.RequestAborted);

                if (departures != null)
                {
                    return Ok(departures);
                }
            }

            await _crawlDeparturesProcessor.SyncDeparturesAsync(crossing, date, days, HttpContext.RequestAborted);

            return Ok(await _cache.GetDeparturesAsync(crossing, date, days, true, HttpContext.RequestAborted));
        }
    }
}
