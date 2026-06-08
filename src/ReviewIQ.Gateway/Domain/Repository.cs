namespace ReviewIQ.Gateway.Domain
{
    public class Repository
    {
        public Guid Id { get; private set; }
        public string GitHubRepoId { get; private set; } = string.Empty;
        public string Owner { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string WebhookSecret { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private Repository() { }
    }
}
