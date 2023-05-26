namespace Forum.Data.Models;

/// <summary>
/// None = 0, Opened, Ignored, Trollstatus, ReporterAbuse, ReportedAbuse
/// </summary>
public enum ReportStatus {
    None,
    Opened,
    Ignored,
    Trollstatus,
    ReporterAbuse,
    ReportedAbuse
}

public class PostReport {
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTime? Created { get; set; }
    public virtual ForumPost? ReportedPost { get; set; }
    public virtual ForumUser? Reporter { get; set; }
    public virtual ForumUser? Reported { get; set; }
    public ReportStatus Status { get; set; }
}
