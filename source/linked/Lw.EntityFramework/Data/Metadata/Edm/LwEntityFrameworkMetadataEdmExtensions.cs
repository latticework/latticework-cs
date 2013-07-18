using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
#if DOTNET45  
using System.Data.Objects;
using System.Data.Metadata.Edm;
#else
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
#endif


namespace Lw.Data.Metadata.Edm
{
    // http://www.dotnetframework.org/default.aspx/Net/Net/3@5@50727@3053/DEVDIV/depot/DevDiv/releases/Orcas/SP/ndp/fx/src/DataEntity/System/Data/Common/Utils/MetadataHelper@cs/4/MetadataHelper@cs
    public static class Extensions
    {
        #region EdmMember Extensions
        public static TypeUsage GetElementType(this EdmMember reference)
        {
            if (reference.IsCollectionMember())
            {
                var elementType = ((CollectionType)reference.TypeUsage.EdmType).TypeUsage;

                return elementType.GetElementType();
            }

            return reference.TypeUsage;
        }
        public static bool IsCollectionMember(this EdmMember reference)
        {
            return reference.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.CollectionType;
        }

        public static bool IsSimpleMember(this EdmMember reference)
        {
            return reference.TypeUsage.EdmType.BuiltInTypeKind.In(
                BuiltInTypeKind.PrimitiveType, BuiltInTypeKind.EnumType);
        }
        #endregion EdmMember Extensions

        #region MetadataWorkspace Extensions
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
        public static ObjectParameter CreateFunctionImportParameter(this MetadataWorkspace reference,
            string functionImportName, string parameterName, string containerName = "")
        {
            var parameter = reference
                .GetEntityContainer(containerName)
                .FunctionImports
                .Where(f => f.Name.EqualsOrdinalIgnoreCase(functionImportName))
                .Single()
                .Parameters
                .Where(fp => fp.Name.EqualsOrdinalIgnoreCase(parameterName))
                .Single();

            return new ObjectParameter(parameter.Name, ((PrimitiveType)parameter.TypeUsage.EdmType).ClrEquivalentType);
        }

        public static EntityContainer GetEntityContainer(this MetadataWorkspace reference, 
            string containerName = "")
        {
            if (containerName.IsNullOrEmpty())
            { 
                return reference.GetItems<EntityContainer>(DataSpace.CSpace).Single();
            }

            return reference.GetEntityContainer(containerName, ignoreCase: false, dataSpace: DataSpace.CSpace);
        }

        public static EntityTypeBase GetEntityType(this MetadataWorkspace reference,
            Type clrType, string containerName = "")
        {
            return reference
                .GetEntityContainer(containerName)
                .BaseEntitySets
                .Single(esb => esb.ElementType.Name == clrType.Name)
                .ElementType;
        }
        #endregion MetadataWorkspace Extensions

        #region TypeUsage Extensions
        public static TypeUsage GetElementType(this TypeUsage reference)
        {
            if (reference.IsCollectionType())
            {
                var elementType = ((CollectionType)reference.EdmType).TypeUsage;

                return elementType.GetElementType();
            }

            return reference;
        }

        public static bool IsCollectionType(this TypeUsage reference)
        {
            return reference.EdmType.BuiltInTypeKind == BuiltInTypeKind.CollectionType;
        }

        #endregion TypeUsage Extensions
    }
}
