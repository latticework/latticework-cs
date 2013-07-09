using Lw.Reflection;
using Lw.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

namespace Lw.Collections.ObjectModel
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class Set<T, TSet> : ISet<T>
#if !NETFX_CORE
        , ISerializable
#endif
        where TSet : ISet<T>, new()
    {
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> class
        //     that is empty and uses the default equality comparer for the set type.
        public Set()
        {
            this.set = new TSet();
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> class
        //     that uses the default equality comparer for the set type, contains elements
        //     copied from the specified collection, and has sufficient capacity to accommodate
        //     the number of elements copied.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements are copied to the new set.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public Set(IEnumerable<T> collection)
        {
            this.set = ActivatorOperations.CreateInstance<TSet>(collection);
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> class
        //     that is empty and uses the specified equality comparer for the set type.
        //
        // Parameters:
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer<ConfigurationFolder> implementation to use
        //     when comparing values in the set, or null to use the default System.Collections.Generic.EqualityComparer<ConfigurationFolder>
        //     implementation for the set type.
        public Set(IEqualityComparer<T> comparer)
        {
            this.set = ActivatorOperations.CreateInstance<TSet>(comparer);
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> class
        //     that uses the specified equality comparer for the set type, contains elements
        //     copied from the specified collection, and has sufficient capacity to accommodate
        //     the number of elements copied.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements are copied to the new set.
        //
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer<ConfigurationFolder> implementation to use
        //     when comparing values in the set, or null to use the default System.Collections.Generic.EqualityComparer<ConfigurationFolder>
        //     implementation for the set type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public Set(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            this.set = ActivatorOperations.CreateInstance<TSet>(collection, comparer);
        }

#if !NETFX_CORE
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> class
        //     with serialized data.
        //
        // Parameters:
        //   info:
        //     A System.Runtime.Serialization.SerializationInfo object that contains the
        //     information required to serialize the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        //   context:
        //     A System.Runtime.Serialization.StreamingContext structure that contains the
        //     source and destination of the serialized stream associated with the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        protected Set(SerializationInfo info, StreamingContext context)
        {
            this.set = info.GetValue<ISet<T>>("Set");
        }
#endif

        // Summary:
        //     Gets the System.Collections.Generic.IEqualityComparer<ConfigurationFolder> object that is used
        //     to determine equality for the values in the set.
        //
        // Returns:
        //     The System.Collections.Generic.IEqualityComparer<ConfigurationFolder> object that is used to
        //     determine equality for the values in the set.
        public IEqualityComparer<T> Comparer
        {
            get { return this.set.GetPropertyValue<IEqualityComparer<T>>("Comparer"); }
        }

        //
        // Summary:
        //     Gets the number of elements that are contained in a set.
        //
        // Returns:
        //     The number of elements that are contained in the set.
        public int Count
        {
            get { return this.set.Count; }
        }

        // Summary:
        //     Adds the specified element to a set.
        //
        // Parameters:
        //   item:
        //     The element to add to the set.
        //
        // Returns:
        //     true if the element is added to the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object; false if the element is already present.
        public bool Add(T item)
        {
            return this.set.Add(item);
        }

        //
        // Summary:
        //     Removes all elements from a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        public void Clear()
        {
            this.set.Clear();
        }
        //
        // Summary:
        //     Determines whether a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object contains
        //     the specified element.
        //
        // Parameters:
        //   item:
        //     The element to locate in the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object contains the specified
        //     element; otherwise, false.
        public bool Contains(T item)
        {
            return this.set.Contains(item);
        }

        //
        // Summary:
        //     Copies the elements of a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object to
        //     an array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional array that is the destination of the elements copied
        //     from the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object. The array must have
        //     zero-based indexing.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        public void CopyTo(T[] array)
        {
            this.set.CopyTo(array, 0);
        }

        //
        // Summary:
        //     Copies the elements of a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object to
        //     an array, starting at the specified array index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional array that is the destination of the elements copied
        //     from the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object. The array must have
        //     zero-based indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   System.ArgumentException:
        //     arrayIndex is greater than the length of the destination array.
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.set.CopyTo(array, arrayIndex);
        }

        //
        // Summary:
        //     Copies the specified number of elements of a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object to an array, starting at the specified array index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional array that is the destination of the elements copied
        //     from the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object. The array must have
        //     zero-based indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        //   count:
        //     The number of elements to copy to array.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        //     arrayIndex is greater than the length of the destination array.-or-count
        //     is greater than the available space from the index to the end of the destination
        //     array.
        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            this.set.InvokeMethod("CopyTo", array, arrayIndex, count);
        }

#if !NETFX_CORE
        //
        // Summary:
        //     Returns an System.Collections.IEqualityComparer object that can be used for
        //     equality testing of a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        //
        // Returns:
        //     An System.Collections.IEqualityComparer object that can be used for deep
        //     equality testing of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        public static IEqualityComparer<Set<T, TSet>> CreateSetComparer()
        {
            return typeof(TSet).InvokeMethod<IEqualityComparer<Set<T, TSet>>>("CreateSetComparer");
        }
#endif

        //
        // Summary:
        //     Removes all elements in the specified collection from the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Parameters:
        //   other:
        //     The collection of items to remove from the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public void ExceptWith(IEnumerable<T> other)
        {
            this.set.ExceptWith(other);
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     A System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>.Enumerator object for the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        public Set<T, TSet>.Enumerator GetEnumerator()
        {
            return new Set<T, TSet>.Enumerator(this.set.GetEnumerator());
        }

#if !NETFX_CORE
        //
        // Summary:
        //     Implements the System.Runtime.Serialization.ISerializable interface and returns
        //     the data needed to serialize a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        //
        // Parameters:
        //   info:
        //     A System.Runtime.Serialization.SerializationInfo object that contains the
        //     information required to serialize the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        //   context:
        //     A System.Runtime.Serialization.StreamingContext structure that contains the
        //     source and destination of the serialized stream associated with the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     info is null.
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue<ISet<T>>("Set", this.set);
        }
#endif

        //
        // Summary:
        //     Modifies the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object to contain
        //     only elements that are present in that object and in the specified collection.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public void IntersectWith(IEnumerable<T> other)
        {
            this.set.IntersectWith(other);
        }

        //
        // Summary:
        //     Determines whether a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a proper
        //     subset of the specified collection.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a proper subset
        //     of other; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.set.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.set.IsProperSupersetOf(other);
        }

        //
        // Summary:
        //     Determines whether a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a subset
        //     of the specified collection.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a subset of other;
        //     otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.set.IsSubsetOf(other);
        }

        //
        // Summary:
        //     Determines whether a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a superset
        //     of the specified collection.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is a superset of
        //     other; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.set.IsSupersetOf(other);
        }

        //
        // Summary:
        //     Determines whether the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object
        //     and a specified collection share common elements.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object and other share
        //     at least one common element; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public bool Overlaps(IEnumerable<T> other)
        {
            return this.set.Overlaps(other);
        }

        //
        // Summary:
        //     Removes the specified element from a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Parameters:
        //   item:
        //     The element to remove.
        //
        // Returns:
        //     true if the element is successfully found and removed; otherwise, false.
        //     This method returns false if item is not found in the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        public bool Remove(T item)
        {
            return this.set.Remove(item);
        }

        //
        // Summary:
        //     Removes all elements that match the conditions defined by the specified predicate
        //     from a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> collection.
        //
        // Parameters:
        //   match:
        //     The System.Predicate<ConfigurationFolder> delegate that defines the conditions of the elements
        //     to remove.
        //
        // Returns:
        //     The number of elements that were removed from the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     collection.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     match is null.
        public int RemoveWhere(Predicate<T> match)
        {
            return this.set.InvokeMethod<int>("RemoveWhere", match);
        }

        //
        // Summary:
        //     Determines whether a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object and the
        //     specified collection contain the same elements.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Returns:
        //     true if the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object is equal to other;
        //     otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public bool SetEquals(IEnumerable<T> other)
        {
            return this.set.SetEquals(other);
        }

        //
        // Summary:
        //     Modifies the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object to contain
        //     only elements that are present either in that object or in the specified
        //     collection, but not both.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            this.set.SymmetricExceptWith(other);
        }

        //
        // Summary:
        //     Modifies the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object to contain
        //     all elements that are present in itself, the specified collection, or both.
        //
        // Parameters:
        //   other:
        //     The collection to compare to the current System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
        //     object.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     other is null.
        public void UnionWith(IEnumerable<T> other)
        {
            this.set.UnionWith(other);
        }

        // Summary:
        //     Enumerates the elements of a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> object.
        [Serializable]
        public struct Enumerator : IEnumerator<T>, IDisposable
        {
            internal Enumerator(IEnumerator<T> setEnumerator)
            {
                this.setEnumerator = setEnumerator;
            }

            // Summary:
            //     Gets the element at the current position of the enumerator.
            //
            // Returns:
            //     The element in the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder> collection at the
            //     current position of the enumerator.
            public T Current
            {
                get { return this.setEnumerator.Current; }
            }

            // Summary:
            //     Releases all resources used by a System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>.Enumerator
            //     object.
            public void Dispose()
            {
                this.setEnumerator.Dispose();
            }

            //
            // Summary:
            //     Advances the enumerator to the next element of the System.Collections.Generic.ConfigurationFolderCollection<ConfigurationFolder>
            //     collection.
            //
            // Returns:
            //     true if the enumerator was successfully advanced to the next element; false
            //     if the enumerator has passed the end of the collection.
            //
            // Exceptions:
            //   System.InvalidOperationException:
            //     The collection was modified after the enumerator was created.
            public bool MoveNext()
            {
                return this.setEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.setEnumerator.Reset();
            }

            private IEnumerator<T> setEnumerator;

            object System.Collections.IEnumerator.Current
            {
                get { throw new NotImplementedException(); }
            }
        }

        private ISet<T> set;

        bool ISet<T>.Add(T item)
        {
            return this.Add(item);
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            this.ExceptWith(other);
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            this.IntersectWith(other);
        }

        bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.IsProperSubsetOf(other);
        }

        bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.IsProperSupersetOf(other);
        }

        bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            return this.IsSubsetOf(other);
        }

        bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            return this.IsSupersetOf(other);
        }

        bool ISet<T>.Overlaps(IEnumerable<T> other)
        {
            return this.Overlaps(other);
        }

        bool ISet<T>.SetEquals(IEnumerable<T> other)
        {
            return this.SetEquals(other);
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            this.SymmetricExceptWith(other);
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            this.UnionWith(other);
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        void ICollection<T>.Clear()
        {
            this.Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        int ICollection<T>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool IsReadOnly
        {
            get { return this.set.IsReadOnly; }
        }

        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        bool ICollection<T>.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }
    }
}
