using JollyApp.Models;

namespace JollyApp.Services
{
    public class SpotifyService : ISpotifyService
    {
        // Mock implementation for demonstration
        // In production, this would integrate with the Spotify Web API
        // using OAuth2 and the official Spotify SDK or HTTP client
        
        private readonly ILogger<SpotifyService> _logger;
        
        // Mock data for demonstration
        private static readonly List<Song> _mockSongs = new()
        {
            new Song
            {
                Id = "1",
                Title = "Shape of You",
                Artist = "Ed Sheeran",
                Album = "÷ (Divide)",
                DurationMs = 233713,
                AlbumArtUrl = "https://via.placeholder.com/300",
                SpotifyUri = "spotify:track:1"
            },
            new Song
            {
                Id = "2",
                Title = "Blinding Lights",
                Artist = "The Weeknd",
                Album = "After Hours",
                DurationMs = 200040,
                AlbumArtUrl = "https://via.placeholder.com/300",
                SpotifyUri = "spotify:track:2"
            },
            new Song
            {
                Id = "3",
                Title = "Levitating",
                Artist = "Dua Lipa",
                Album = "Future Nostalgia",
                DurationMs = 203064,
                AlbumArtUrl = "https://via.placeholder.com/300",
                SpotifyUri = "spotify:track:3"
            },
            new Song
            {
                Id = "4",
                Title = "Bohemian Rhapsody",
                Artist = "Queen",
                Album = "A Night at the Opera",
                DurationMs = 354320,
                AlbumArtUrl = "https://via.placeholder.com/300",
                SpotifyUri = "spotify:track:4"
            },
            new Song
            {
                Id = "5",
                Title = "Hotel California",
                Artist = "Eagles",
                Album = "Hotel California",
                DurationMs = 391376,
                AlbumArtUrl = "https://via.placeholder.com/300",
                SpotifyUri = "spotify:track:5"
            }
        };

        public SpotifyService(ILogger<SpotifyService> logger)
        {
            _logger = logger;
        }

        public Task<SpotifySearchResult> SearchSongsAsync(string query, int limit = 20)
        {
            _logger.LogInformation($"Searching for songs with query: {query}");
            
            // Mock search - filters songs by query
            var filteredSongs = string.IsNullOrWhiteSpace(query)
                ? _mockSongs
                : _mockSongs.Where(s => 
                    s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.Artist.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.Album.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var result = new SpotifySearchResult
            {
                Songs = filteredSongs.Take(limit).ToList(),
                TotalResults = filteredSongs.Count
            };

            return Task.FromResult(result);
        }

        public Task<Song?> GetSongByIdAsync(string songId)
        {
            _logger.LogInformation($"Getting song by ID: {songId}");
            var song = _mockSongs.FirstOrDefault(s => s.Id == songId);
            return Task.FromResult(song);
        }
    }
}
