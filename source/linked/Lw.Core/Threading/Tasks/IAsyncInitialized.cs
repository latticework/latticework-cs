using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    public interface IAsyncInitialized
    {
        Task InitializeAsync();
    }
}

