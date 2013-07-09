using Lw.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lw.Configuration
{
    public class SettingsDbProviderDescription
    {
        public string Description { get; set;  }
        public string Name { get; set; }
        public IList<SettingsRoot> SupportedRoots { get; set; }
        public IList<int> SupportedVersions { get; set; }
        public IList<int> ConversionVersions { get; set; }
        public Guid Uid { get; set; }

        public int GetNewestVersion()
        {
            if (this.SupportedVersions.IsNullOrEmpty())
            {
                throw new InvalidOperationException();
            }

            return this.SupportedVersions.Max();
        }
    }
}
