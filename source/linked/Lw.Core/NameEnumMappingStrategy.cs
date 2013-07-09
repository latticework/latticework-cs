using System;
using System.Diagnostics;

namespace Lw
{
    public class NameEnumMappingStrategy : IEnumMappingStrategy
    {
        [DebuggerStepThrough]
        public object Map(Type destinationType, Enum value)
        {
            ExceptionOperations.VerifyNonNull(destinationType, () => destinationType);
            EnumOperations.VerifyEnumType(destinationType, () => destinationType);

            string name = value.GetName();

            return Enum.Parse(destinationType, name, false);
        }
    }
}
