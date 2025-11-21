namespace JollyApp.Models
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Refused,
        Maybe
    }

    public class QueueRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Song Song { get; set; } = new Song();
        public string RequestedBy { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public string? Notes { get; set; }
    }
}
