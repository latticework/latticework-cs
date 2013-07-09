using System;

namespace Lw.ApplicationMessages
{
    [global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class ApplicationMessageSourceAttribute : Attribute
    {
        public ApplicationMessageSourceAttribute(Type type)
        {
            this.type = type;
        }

        public const string DefaultMessageFormatMethod = "GetFormatString";

        public Type Type
        {
            get { return type; }
        }

        public string MessageFormatMethod
        {
            get { return this.messageFormatMethod ?? ApplicationMessageSourceAttribute.DefaultMessageFormatMethod; }
            set { this.messageFormatMethod = value; }
        }

        private readonly Type type;
        private string messageFormatMethod;
    }
}
