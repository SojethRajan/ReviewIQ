namespace ReviewIQ.Gateway.Domain
{
    public class IncomingEvent
    {
        public Guid Id { get; private set; }
        public Guid RepositoryId { get; private set; }
        public string DeliveryId { get; private set; } = string.Empty;
        public string EventType { get; private set; } = string.Empty;
        public string Action { get; private set; } = string.Empty;
        public int PullRequestNumber { get; private set; }
        public string PullRequestTitle { get; private set; } = string.Empty;
        public string PrAuthorLogin { get; private set; } = string.Empty;
        public string RawPayload { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public DateTime ReceivedOn { get; private set; }

        private IncomingEvent() { }

        public static IncomingEvent Create(
            Guid repositoryId,
            string deliveryId,
            string eventType,
            string action,
            int pullRequestNumber,
            string pullRequestTitle,
            string prAuthorLogin,
            string rawPayload)
        {
            return new IncomingEvent
            {
                Id = Guid.NewGuid(),
                RepositoryId = repositoryId,
                DeliveryId = deliveryId,
                EventType = eventType,
                Action = action,
                PullRequestNumber = pullRequestNumber,
                PullRequestTitle = pullRequestTitle,
                PrAuthorLogin = prAuthorLogin,
                RawPayload = rawPayload,
                Status = "Received",
                ReceivedOn = DateTime.UtcNow
            };
        }

        public void MarkAsPublished()
        {
            Status = "Published";
        }

        public void MarkAsFailed()
        {
            Status = "Failed";
        }
    }
}
