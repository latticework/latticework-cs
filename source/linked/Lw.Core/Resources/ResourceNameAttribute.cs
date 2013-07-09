using System;

namespace Lw.Resources
{
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class ResourceNameAttribute : Attribute
    {
        readonly string resourceName;

        // This is a positional argument
        public ResourceNameAttribute(string resourceName)
        {
            this.resourceName = resourceName;
        }

        public string ResourceName
        {
            get { return resourceName; }
        }
    }
}
