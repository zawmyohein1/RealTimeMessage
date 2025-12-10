using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class EmployeeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
