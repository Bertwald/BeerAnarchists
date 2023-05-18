namespace Forum.Data.Models;
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
    public virtual ForumUser? Reporter { get; set; }
    public virtual ForumUser? Reported { get; set; }
    public ReportStatus Status { get; set; }
}
