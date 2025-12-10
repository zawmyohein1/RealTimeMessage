using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRService.Hubs;

namespace CalculationAPI.Services;

/// <summary>
/// Handles asynchronous employee processing and emits real-time status updates via SignalR.
/// </summary>
public class EmployeeProcessingService
{
    private readonly IHubContext<EmployeeStatusHub> _hubContext;
    private readonly ILogger<EmployeeProcessingService> _logger;

    public EmployeeProcessingService(IHubContext<EmployeeStatusHub> hubContext, ILogger<EmployeeProcessingService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Processes employees sequentially and sends live updates to connected clients.
    /// </summary>
    public async Task ProcessEmployeesAsync(CancellationToken cancellationToken)
    {
        for (var employeeId = 1; employeeId <= 100; employeeId++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", employeeId, "processing", cancellationToken);
            _logger.LogInformation("Processing employee {EmployeeId}", employeeId);

            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);

            await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", employeeId, "done", cancellationToken);
            _logger.LogInformation("Employee {EmployeeId} processed", employeeId);
        }
    }
}
