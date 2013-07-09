using System;

namespace Lw
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lw.Extensions.Map{TDestination}(IEnumMappingStrategy, Enum)"/>
    public interface IEnumMappingStrategy
    {
        object Map(Type destinationType, Enum value);
    }
}
