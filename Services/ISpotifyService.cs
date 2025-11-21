using JollyApp.Models;

namespace JollyApp.Services
{
    public interface ISpotifyService
    {
        Task<SpotifySearchResult> SearchSongsAsync(string query, int limit = 20);
        Task<Song?> GetSongByIdAsync(string songId);
    }
}
