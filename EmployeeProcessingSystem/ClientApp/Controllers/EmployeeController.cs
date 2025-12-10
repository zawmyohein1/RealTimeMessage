using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class EmployeeController : Controller
{
    private readonly IConfiguration _configuration;

    public EmployeeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var baseUrl = _configuration["EmployeeService:BaseUrl"] ?? "http://localhost:5262";
        var processApiUrl = _configuration["EmployeeService:ProcessApiUrl"] ?? $"{baseUrl.TrimEnd('/')}/api/employee/process";

        ViewData["EmployeeServiceBaseUrl"] = baseUrl.TrimEnd('/');
        ViewData["EmployeeServiceProcessApiUrl"] = processApiUrl.TrimEnd('/');

        return View();
    }
}
