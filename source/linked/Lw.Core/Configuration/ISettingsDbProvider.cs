using Lw.Threading.Tasks;
using System.Threading.Tasks;

namespace Lw.Configuration
{
    public interface ISettingsDbProvider : IAsyncInitialized
    {
        ISettingsDb GetDb(int version);
        SettingsDbProviderDescription GetDescription();
        Task UpgradeAsync(int installedVersion, int targetVersion);
    }
}
