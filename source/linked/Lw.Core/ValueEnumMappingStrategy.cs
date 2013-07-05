using System;

namespace Lw
{
    public class ValueEnumMappingStrategy : IEnumMappingStrategy
    {
        public object Map(Type destinationType, Enum value)
        {
            ExceptionOperations.VerifyNonNull(destinationType, () => destinationType);
            EnumOperations.VerifyEnumType(destinationType, () => destinationType);

            return Enum.ToObject(destinationType, (object)value);
        }
    }
}
