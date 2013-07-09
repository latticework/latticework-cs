using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lw.Linq.Expressions
{
    /// <summary>
    ///     Represents access to an object property.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class PropertyAccessor<TType, TProperty> : IPropertyAccessor<TProperty>
    {
        public PropertyAccessor(PropertyInfo propertyInfo)
            : this(propertyInfo, default(TType))
        {
        }

        public PropertyAccessor(PropertyInfo propertyInfo, TType obj)
        {
            this.Property = propertyInfo;

            this.getter = propertyInfo.CanRead 
                ? (t) => (TProperty)propertyInfo.GetValue(t, null) : (Func<TType, TProperty>)null;

            this.setter = propertyInfo.CanWrite 
                ? (t, p) => propertyInfo.SetValue(t, p, null) 
                : (Action<TType, TProperty>)null;

            this.Obj = obj;
        }

        public PropertyAccessor(Expression<Func<TType, TProperty>> lambdaExpression)
            : this(lambdaExpression, default(TType))
        {
        }

        public PropertyAccessor(Expression<Func<TType, TProperty>> lambdaExpression, TType obj)
        {
            PropertyInfo propertyInfo = null;
            MemberExpression getterExpression = null;
            Expression expressionToCheck = lambdaExpression;

            bool done = false;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        getterExpression = (MemberExpression)expressionToCheck;
                        propertyInfo = getterExpression.Member as PropertyInfo;
                        done = true;
                        break;
                    default:
                        throw new ArgumentException(
                            "Lamda expression does not represent a property accessor", "lambdaExpression");
                }
            }

            Func<TType, TProperty> getter = (t) => lambdaExpression.Compile()(t);

            Action<TType, TProperty> setter = null;
            if (propertyInfo.CanWrite)
            {
                setter = (t, p) => { propertyInfo.SetValue(t, p, null); };
            }

            this.Property = propertyInfo;
            this.getter = getter;
            this.setter = setter;
            this.Obj = obj;
        }


        private Func<TType, TProperty> getter;
        private Action<TType, TProperty> setter;

        public PropertyInfo Property { get; private set; }

        public TType Obj { get; set; }
        public TProperty Value
        {
            get
            {
                VerifyObj();
                return getter(Obj);
            }
            set
            {
                VerifyObj();
                if (setter == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                        "The property '{0}.{1}' has no setter.",
                        Property.DeclaringType.Name,
                        Property.Name));
                }

                setter(Obj, value);
            }
        }

        private void VerifyObj()
        {
            if (Obj == null)
            {
                throw new InvalidOperationException("Cannot access property if 'Obj' is 'null'.");
            }
        }

        #region IPropertyAccessor Members


        object IPropertyAccessor.Value
        {
            get
            {
                return Value;
            }
            set
            {
                if (!(value is TProperty)) { throw new InvalidOperationException(); }

                Value = (TProperty)value;
            }
        }

        #endregion
    }
}
