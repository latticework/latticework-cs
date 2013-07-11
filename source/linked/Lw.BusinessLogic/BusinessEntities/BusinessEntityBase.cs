using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Lw.Collections.Generic;
using Lw.ComponentModel.Composition;
using Lw.ComponentModel.DataAnnotations;

namespace Lw.BusinessEntities
{
    public abstract class BusinessEntityBase : IBusinessEntity
    {
        #region Public Properties
        public IComponentContainer ComponentContainer
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref componentContainer, () => Components.Current);
            }
            set
            {
                // TODO: AnnontatedValue.ComponentContainer: May need to rebind if no container set.
                ExceptionOperations.VerifyNonNull(value, () => value);

                this.componentContainer = value;
            }
        }
        #endregion Public Properties

        #region Public Methods
        //public ValidationErrorCollection TryValidate()
        //{
        //    return TryValidateOverride();
        //}

        //public ValidationErrorCollection TryValidateProperty(string propertyName)
        //{
        //    ExceptionOperations.VerifyNonNull(propertyName, () => propertyName);

        //    return TryValidatePropertyOverride(propertyName);
        //}
        #endregion Public Methods

        #region Protected Methods
//        protected virtual ValidationErrorCollection TryValidateOverride()
//        {

//            var publicInstanceProperties = BindingFlags.Instance | BindingFlags.Public;

//            var errorList = (
//                    from po in this.GetType().GetProperties(publicInstanceProperties)
//                    from ve in this.TryValidateProperty(po.Name)
//                    select ve)
//                .ToList();

//            errorList.AddRange(TryValidateCore());

//            return new ValidationErrorCollection(errorList);
//        }

//        protected virtual ValidationErrorCollection TryValidateCore()
//        {
//            return new ValidationErrorCollection();
//        }

//        protected virtual ValidationErrorCollection TryValidatePropertyCore()
//        {
//            return new ValidationErrorCollection();
//        }

//        protected virtual ValidationErrorCollection TryValidatePropertyOverride(string propertyName)
//        {
//#if (SILVERLIGHT)
//            throw new NotImplementedException();
//#else
//            IDataAnnotationProvider provider;

//            bool found = ComponentContainer.TryGetInstance<IDataAnnotationProvider>(out provider);

//            if (!found)
//            {
//                provider = defaultProvider;
//            }

//            var errors = provider.TryValidate(this, propertyName);

//            // If the RequiredAttribute validation error not there, perform custom validation.
//            if (!errors.Any(ve => ve is AnnotationValidationError && ((AnnotationValidationError)ve).Validator is RequiredAttribute))
//            {
//                errors.AddRange(TryValidatePropertyCore());
//            }

//            return errors;
//#endif // SILVERLIGHT
//        }

        protected abstract bool EqualsInternal(IBusinessEntity other);

        protected abstract IBusinessEntityKey GetKeyInternal();
        #endregion Protected Methods

        #region Private Fields
#if (SILVERLIGHT)
#else
        private static AssociatedMetadataTypeDataAnnotationProvider defaultProvider =
            new AssociatedMetadataTypeDataAnnotationProvider();
#endif //SILVERLIGHT
        private IComponentContainer componentContainer = null;
        #endregion Private Fields

        #region Explicit Interface Implementations
        IBusinessEntityKey IBusinessEntity.GetKey()
        {
            return GetKeyInternal();
        }

        bool IEquatable<IBusinessEntity>.Equals(IBusinessEntity other)
        {
            return EqualsInternal(other);
        }
        #endregion Explicit Interface Implementations
    }
}
