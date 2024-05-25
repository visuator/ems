using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Domain;

[Owned]
public class Contact
{
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}