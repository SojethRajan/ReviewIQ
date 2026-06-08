using ReviewIQ.Gateway.Interfaces;
using ReviewIQ.Gateway.Models;
using System.Text.Json;

namespace ReviewIQ.Gateway.Services
{
    public class GitHubPayloadParser : IGitHubPayloadParser
    {
        public GitHubPayload Parse(string rawBody)
        {
            using var doc = JsonDocument.Parse(rawBody);
            var root = doc.RootElement;

            return new GitHubPayload
            {
                Action = root.GetProperty("action").GetString() ?? string.Empty,
                PullRequestNumber = root.GetProperty("number").GetInt32(),
                PullRequestTitle = root
                    .GetProperty("pull_request")
                    .GetProperty("title")
                    .GetString() ?? string.Empty,
                PrAuthorLogin = root
                    .GetProperty("pull_request")
                    .GetProperty("user")
                    .GetProperty("login")
                    .GetString() ?? string.Empty,
                CommitSha = root
                    .GetProperty("pull_request")
                    .GetProperty("head")
                    .GetProperty("sha")
                    .GetString() ?? string.Empty,
                RepositoryOwner = root
                    .GetProperty("repository")
                    .GetProperty("owner")
                    .GetProperty("login")
                    .GetString() ?? string.Empty,
                RepositoryName = root
                    .GetProperty("repository")
                    .GetProperty("name")
                    .GetString() ?? string.Empty,
                RawBody = rawBody
            };
        }
    }
}
