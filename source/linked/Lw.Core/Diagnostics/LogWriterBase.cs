using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Lw.Diagnostics
{
    public abstract class LogWriterBase : ILogWriter
    {
        public void Write(
            Type source, 
            LogItem item, 
            [CallerMemberName] string sourceMemberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(item != null, "item");

            item.StaticLogCategories = LoggingOperations.GetStaticLogCategories(source, sourceMemberName).ToList();

            this.Write(item);
        }

        protected abstract void Write(LogItem item);
    }
}
