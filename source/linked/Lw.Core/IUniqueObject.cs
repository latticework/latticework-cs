using System;

namespace Lw
{
    public interface IUniqueObject : IEquatable<IUniqueObject>
    {
        Guid Uid { get; }

        UniqueObjectPath Find(Guid uid);
    }
}
