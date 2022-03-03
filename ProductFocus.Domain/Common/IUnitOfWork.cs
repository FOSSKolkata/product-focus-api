using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
