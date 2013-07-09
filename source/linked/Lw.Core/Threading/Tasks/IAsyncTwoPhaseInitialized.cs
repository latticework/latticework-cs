using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    public interface IAsyncTwoPhaseInitialized : IAsyncInitialized
    {
        Task PreinitializeAsync();
    }
}
