using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;

namespace Lw.Text
{
    public static class Extensions
    {
        #region StringBuilder
        [DebuggerStepThrough]
        public static void AppendLineFormat(this StringBuilder reference, string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null, "format");

            reference.AppendLine(string.Format(format, args));
        }
        #endregion StringBuilder
    }
}
