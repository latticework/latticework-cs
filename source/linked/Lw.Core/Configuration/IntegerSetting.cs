using System;

namespace Lw.Configuration
{
    // TODO: IntegerSetting -- Robust error messages.
    public class IntegerSetting : SettingsValue
    {
        public override SettingDataType DataType 
        { 
            get { return SettingDataType.Integer; }
            set { throw new InvalidOperationException(); }
        }
    }
}
