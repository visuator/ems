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
public class QrCodeMarkSession : MarkSession
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid? SelectedId { get; set; }
    public int Index { get; set; }
    public bool Completed { get; set; }
    public List<QrCode> QrCodes { get; set; } = default!;
}
public class QrCode : BaseEntity
{
    public string Content { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}
public class GpsPoint : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid PersonId { get; set; }
}