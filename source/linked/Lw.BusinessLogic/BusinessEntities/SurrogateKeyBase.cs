using System;

namespace Lw.BusinessEntities
{
    public abstract class SurrogateKeyBase<T> : IBusinessEntityKey
        where T : struct, IConvertible
    {
        #region Public Methods
        public void LoadToken(string keyToken)
        {
            KeyValue = (T)((IConvertible)keyToken).ToType(typeof(T), null);
        }

        public string ToToken()
        {
            return ((IConvertible)KeyValue).ToString(null);
        }
        #endregion Public Methods

        #region Protected Properties
        protected abstract T KeyValue { get; set; }
        #endregion Protected Properties

        #region Explicit Interface Implementations
        bool IEquatable<IBusinessEntityKey>.Equals(IBusinessEntityKey other)
        {
            return other is SurrogateKeyBase<T> && this.KeyValue.Equals(((SurrogateKeyBase<T>)other).KeyValue);
        }
        #endregion Explicit Interface Implementations
    }
}
