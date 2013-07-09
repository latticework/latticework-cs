using System;

namespace Lw.Diagnostics
{
    [global::System.AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ExceptionPolicyAttribute : Attribute
    {
        public ExceptionPolicyAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}
