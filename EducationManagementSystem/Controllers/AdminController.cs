using Microsoft.AspNetCore.Mvc;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    public async Task<IActionResult> RegisterStudent(IFormFile file, CancellationToken token = default)
    {
        return Ok();
    }
    public async Task<IActionResult> PublishSchedule()
    {
        return Ok();
    }
    public async Task<IActionResult> GetGroupInfo()
    {
        return Ok();
    }
    public async Task<IActionResult> RevokeMark()
    {
        return Ok();
    }
    public async Task<IActionResult> Import()
    {
        return Ok();
    }
}