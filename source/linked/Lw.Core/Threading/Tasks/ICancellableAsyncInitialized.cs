using System.Threading;
using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    public interface ICancellableAsyncInitialized : IAsyncInitialized
    {
        Task InitializeAsync(CancellationToken token);
    }
}
