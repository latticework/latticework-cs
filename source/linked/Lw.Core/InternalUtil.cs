using System;

namespace Lw
{
    internal class InternalUtil
    {
        public static readonly string CallContextPrefix = "Lw.{0}.".DoFormat(Guid.NewGuid());
    }
}
