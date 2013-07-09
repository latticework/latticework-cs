using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lw
{
    public class DelegateComparer<T, U> : Comparer<T>
    {
        public DelegateComparer(Func<T, U> compareValueSelector)
            : base()
        {
            Contract.Requires<ArgumentNullException>(compareValueSelector != null, "compareValueSelector");

            this.compareValueSelector = compareValueSelector;
        }

        public override int Compare(T x, T y)
        {
            return Comparer<U>.Default.Compare(compareValueSelector(x), compareValueSelector(y));
        }

        private readonly Func<T, U> compareValueSelector;
    }

    public class DelegateComparer<T> : Comparer<T>
    {
        public DelegateComparer(Func<T, T, int> compare)
            : base()
        {
            this.compare = compare;
        }

        public override int Compare(T x, T y)
        {
            return compare(x, y);
        }

        private readonly Func<T, T, int> compare;
    }
}
