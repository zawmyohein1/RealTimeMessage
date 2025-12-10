using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalRService.Hubs;

/// <summary>
/// SignalR hub responsible for streaming employee processing updates to all connected clients.
/// </summary>
public class EmployeeStatusHub : Hub
{
    private readonly ILogger<EmployeeStatusHub> _logger;

    public EmployeeStatusHub(ILogger<EmployeeStatusHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Allows trusted services to fan out status updates to connected clients.
    /// </summary>
    public async Task ForwardStatusUpdate(int employeeId, string status)
    {
        var cancellationToken = Context?.ConnectionAborted ?? CancellationToken.None;

        try
        {
            await Clients.All.SendAsync("ReceiveStatusUpdate", employeeId, status, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to forward status update for employee {EmployeeId}", employeeId);
            throw;
        }
    }
}
