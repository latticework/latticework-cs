using System.DirectoryServices;
using System.Security.Principal;

namespace Lw.DirectoryServices
{
    public static class DirectoryServicesOperations
    {
        public static string GetUpnFromSid(SecurityIdentifier sid, string ldapServerPath)
        {
            ExceptionOperations.VerifyNonNull(sid, () => sid);
            ExceptionOperations.VerifyNonEmpty(ldapServerPath, () => ldapServerPath);

            var path = "{0}/<SID={1}>".DoFormat(ldapServerPath, sid.Value);

            DirectoryEntry userEntry = new DirectoryEntry(path);

            string upn = userEntry.Properties["userPrincipalName"].Value.ToString();

            return upn;
        }

    }
}
