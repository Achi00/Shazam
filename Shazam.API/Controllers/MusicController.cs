using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shazam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        [HttpPost]
        public IActionResult StoreMusic()
        {
            return Ok("hello");
        }
    }
}
