using System;

namespace Lw.ApplicationMessages
{
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class ApplicationMessageArgumentAttribute : Attribute
    {
        readonly string name;
        readonly Type type;
        readonly string description;

        // This is a positional argument
        public ApplicationMessageArgumentAttribute(string name, Type type, string description)
        {
            this.name = name;
            this.type = type;
            this.description = description;
        }

        public string Name
        {
            get { return name; }
        }

        public Type Type
        {
            get { return type; }
        }

        public string Description
        {
            get { return description; }
        }
    }
}
