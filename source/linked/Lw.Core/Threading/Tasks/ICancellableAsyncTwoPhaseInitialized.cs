using System.Threading;
using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    public interface ICancellableAsyncTwoPhaseInitialized : ICancellableAsyncInitialized, IAsyncTwoPhaseInitialized
    {
        Task PreinitializeAsync(CancellationToken token);
    }
}
