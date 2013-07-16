using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Lw.Collections.Generic;

namespace Lw.ComponentModel.DataAnnotations
{
    public class GenericAssociatedMetadataTypeTypeDescriptionProvider : TypeDescriptionProvider
    {
        public GenericAssociatedMetadataTypeTypeDescriptionProvider()
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            var provider = providers.GetOrCreateValue(objectType,
                () => new AssociatedMetadataTypeTypeDescriptionProvider(objectType));

            return provider.GetTypeDescriptor(objectType, instance);
        }

        Dictionary<Type, AssociatedMetadataTypeTypeDescriptionProvider> providers = 
            new Dictionary<Type, AssociatedMetadataTypeTypeDescriptionProvider>();
    }
}
