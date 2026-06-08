namespace ReviewIQ.Gateway.Models
{
    public class GitHubPayload
    {
        public string Action { get; init; } = string.Empty;
        public int PullRequestNumber { get; init; }
        public string PullRequestTitle { get; init; } = string.Empty;
        public string PrAuthorLogin { get; init; } = string.Empty;
        public string CommitSha { get; init; } = string.Empty;
        public string RepositoryOwner { get; init; } = string.Empty;
        public string RepositoryName { get; init; } = string.Empty;
        public string RawBody { get; init; } = string.Empty;
        
    }
}
