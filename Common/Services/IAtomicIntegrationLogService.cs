namespace IntegrationCommon.Services
{
    public interface IAtomicIntegrationLogService
    {
        Task SaveAtomicallyWithDbContextChangesAsync();
    }
}
