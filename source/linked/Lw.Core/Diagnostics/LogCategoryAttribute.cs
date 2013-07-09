using System;

namespace Lw.Diagnostics
{
    [global::System.AttributeUsage(
        AttributeTargets.Assembly | 
            AttributeTargets.Class | 
            AttributeTargets.Constructor | 
            AttributeTargets.Interface | 
            AttributeTargets.Method | 
            AttributeTargets.Property | 
            AttributeTargets.Struct, 
        Inherited = false, 
        AllowMultiple = true)]
    public sealed class LogCategoryAttribute : Attribute
    {
        public LogCategoryAttribute(string logCategory)
        {
            this.LogCategory = logCategory;
        }

        public string LogCategory {get; private set;}
    }
}
