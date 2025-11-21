using JollyApp.Models;

namespace JollyApp.Services
{
    public class QueueService : IQueueService
    {
        private readonly ISpotifyService _spotifyService;
        private readonly ILogger<QueueService> _logger;
        private static readonly List<QueueRequest> _queueRequests = new();

        public QueueService(ISpotifyService spotifyService, ILogger<QueueService> logger)
        {
            _spotifyService = spotifyService;
            _logger = logger;
        }

        public async Task<QueueRequest> AddToQueueAsync(string songId, string requestedBy)
        {
            _logger.LogInformation($"Adding song {songId} to queue, requested by {requestedBy}");
            
            var song = await _spotifyService.GetSongByIdAsync(songId);
            
            if (song == null)
            {
                throw new ArgumentException($"Song with ID {songId} not found");
            }

            var request = new QueueRequest
            {
                Song = song,
                RequestedBy = requestedBy,
                Status = RequestStatus.Pending
            };

            _queueRequests.Add(request);
            _logger.LogInformation($"Song added to queue with request ID: {request.Id}");
            
            return request;
        }

        public Task<List<QueueRequest>> GetAllRequestsAsync()
        {
            _logger.LogInformation("Getting all queue requests");
            return Task.FromResult(_queueRequests.ToList());
        }

        public Task<List<QueueRequest>> GetPendingRequestsAsync()
        {
            _logger.LogInformation("Getting pending queue requests");
            var pending = _queueRequests
                .Where(r => r.Status == RequestStatus.Pending)
                .OrderBy(r => r.RequestedAt)
                .ToList();
            return Task.FromResult(pending);
        }

        public Task<QueueRequest?> UpdateRequestStatusAsync(string requestId, RequestStatus status, string? notes = null)
        {
            _logger.LogInformation($"Updating request {requestId} status to {status}");
            
            var request = _queueRequests.FirstOrDefault(r => r.Id == requestId);
            
            if (request == null)
            {
                _logger.LogWarning($"Request {requestId} not found");
                return Task.FromResult<QueueRequest?>(null);
            }

            request.Status = status;
            if (notes != null)
            {
                request.Notes = notes;
            }

            _logger.LogInformation($"Request {requestId} updated successfully");
            return Task.FromResult<QueueRequest?>(request);
        }

        public Task<bool> RemoveFromQueueAsync(string requestId)
        {
            _logger.LogInformation($"Removing request {requestId} from queue");
            
            var request = _queueRequests.FirstOrDefault(r => r.Id == requestId);
            
            if (request == null)
            {
                _logger.LogWarning($"Request {requestId} not found");
                return Task.FromResult(false);
            }

            _queueRequests.Remove(request);
            _logger.LogInformation($"Request {requestId} removed successfully");
            return Task.FromResult(true);
        }

        public Task<QueueRequest?> GetRequestByIdAsync(string requestId)
        {
            _logger.LogInformation($"Getting request by ID: {requestId}");
            var request = _queueRequests.FirstOrDefault(r => r.Id == requestId);
            return Task.FromResult(request);
        }
    }
}
