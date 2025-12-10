# RealTimeMessage

## Employee service configuration

The client app can target distinct hosts for the process API and the SignalR hub:

- Set `EmployeeService:BaseUrl` to the host that serves the SignalR hub (for example, `https://employee-hub.example.com`). The hub endpoint is `{BaseUrl}/employeeStatusHub`.
- Set `EmployeeService:ProcessApiUrl` to the fully qualified URL for the processing API (for example, `https://employee-api.example.com/api/employee/process`).

Both values can be configured in `EmployeeProcessingSystem/ClientApp/appsettings.json` or via environment variables (`EmployeeService__BaseUrl` and `EmployeeService__ProcessApiUrl`).

## Calculation API configuration

The Calculation API now connects to the SignalR hub as a client using the URL in `SignalR:HubUrl` (default: `http://localhost:5262/employeeStatusHub`). Set this to the same hub endpoint your front end uses so `EmployeeProcessingService` can fan out status updates successfully.
