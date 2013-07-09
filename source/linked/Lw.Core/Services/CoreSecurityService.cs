using System.Security.Principal;
using System.Threading;

namespace Lw.Services
{
    public class CoreSecurityService : ISecurityService
    {
        public IPrincipal CurrentPrincipal
        {
            get
            {
                return Thread.CurrentPrincipal;
            }
        }
    }
}
