using System.Text;
using System.Text.Json;
using ReviewIQ.AI.AIProvider.Interfaces;
using ReviewIQ.AI.AIProvider.Models;

namespace ReviewIQ.AI.AIProvider.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GeminiService> _logger;

    public GeminiService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<GeminiReviewResponse> ReviewAsync(string diffChunk)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        var url = $"v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

        var prompt = BuildPrompt(diffChunk);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Sending diff chunk to Gemini for review");

        var response = await _httpClient.PostAsync(url, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Gemini API call failed. Status: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Gemini API call failed with status {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return ParseGeminiResponse(responseContent);
    }

    private string BuildPrompt(string diffChunk)
    {
        var template = """
            You are an expert code reviewer. Analyse the following GitHub PR diff and return a JSON response only — no markdown, no explanation, just raw JSON.
            The JSON must follow this exact structure:
            {
                "qualityScore": <int 0-100>,
                "comments": [
                    {
                        "filePath": "<file path>",
                        "lineNumber": <int>,
                        "category": "<Security|Bug|Performance|Style>",
                        "severity": "<Critical|Warning|Suggestion>",
                        "comment": "<what the issue is>",
                        "suggestion": "<how to fix it, or null>"
                    }
                ]
            }
            Rules:
            - qualityScore: 100 means perfect code, 0 means critical issues throughout
            - Only include real issues — do not invent problems
            - category must be one of: Security, Bug, Performance, Style
            - severity must be one of: Critical, Warning, Suggestion
            - Return an empty comments array if no issues are found
            - Return raw JSON only — no markdown fences, no explanation

            PR Diff:
            """;

        return template + diffChunk;
    }

    private GeminiReviewResponse ParseGeminiResponse(string responseContent)
    {
        using var doc = JsonDocument.Parse(responseContent);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("Gemini returned an empty response.");

        // Strip markdown code fences if Gemini wraps response despite being told not to
        text = text.Trim();
        if (text.StartsWith("```"))
        {
            text = text.Substring(text.IndexOf('\n') + 1);
            text = text.Substring(0, text.LastIndexOf("```")).Trim();
        }

        return JsonSerializer.Deserialize<GeminiReviewResponse>(text)
            ?? throw new InvalidOperationException("Failed to deserialise Gemini response.");
    }
}