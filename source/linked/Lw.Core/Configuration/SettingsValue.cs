using System;
using System.Collections.Generic;

namespace Lw.Configuration
{
    public class SettingsValue
    {
        public virtual SettingDataType DataType { get; set; }
        public SettingsPath Path { get; set; }

        public object GetDefaultValue() { throw new NotImplementedException(); }
        public IList<SettingsValue> GetFallbackValues() { throw new NotImplementedException(); }
    }
}
