using JollyApp.Models;

namespace JollyApp.Services
{
    public interface IQueueService
    {
        Task<QueueRequest> AddToQueueAsync(string songId, string requestedBy);
        Task<List<QueueRequest>> GetAllRequestsAsync();
        Task<List<QueueRequest>> GetPendingRequestsAsync();
        Task<QueueRequest?> UpdateRequestStatusAsync(string requestId, RequestStatus status, string? notes = null);
        Task<bool> RemoveFromQueueAsync(string requestId);
        Task<QueueRequest?> GetRequestByIdAsync(string requestId);
    }
}
