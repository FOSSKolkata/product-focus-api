namespace IntegrationEventLogEF
{
    public enum EventStateEnum
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3,
        ProcessingInProgress = 4,
        Processed = 5,
        ProcessingFailed = 6
    }
}
