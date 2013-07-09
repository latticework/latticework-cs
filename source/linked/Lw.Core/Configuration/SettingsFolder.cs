using System.Collections.Generic;

namespace Lw.Configuration
{
    public class SettingsFolder
    {
        internal SettingsFolder(ISettingsDb db, SettingsRoot root, string path, string name)
        {
            this.Db = db;
            this.Root = root;
            this.Path = path;
            this.Name = name;
        }

        public ISettingsDb Db { get; internal set; }
        public string Name { get; internal set; }
        public string Path { get; internal set; }
        public SettingsRoot Root { get; internal set; }
        public ISet<SettingsFolder> Folders { get; private set; }
        public ISet<SettingsValue> Values { get; private set; }
    }
}
