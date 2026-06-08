namespace ReviewIQ.Shared.Messages
{
    public class PrReviewRequestedMessage
    {
        public Guid IncomingEventId { get; init; }
        public string DeliveryId { get; init; } = string.Empty;
        public string RepositoryOwner { get; init; } = string.Empty;
        public string RepositoryName { get; init; } = string.Empty;
        public int PullRequestNumber { get; init; }
        public string PullRequestTitle { get; init; } = string.Empty;
        public string PrAuthorLogin { get; init; } = string.Empty;
        public string CommitSha { get; init; } = string.Empty;
        public DateTime ReceivedOn { get; init; }
    }
}
