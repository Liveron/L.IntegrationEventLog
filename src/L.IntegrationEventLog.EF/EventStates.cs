namespace L.IntegrationEventLog.EF;

public enum EventState : byte
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFailed = 3,
}