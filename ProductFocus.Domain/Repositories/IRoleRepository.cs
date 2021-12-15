using Common;

namespace ProductFocus.Domain.Repositories
{
    public interface IRoleRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
    {
    }
}
