using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Lw.Diagnostics
{
    public static class LoggingOperations
    {
        [DebuggerStepThrough]
        public static IEnumerable<string> GetStaticLogCategories(Type source, string sourceMemberName)
        {
            Contract.Requires<ArgumentNullException>(source != null, "type");
            Contract.Requires<ArgumentNullException>(sourceMemberName != null, "sourceMemberName");

            var typeInfo = source.GetTypeInfo();

            var customAttributes = new List<IEnumerable<LogCategoryAttribute>>
            {
                typeInfo.GetCustomAttributes<LogCategoryAttribute>(true),
                typeInfo.Assembly.GetCustomAttributes<LogCategoryAttribute>()
            };

            // Get the first ICustomAttributeProvider that has LogCategoryAttributes defined.
            var logCategories = (
                    from lcas in customAttributes
                    from lca in lcas
                    select lca.LogCategory)
                .ToList();

            // Add namespace categories.
            logCategories.Add(typeInfo.Namespace);
            logCategories.Add(typeInfo.FullName);
            logCategories.Add(typeInfo.FullName + "." + sourceMemberName);

            return logCategories;
        }
    }
}
