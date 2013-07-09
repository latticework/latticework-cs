using System;

namespace Lw
{
    [global::System.AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, 
        Inherited = true, 
        AllowMultiple = true)]
    public sealed class ExceptionContractAttribute : Attribute
    {
        public ExceptionContractAttribute()
            : this(null)
        {
        }

        public ExceptionContractAttribute(Type exceptionType)
        {
            this.exceptionType = exceptionType;
        }

        public Type ExceptionType
        {
            get { return exceptionType; }
        }

        private readonly Type exceptionType;
    }
}
