using System;

namespace Lw.ApplicationMessages
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field, 
        Inherited = false, 
        AllowMultiple = false)]
    public sealed class ApplicationMessageCodeAttribute : Attribute
    {
    }
}
