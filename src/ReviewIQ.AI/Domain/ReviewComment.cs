namespace ReviewIQ.AI.Domain;

public class ReviewComment
{
    public Guid Id { get; private set; }
    public Guid CodeReviewId { get; private set; }
    public string FilePath { get; private set; } = string.Empty;
    public int LineNumber { get; private set; }
    public Category Category { get; private set; }
    public Severity Severity { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public string? Suggestion { get; private set; }
    public DateTime CreatedOn { get; private set; }

    //constructor is private to enforce use of factory method
    private ReviewComment() { }

    public static ReviewComment Create(
        Guid codeReviewId,
        string filePath,
        int lineNumber,
        Category category,
        Severity severity,
        string comment,
        string? suggestion)
    {
        return new ReviewComment
        {
            Id = Guid.NewGuid(),
            CodeReviewId = codeReviewId,
            FilePath = filePath,
            LineNumber = lineNumber,
            Category = category,
            Severity = severity,
            Comment = comment,
            Suggestion = suggestion,
            CreatedOn = DateTime.UtcNow
        };
    }
}