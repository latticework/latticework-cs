using System;

using Lw.ComponentModel.Composition;
using Lw.ComponentModel.DataAnnotations;

namespace Lw.BusinessEntities
{
    public interface IBusinessEntity : IEquatable<IBusinessEntity>
    {
        IBusinessEntityKey GetKey();

        //ValidationErrorCollection TryValidate();
        //ValidationErrorCollection TryValidateProperty(string propertyName);

        IComponentContainer ComponentContainer { get; set; }
    }
}
