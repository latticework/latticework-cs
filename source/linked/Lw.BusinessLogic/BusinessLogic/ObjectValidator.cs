using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
//using Lw.Data.Entity;

namespace Lw.BusinessLogic
{
    public class ObjectValidator<T> //<TDbContext,TEntity>//where TEntity : IEntity
    {
        #region Public Constructor
        public static EntityValidationResult Validate(T entity)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, context, validationResults, validateAllProperties: true);
            /*var validator = this as IValidatableObject;
            if (validator != null)
            {
                foreach (var error in validator.Validate(vc))
                    validationList.Add(error);
            }*/
            return new EntityValidationResult(validationResults);
        }

        /*public EntityValidationResult Validate(TDbContext context, TEntity entity)
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var modelEntity = entity as DbEntityEntry;
            if (null != modelEntity)
            {
                DbEntityValidationResult dbValidationResults = modelEntity.GetValidationResult();
                return WrapValidationResults(dbValidationResults);
            }
            else
            {
                var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);
                return new EntityValidationResult(validationResults);
            }
        }*/
        #endregion Public Constructor
    }
}
