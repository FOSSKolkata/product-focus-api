using Common;

namespace ProductFocus.Domain.Repositories
{
    public interface IPermissionRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
    {
    }
}
