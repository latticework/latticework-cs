using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lw.ComponentModel.Composition;

namespace Lw.Diagnostics
{
    public static class ExceptionManagerOperations
    {
        public static void DoWithExceptionHandling(string policyName, Action statements)
        {
            Components.Current.GetInstance<IExceptionManager>().DoWithExceptionHandling(policyName, statements);
        }
    }
}
