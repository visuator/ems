namespace EducationManagementSystem.Controllers.Dtos;

public class QrCodeContentDto
{
    public Guid SessionId { get; set; }
    public string? Content { get; set; }
    public bool Final { get; set; }
}