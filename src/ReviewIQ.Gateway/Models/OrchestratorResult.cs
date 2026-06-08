namespace ReviewIQ.Gateway.Models
{
    public enum OrchestratorResult
    {
        Success,
        InvalidSignature,
        EventIgnored,
        ActionIgnored,
        DuplicateEvent,
        RepositoryNotFound,
        PersistenceFailed,
        PublishFailed
    }
}
