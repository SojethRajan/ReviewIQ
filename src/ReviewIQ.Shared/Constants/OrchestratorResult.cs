namespace ReviewIQ.Shared.Constants;

public enum OrchestratorResult
{
    Success,
    InvalidSignature,
    EventIgnored,
    ActionIgnored,
    DuplicateEvent,
    RepositoryNotFound,
    PersistenceFailed,
    PublishFailed,
    DiffFetchFailed,
    GeminiFailed,
    ReviewSaveFailed
}
