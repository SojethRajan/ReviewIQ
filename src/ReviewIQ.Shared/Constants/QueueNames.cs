namespace ReviewIQ.Shared.Constants;

public static class QueueNames
{
    // Exchange names
    public const string PrReviewExchange = "pr.review.exchange";
    public const string ReviewCompletedExchange = "review.completed.exchange";

    // Queue names
    public const string PrReviewQueue = "pr.review.queue";
    public const string ReviewCompletedAnalyticsQueue = "review.completed.analytics.queue";
    public const string ReviewCompletedNotifyQueue = "review.completed.notify.queue";

    // Dead-letter queue names
    public const string PrReviewDlq = "pr.review.dlq";
    public const string ReviewCompletedAnalyticsDlq = "review.completed.analytics.dlq";
    public const string ReviewCompletedNotifyDlq = "review.completed.notify.dlq";

    // Dead-letter exchange
    public const string DeadLetterExchange = "reviewiq.dlx";
}
