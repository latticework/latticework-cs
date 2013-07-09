using System.Reflection;

namespace Lw.Linq.Expressions
{
    public interface IPropertyAccessor
    {
        PropertyInfo Property { get; }
        object Value { get; set; }
    }

    public interface IPropertyAccessor<TProperty> : IPropertyAccessor
    {
        new TProperty Value { get; set; }
    }
}
