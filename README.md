# RealTimeMessage

## Employee service configuration

The client app expects a single backend host that exposes both the employee processing API and the SignalR hub. Set `EmployeeService:BaseUrl` to the root of that backend (for example, `https://employee-service.example.com`). The following endpoints are derived from it:

- `POST {BaseUrl}/api/employee/process` – starts the processing workflow.
- `{BaseUrl}/employeeStatusHub` – SignalR hub used by the dashboard for real-time updates.

Configure the base URL via `EmployeeProcessingSystem/ClientApp/appsettings.json` or the `EmployeeService__BaseUrl` environment variable when deploying.
