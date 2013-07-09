using System.Security.Principal;

namespace Lw.Security.Principal
{
#if !NETFX_CORE
    public interface ICustomIdentity : IIdentity
    {
        WindowsIdentity WindowsIdentity { get; }
    }
#endif
}
