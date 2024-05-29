namespace EducationManagementSystem.Domain;

public abstract class MarkSession : BaseEntity
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
public class GpsMarkSession : MarkSession
{
    public Guid SourceId { get; set; }
    public GpsPoint Source { get; set; } = default!;
    public List<GpsPoint> Points { get; set; } = default!;
}
public class QrMarkSession : MarkSession
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public List<Guid> Ids { get; set; } = default!;
    public Guid SelectedId { get; set; }
}
public class GpsPoint : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
}