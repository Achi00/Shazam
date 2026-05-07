using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Interfaces.Service.Song;

namespace Shazam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;
        public SongController(ISongService songService)
        {
            _songService = songService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongResponse>>> GetAll(CancellationToken ct = default)
        {
            var songs = await _songService.GetAllAsync(ct);

            return Ok(songs);
        }

        [HttpPost]
        public async Task<ActionResult<SongResponse>> AddSong(string url, CancellationToken ct = default)
        {
            return await _songService.AddSongAsync(url, ct);
        }
    }
}
