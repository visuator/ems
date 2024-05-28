using EducationManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Mark> Marks { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Template> Templates { get; set; }
}