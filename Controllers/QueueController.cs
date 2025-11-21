using Microsoft.AspNetCore.Mvc;
using JollyApp.Services;
using JollyApp.Models;

namespace JollyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly ILogger<QueueController> _logger;

        public QueueController(IQueueService queueService, ILogger<QueueController> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToQueue([FromBody] AddToQueueRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.SongId))
                {
                    return BadRequest("SongId is required");
                }

                if (string.IsNullOrWhiteSpace(request.RequestedBy))
                {
                    return BadRequest("RequestedBy is required");
                }

                var queueRequest = await _queueService.AddToQueueAsync(request.SongId, request.RequestedBy);
                return Ok(queueRequest);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid song ID");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding song to queue");
                return StatusCode(500, "An error occurred while adding the song to queue");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRequests()
        {
            try
            {
                var requests = await _queueService.GetAllRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all requests");
                return StatusCode(500, "An error occurred while retrieving requests");
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var requests = await _queueService.GetPendingRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending requests");
                return StatusCode(500, "An error occurred while retrieving pending requests");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var updatedRequest = await _queueService.UpdateRequestStatusAsync(id, request.Status, request.Notes);
                
                if (updatedRequest == null)
                {
                    return NotFound($"Request with ID {id} not found");
                }

                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating request status");
                return StatusCode(500, "An error occurred while updating the request status");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromQueue(string id)
        {
            try
            {
                var removed = await _queueService.RemoveFromQueueAsync(id);
                
                if (!removed)
                {
                    return NotFound($"Request with ID {id} not found");
                }

                return Ok(new { message = "Request removed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing request from queue");
                return StatusCode(500, "An error occurred while removing the request");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(string id)
        {
            try
            {
                var request = await _queueService.GetRequestByIdAsync(id);
                
                if (request == null)
                {
                    return NotFound($"Request with ID {id} not found");
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting request");
                return StatusCode(500, "An error occurred while retrieving the request");
            }
        }
    }

    public class AddToQueueRequest
    {
        public string SongId { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
    }

    public class UpdateStatusRequest
    {
        public RequestStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
