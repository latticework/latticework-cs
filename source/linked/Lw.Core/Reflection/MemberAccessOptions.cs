using System;

namespace Lw.Reflection
{
    [Flags]
    public enum MemberAccessOptions
    {
        None = 0,
        Static = 1,
        IncludeNonPublic = 2,
    }
}
