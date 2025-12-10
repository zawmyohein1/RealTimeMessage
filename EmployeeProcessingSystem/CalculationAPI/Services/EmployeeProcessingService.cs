using CalculationAPI.Options;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalculationAPI.Services;

/// <summary>
/// Handles asynchronous employee processing and emits real-time status updates via SignalR.
/// </summary>
public class EmployeeProcessingService
{
    private readonly HubConnection _hubConnection;
    private readonly string _hubUrl;
    private readonly ILogger<EmployeeProcessingService> _logger;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public EmployeeProcessingService(HubConnection hubConnection, IOptions<SignalRSettings> signalROptions, ILogger<EmployeeProcessingService> logger)
    {
        _hubConnection = hubConnection;
        _hubUrl = signalROptions.Value.HubUrl;
        _logger = logger;
    }

    /// <summary>
    /// Processes employees sequentially and sends live updates to connected clients.
    /// </summary>
    public async Task ProcessEmployeesAsync(CancellationToken cancellationToken)
    {
        await EnsureConnectedAsync(cancellationToken);

        for (var employeeId = 1; employeeId <= 100; employeeId++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _hubConnection.InvokeAsync("ForwardStatusUpdate", employeeId, "processing", cancellationToken);
            _logger.LogInformation("Processing employee {EmployeeId}", employeeId);

            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);

            await _hubConnection.InvokeAsync("ForwardStatusUpdate", employeeId, "done", cancellationToken);
            _logger.LogInformation("Employee {EmployeeId} processed", employeeId);
        }
    }

    private async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            return;
        }

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync(cancellationToken);
                _logger.LogInformation("Connected to employee status hub at {HubUrl}", _hubUrl);
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }
}
