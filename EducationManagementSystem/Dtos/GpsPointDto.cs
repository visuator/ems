namespace EducationManagementSystem.Controllers.Dtos;

public class GpsPointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid PersonId { get; set; }
}