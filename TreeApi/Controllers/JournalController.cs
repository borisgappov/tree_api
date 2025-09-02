using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TreeApi.Models;
using TreeApi.Services;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api.user.journal")]
    [SwaggerTag("Represents journal API")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService _journalService;

        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [HttpPost("getRange")]
        [SwaggerOperation(
            Summary = "Get journal entries with pagination",
            Description = "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.")]
        [SwaggerResponse(200, "Successful response", typeof(MRangeMJournalInfo))]
        public async Task<ActionResult<MRangeMJournalInfo>> GetRange(
            [FromQuery, SwaggerParameter(Required = true)] int skip,
            [FromQuery, SwaggerParameter(Required = true)] int take,
            [FromBody, SwaggerParameter(Required = false)] VJournalFilter? filter)
        {
            var result = await _journalService.GetRangeAsync(skip, take, filter);
            return Ok(result);
        }

        [HttpPost("getSingle")]
        [SwaggerOperation(
            Summary = "Get journal entry by ID",
            Description = "Returns the information about an particular event by ID.")]
        [SwaggerResponse(200, "Successful response", typeof(MJournal))]
        public async Task<ActionResult<MJournal>> GetSingle([FromQuery, SwaggerParameter(Required = true)] long id)
        {
            var journal = await _journalService.GetSingleAsync(id);
            if (journal == null)
            {
                return NotFound($"Journal entry with id {id} not found.");
            }
            return Ok(journal);
        }
    }
}
