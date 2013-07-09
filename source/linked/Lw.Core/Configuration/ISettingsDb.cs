using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lw.Configuration
{
    public interface ISettingsDb
    {
        ISettingsDbProvider GetProvider();

        object GetValue(SettingsRoot root, IEnumerable<string> path, string name, SettingDataType type);
        
        Task<Stream> GetValueAsync(SettingsRoot root, IEnumerable<string> path, string name, SettingDataType type);

        int GetVersion();
        
        void SetValue(SettingsRoot root, IEnumerable<string> path, string name, SettingDataType type, object value);

        Task SetValueAsync(
            SettingsRoot root, IEnumerable<string> path, string name, SettingDataType type, Stream stream);
    }
}
