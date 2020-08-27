using SobrietyApi.Models;
using SobrietyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace SobrietyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        
        [HttpGet]
        public ActionResult<List<RecordMinimal>> Get() => _leaderboardService.Get();

        [HttpGet("{id:length(24)}", Name = "GetRecord")]
        public ActionResult<Record> Get(string id)
        {
            var record = _leaderboardService.Get(id);

            if (record == null)
            {
                return NotFound();
            }

            return record;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Record> Create([FromBody] RecordMinimal recordMinimal)
        {
            var record = _leaderboardService.Create(recordMinimal);

            return CreatedAtRoute("GetRecord", new { id = record.Id.ToString() }, record);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update(Record recordIn)
        {
            var record = _leaderboardService.Get(recordIn.Id);

            if (record == null)
                return NotFound();

            _leaderboardService.Update(recordIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            var record = _leaderboardService.Get(id);

            if (record == null)
            {
                return NotFound();
            }

            _leaderboardService.Remove(record.Id);

            return NoContent();
        }
    }
}