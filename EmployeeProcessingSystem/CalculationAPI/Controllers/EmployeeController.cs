using CalculationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeProcessingService _employeeProcessingService;

    public EmployeeController(EmployeeProcessingService employeeProcessingService)
    {
        _employeeProcessingService = employeeProcessingService;
    }

    /// <summary>
    /// Initiates the asynchronous employee processing workflow.
    /// </summary>
    [HttpPost("process")]
    public async Task<IActionResult> ProcessAsync(CancellationToken cancellationToken)
    {
        await _employeeProcessingService.ProcessEmployeesAsync(cancellationToken);
        return Ok(new { message = "Employee processing completed." });
    }
}
