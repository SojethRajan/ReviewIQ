namespace ReviewIQ.Shared.Messages
{
    public class ReviewCompletedMessage
    {
        public Guid CodeReviewId { get; init; }
        public Guid RepositoryId { get; init; }
        public Guid IncomingEventId { get; init; }
        public string RepositoryOwner { get; init; } = string.Empty;
        public string RepositoryName { get; init; } = string.Empty;
        public int PullRequestNumber { get; init; }
        public string PrAuthorLogin { get; init; } = string.Empty;
        public int QualityScore { get; init; }
        public int TotalComments { get; init; }
        public int CriticalCount { get; init; }
        public int WarningCount { get; init; }
        public int SuggestionCount { get; init; }
        public DateTime CompletedOn { get; init; }
    }
}
