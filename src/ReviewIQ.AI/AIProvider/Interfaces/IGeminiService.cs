using ReviewIQ.AI.AIProvider.Models;

namespace ReviewIQ.AI.AIProvider.Interfaces;

public interface IGeminiService
{
    Task<GeminiReviewResponse> ReviewAsync(string diffChunk);
}