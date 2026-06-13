using Microsoft.EntityFrameworkCore;
using ReviewIQ.AI.AIProvider.Interfaces;
using ReviewIQ.AI.AIProvider.Models;
using ReviewIQ.AI.Domain;
using ReviewIQ.AI.Infrastructure;
using ReviewIQ.AI.Interfaces;
using ReviewIQ.Shared.Constants;
using ReviewIQ.Shared.Messages;

namespace ReviewIQ.AI.Services;

public class ReviewOrchestrator : IReviewOrchestrator
{
    private readonly IDiffFetcherService _diffFetcherService;
    private readonly IDiffChunker _diffChunker;
    private readonly IGeminiService _geminiService;
    private readonly IReviewPublisher _reviewPublisher;
    private readonly AiDbContext _context;
    private readonly ILogger<ReviewOrchestrator> _logger;

    public ReviewOrchestrator(
        IDiffFetcherService diffFetcherService,
        IDiffChunker diffChunker,
        IGeminiService geminiService,
        IReviewPublisher reviewPublisher,
        AiDbContext context,
        ILogger<ReviewOrchestrator> logger)
    {
        _diffFetcherService = diffFetcherService;
        _diffChunker = diffChunker;
        _geminiService = geminiService;
        _reviewPublisher = reviewPublisher;
        _context = context;
        _logger = logger;
    }

    public async Task<OrchestratorResult> ProcessAsync(PrReviewRequestedMessage message)
    {
        _logger.LogInformation("Processing PR #{PullRequestNumber} for {Owner}/{Repo}",
            message.PullRequestNumber, message.RepositoryOwner, message.RepositoryName);

        // Step 1 — Create CodeReview aggregate
        var codeReview = CodeReview.Create(
            message.RepositoryId,
            message.IncomingEventId,
            message.PullRequestNumber,
            message.CommitSha);

        // Step 2 — Fetch diff
        var diff = await _diffFetcherService.FetchDiffAsync(
            message.RepositoryOwner,
            message.RepositoryName,
            message.PullRequestNumber);

        if (string.IsNullOrWhiteSpace(diff))
        {
            _logger.LogError("Diff fetch failed for PR #{PullRequestNumber}", message.PullRequestNumber);
            codeReview.Fail();
            await SaveReviewAsync(codeReview);
            return OrchestratorResult.DiffFetchFailed;
        }

        // Step 3 — Chunk the diff
        var chunks = _diffChunker.Chunk(diff);

        // Step 4 — Send each chunk to Gemini
        var allComments = new List<GeminiComment>();
        var totalQualityScore = 0;

        try
        {
            foreach (var chunk in chunks)
            {
                var geminiResponse = await _geminiService.ReviewAsync(chunk);
                allComments.AddRange(geminiResponse.Comments);
                totalQualityScore += geminiResponse.QualityScore;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gemini review failed for PR #{PullRequestNumber}", message.PullRequestNumber);
            codeReview.Fail();
            await SaveReviewAsync(codeReview);
            return OrchestratorResult.GeminiFailed;
        }

        // Step 5 — Add comments to aggregate
        foreach (var item in allComments)
        {
            codeReview.AddComment(
                filePath: item.FilePath,
                lineNumber: item.LineNumber,
                category: Enum.Parse<Category>(item.Category),
                severity: Enum.Parse<Severity>(item.Severity),
                comment: item.Comment,
                suggestion: item.Suggestion);
        }

        // Step 6 — Complete the review
        var averageScore = chunks.Count > 0 ? totalQualityScore / chunks.Count : 0;
        codeReview.Complete(new QualityScore(averageScore));

        // Step 7 — Save to database
        var saved = await SaveReviewAsync(codeReview);
        if (!saved)
            return OrchestratorResult.ReviewSaveFailed;

        // Step 8 — Publish ReviewCompletedMessage
        var completedMessage = new ReviewCompletedMessage
        {
            CodeReviewId = codeReview.Id,
            RepositoryId = codeReview.RepositoryId,
            IncomingEventId = codeReview.IncomingEventId,
            RepositoryOwner = message.RepositoryOwner,
            RepositoryName = message.RepositoryName,
            PullRequestNumber = codeReview.PullRequestNumber,
            PrAuthorLogin = message.PrAuthorLogin,
            QualityScore = codeReview.QualityScore,
            TotalComments = codeReview.TotalComments,
            CriticalCount = codeReview.Comments.Count(c => c.Severity == Severity.Critical),
            WarningCount = codeReview.Comments.Count(c => c.Severity == Severity.Warning),
            SuggestionCount = codeReview.Comments.Count(c => c.Severity == Severity.Suggestion),
            CompletedOn = codeReview.CompletedOn ?? DateTime.UtcNow
        };

        await _reviewPublisher.PublishAsync(completedMessage);

        _logger.LogInformation("PR #{PullRequestNumber} review completed. Score: {Score}",
            message.PullRequestNumber, codeReview.QualityScore);

        return OrchestratorResult.Success;
    }

    private async Task<bool> SaveReviewAsync(CodeReview codeReview)
    {
        try
        {
            _context.CodeReviews.Add(codeReview);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save CodeReview {CodeReviewId}", codeReview.Id);
            return false;
        }
    }
}