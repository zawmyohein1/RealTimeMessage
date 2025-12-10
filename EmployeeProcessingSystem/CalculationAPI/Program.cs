using CalculationAPI.Services;
using SignalRService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure services for API endpoints, SignalR, and employee processing.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true));
});
builder.Services.AddSingleton<EmployeeProcessingService>();

var app = builder.Build();

app.UseRouting();
app.UseCors("CorsPolicy");

app.MapGet("/", () => Results.Ok("Calculation API is running."));
app.MapControllers();
app.MapHub<EmployeeStatusHub>("/employeeStatusHub");

app.Run();
