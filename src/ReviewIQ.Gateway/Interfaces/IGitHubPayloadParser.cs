using ReviewIQ.Gateway.Models;

namespace ReviewIQ.Gateway.Interfaces
{
    public interface IGitHubPayloadParser
    {
        GitHubPayload Parse(string rawBody);
    }
}
