using System;

namespace Lw.Diagnostics
{
    [global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class LogCategorySourceAttribute : Attribute
    {
        public LogCategorySourceAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}
