using ReviewIQ.AI.Interfaces;

namespace ReviewIQ.AI.Services;

public class DiffFetcherService : IDiffFetcherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DiffFetcherService> _logger;

    public DiffFetcherService(HttpClient httpClient, ILogger<DiffFetcherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> FetchDiffAsync(string owner, string repoName, int pullRequestNumber)
    {
        var url = $"repos/{owner}/{repoName}/pulls/{pullRequestNumber}/files";

        _logger.LogInformation("Fetching diff for PR #{PullRequestNumber} from {Owner}/{RepoName}",pullRequestNumber, owner, repoName);

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch diff. Status: {StatusCode}", response.StatusCode);
            return string.Empty;
        }

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}