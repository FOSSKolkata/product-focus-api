using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessRequirements.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
