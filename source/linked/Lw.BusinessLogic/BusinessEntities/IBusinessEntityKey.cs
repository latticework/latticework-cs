using System;

namespace Lw.BusinessEntities
{
    public interface IBusinessEntityKey : IEquatable<IBusinessEntityKey>
    {
        void LoadToken(string keyToken);
        string ToToken();
    }
}
