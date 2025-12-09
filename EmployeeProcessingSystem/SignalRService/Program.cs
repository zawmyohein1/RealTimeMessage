using SignalRService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure SignalR services and permissive CORS policy for cross-application communication.
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true));
});

var app = builder.Build();

app.UseRouting();
app.UseCors("CorsPolicy");

app.MapGet("/", () => Results.Ok("Employee Status SignalR Service is running."));
app.MapHub<EmployeeStatusHub>("/employeeStatusHub");

app.Run();
