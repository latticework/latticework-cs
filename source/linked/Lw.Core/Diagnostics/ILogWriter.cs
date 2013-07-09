
using System;
using System.Runtime.CompilerServices;

namespace Lw.Diagnostics
{
    public interface ILogWriter
    {
        void Write(
            Type source,
            LogItem item,
            [CallerMemberName] string sourceMemberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);
    }
}
