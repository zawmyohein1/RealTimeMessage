using Microsoft.AspNetCore.SignalR;

namespace SignalRService.Hubs;

/// <summary>
/// SignalR hub responsible for streaming employee processing updates to all connected clients.
/// </summary>
public class EmployeeStatusHub : Hub
{
    /// <summary>
    /// Allows trusted services to fan out status updates to connected clients.
    /// </summary>
    public async Task ForwardStatusUpdate(int employeeId, string status, CancellationToken cancellationToken)
    {
        await Clients.All.SendAsync("ReceiveStatusUpdate", employeeId, status, cancellationToken);
    }
}
