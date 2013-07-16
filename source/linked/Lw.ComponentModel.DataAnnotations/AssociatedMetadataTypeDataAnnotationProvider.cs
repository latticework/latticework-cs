
namespace Lw.ComponentModel.DataAnnotations
{
    public class AssociatedMetadataTypeDataAnnotationProvider : TypeDescriptionDataAnnotationProvider
    {
        public AssociatedMetadataTypeDataAnnotationProvider()
            : base(new GenericAssociatedMetadataTypeTypeDescriptionProvider())
        {
        }
    }
}
