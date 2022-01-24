using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocus.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
