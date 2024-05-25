using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Domain;

[Owned]
public class Person
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string MiddleName { get; set; } = default!;
}