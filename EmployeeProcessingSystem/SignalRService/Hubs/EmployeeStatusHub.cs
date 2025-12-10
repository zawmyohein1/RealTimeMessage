using Microsoft.AspNetCore.SignalR;

namespace SignalRService.Hubs;

/// <summary>
/// SignalR hub responsible for streaming employee processing updates to all connected clients.
/// </summary>
public class EmployeeStatusHub : Hub
{
}
