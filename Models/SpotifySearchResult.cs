namespace JollyApp.Models
{
    public class SpotifySearchResult
    {
        public List<Song> Songs { get; set; } = new List<Song>();
        public int TotalResults { get; set; }
    }
}
