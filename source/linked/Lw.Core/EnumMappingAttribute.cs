using System;

namespace Lw
{
    [global::System.AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = true)]
    public sealed class EnumMappingAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        public EnumMappingAttribute(Type mappedType)
            : this(mappedType, null)
        {
        }

        public EnumMappingAttribute(Type mappedType, Type enumMappingStrategy)
        {
            this.mappedType = mappedType;
            this.enumMappingStrategy = enumMappingStrategy ?? typeof(ValueEnumMappingStrategy);
        }

        public TDestination Map<TDestination>(Enum value)
        {
            if (typeof(TDestination) != mappedType)
            {
                ExceptionOperations.ThrowArgumentException("TDestination");
            }

            return (TDestination)Map(value);
        }

        public object Map(Enum value)
        {
            IEnumMappingStrategy mapper = (IEnumMappingStrategy)Activator.CreateInstance(enumMappingStrategy);

            return mapper.Map(mappedType, value);
        }

        public Type EnumMappingStrategy { get; set; }

        public Type MappedType
        {
            get { return mappedType; }
        }

        readonly private Type mappedType;
        readonly private Type enumMappingStrategy;
    }
}
