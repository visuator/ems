using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[ApiController]
public class StudentController : ControllerBase
{
    public async Task<IActionResult> MarkViaQrCode()
    {
        return Ok();
    }
    public async Task<IActionResult> MarkViaGps()
    {
        return Ok();
    }
    public async Task<IActionResult> GetLastScheduleInfo()
    {
        return Ok();
    }
    public async Task<IActionResult> EliminateDebt()
    {
        return Ok();
    }
}