using System;

namespace Lw
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Iim.Extensions.Map{TDestination}(IEnumMappingStrategy, Enum)"/>
    public interface IEnumMappingStrategy
    {
        object Map(Type destinationType, Enum value);
    }
}
