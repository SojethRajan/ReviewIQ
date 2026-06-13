using System.Text.Json.Serialization;

namespace ReviewIQ.AI.AIProvider.Models;

public class GeminiReviewResponse
{
    [JsonPropertyName("qualityScore")]
    public int QualityScore { get; set; }

    [JsonPropertyName("comments")]
    public List<GeminiComment> Comments { get; set; } = new();
}