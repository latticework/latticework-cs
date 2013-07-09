using System.Security.Principal;

namespace Lw.Services
{
    public interface ISecurityService
    {
        IPrincipal CurrentPrincipal { get; }
    }
}
