using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Interfaces.Service.Match;

namespace Shazam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecognizeController : ControllerBase
    {
        private readonly ISongMatchingService _matchingService;

        public RecognizeController(ISongMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello");
        }

        [HttpPost]
        public async Task<ActionResult<SongResponse>> Recognize([FromBody] Dictionary<string, int> hashes, CancellationToken ct = default)
        {
            var song = await _matchingService.FindMatchAsync(hashes, ct);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }
    }
}
