using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("schedule")]
public class ScheduleController(ScheduleService scheduleService) : ControllerBase
{
    [Authorize(Roles = "student,lecturer,admin")]
    [HttpGet]
    public async Task<IActionResult> GetLastScheduleInfo(CancellationToken token = default)
    {
        return Ok(await scheduleService.GetLatest(RequestedAt, token));
    }
    private DateTime RequestedAt => HttpContext.Items["requestedAt"] is DateTime dt ? dt : throw new Exception();
}