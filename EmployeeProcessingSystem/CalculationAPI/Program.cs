using CalculationAPI.Services;
using CalculationAPI.Options;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
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
builder.Services.Configure<SignalRSettings>(builder.Configuration.GetSection("SignalR"));

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<SignalRSettings>>();
    var hubUrl = string.IsNullOrWhiteSpace(options.Value.HubUrl)
        ? "http://localhost:5262/employeeStatusHub"
        : options.Value.HubUrl;

    return new HubConnectionBuilder()
        .WithUrl(hubUrl)
        .WithAutomaticReconnect()
        .Build();
});

builder.Services.AddSingleton<EmployeeProcessingService>();

var app = builder.Build();

app.UseRouting();
app.UseCors("CorsPolicy");

app.MapGet("/", () => Results.Ok("Calculation API is running."));
app.MapControllers();
app.MapHub<EmployeeStatusHub>("/employeeStatusHub");

app.Run();
