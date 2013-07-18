using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Lw.Data.Metadata.Edm;
#if DOTNET45  
using System.Data.Objects;
using System.Data.Metadata.Edm;
#else
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
#endif

namespace Lw.Data.Objects
{
    public static class Extensions
    {
        /// <summary>
        ///     Creates the properly typed parameter objext for the specified Enity Framework FunctionImport 
        ///     parameter.
        /// </summary>
        /// <param name="reference">
        ///     The <see cref="ObjectContext"/> that defines the FunctionImport.
        /// </param>
        /// <param name="functionImportName">
        ///     A <see cref="String"/>.
        /// </param>
        /// <param name="parameterName">
        ///     A <see cref="String"/>.
        /// </param>
        /// <returns>
        ///     A properly typed <see cref="ObjectParameter"/>.
        /// </returns>
        /// <remarks>
        ///     Inspects the metadata for the specifed parameter and configures the new <see cref="ObjectParameter"/>
        ///     with the neccesary type information.
        /// </remarks>
        public static ObjectParameter CreateFunctionImportParameter(this ObjectContext reference, 
            string functionImportName, string parameterName, string containerName = "")
        {
           return reference.MetadataWorkspace.CreateFunctionImportParameter(
                functionImportName, parameterName, containerName);
        }

        public static EntityTypeBase GetEntityType(this ObjectContext reference, 
            Type clrType, string containerName = "")
        {
            return reference.MetadataWorkspace.GetEntityType(clrType, containerName);
        }
    }
}
