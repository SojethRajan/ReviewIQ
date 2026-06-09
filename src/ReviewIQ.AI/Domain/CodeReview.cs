namespace ReviewIQ.AI.Domain;

public class CodeReview
{
    public Guid Id { get; private set; }
    public Guid RepositoryId { get; private set; }
    public Guid IncomingEventId { get; private set; }
    public int PullRequestNumber { get; private set; }
    public string CommitSha { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Pending";
    public int TotalComments { get; private set; }
    public string? GitHubReviewId { get; private set; }
    public DateTime StartedOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public int QualityScore { get; private set; }

    private readonly List<ReviewComment> _comments = new();
    public IReadOnlyCollection<ReviewComment> Comments => _comments.AsReadOnly();


    //private constructor to enforce use of factory method
    private CodeReview() { }

    public static CodeReview Create(Guid repositoryId, Guid incomingEventId, int pullRequestNumber, string commitSha)
    {
        return new CodeReview
        {
            Id = Guid.NewGuid(),
            RepositoryId = repositoryId,
            IncomingEventId = incomingEventId,
            PullRequestNumber = pullRequestNumber,
            CommitSha = commitSha,
            Status = "Pending",
            StartedOn = DateTime.UtcNow
        };
    }

    public void AddComment( string filePath, int lineNumber,Category category,Severity severity,string comment,string? suggestion)
    {
        var reviewComment = ReviewComment.Create(Id, filePath, lineNumber, category, severity, comment, suggestion);

        _comments.Add(reviewComment);
        TotalComments = _comments.Count;
    }

    public void Complete(QualityScore qualityScore)
    {
        QualityScore = qualityScore.Value;
        Status = "Completed";
        CompletedOn = DateTime.UtcNow;
    }

    public void Fail()
    {
        Status = "Failed";
        CompletedOn = DateTime.UtcNow;
    }

    public void SetGitHubReviewId(string githubReviewId)
    {
        GitHubReviewId = githubReviewId;
    }
}