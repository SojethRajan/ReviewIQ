namespace ReviewIQ.AI.Interfaces;

public interface IDiffFetcherService
{
    Task<string> FetchDiffAsync(string owner, string repoName, int pullRequestNumber);
}