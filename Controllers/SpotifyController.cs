using Microsoft.AspNetCore.Mvc;
using JollyApp.Services;

namespace JollyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;
        private readonly ILogger<SpotifyController> _logger;

        public SpotifyController(ISpotifyService spotifyService, ILogger<SpotifyController> logger)
        {
            _spotifyService = spotifyService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int limit = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Query parameter is required");
                }

                var result = await _spotifyService.SearchSongsAsync(query, limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching songs");
                return StatusCode(500, "An error occurred while searching for songs");
            }
        }

        [HttpGet("song/{id}")]
        public async Task<IActionResult> GetSong(string id)
        {
            try
            {
                var song = await _spotifyService.GetSongByIdAsync(id);
                
                if (song == null)
                {
                    return NotFound($"Song with ID {id} not found");
                }

                return Ok(song);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting song");
                return StatusCode(500, "An error occurred while retrieving the song");
            }
        }
    }
}
