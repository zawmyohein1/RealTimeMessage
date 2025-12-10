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
        ViewData["EmployeeServiceBaseUrl"] = baseUrl.TrimEnd('/');

        return View();
    }
}
