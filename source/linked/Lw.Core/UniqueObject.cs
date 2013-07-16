using Lw.Linq;
using System;
using System.Collections.Generic;

namespace Lw
{
    /// <summary>
    ///     Represents an object with a unique identity. Primarily used as a base class for Business Entities and
    ///     Data Contracts.
    /// </summary>
    /// <remarks>
    ///     <see cref="UniqueObject"/> exposes the <see cref="Uid"/> property that ensures uniqueness. Identity is 
    ///     maintained by implementing <see cref="IEquatable{UniqueObject}"/> along with other equality 
    ///     inviariants, such as a reimplemneted <see cref="GetHashCode"/>. A <see cref="Find"/> method is provided to
    ///     enforce a convention of finding <see cref="UniqueObject"/> instances in a graph.
    /// </remarks>
    /// <seealso cref="Uid"/>
    /// <seealso cref="Find"/>
    /// <seealso cref="IEquatable{T}"/>
#if !NETFX_CORE
    [Serializable]
#endif
    public class UniqueObject : IUniqueObject, IEquatable<UniqueObject>, IEquatable<IUniqueObject>
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new <see cref="UniqueObject"/> with default properties. <see cref="Uid"/> will be a 
        ///     random value.
        /// </summary>
        /// <seealso cref="Uid"/>
        public UniqueObject()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="UniqueObject"/> with the specified <see cref="Guid"/> value.
        /// </summary>
        /// <param name="uid">
        ///     A <see cref="Guid"/>.
        /// </param>
        /// <remarks>
        ///     <note>
        ///         <see cref="Uid"/> is assigned the value of <paramref name="uid"/>.
        ///     </note>
        /// </remarks>
        /// <seealso cref="Uid"/>
        public UniqueObject(Guid uid)
        {
            this.Uid = uid;
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets and sets a unique identifier for the current <see cref="UniqueObject"/>.
        /// </summary>
        /// <value>
        ///     A unique identifier for the current <see cref="UniqueObject"/>.
        /// </value>
        [TransientKey]
        public Guid Uid { get; set; }
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        ///     Determines whether the specified <see cref="UniqueObject"/> is equal to the current 
        ///     <see cref="UniqueObject"/>.
        /// </summary>
        /// <param name="other">
        ///     <see cref="UniqueObject"/> to compare with the current <see cref="UniqueObject"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="UniqueObject"/> is equal to the current 
        ///     <see cref="UniqueObject"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(UniqueObject other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Uid == other.Uid;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        ///     <see cref="object"/> to compare with the current <see cref="object"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="object"/> is equal to the current <see cref="object"/>;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is UniqueObject)
            {
                return Equals((UniqueObject)obj);
            }

            return base.Equals(obj);
        }

        /// <summary>
        ///     Returns a <see cref="UniqueObjectPath"/> representing the path to the <see cref="IUniqueObject"/> in 
        ///     the graph of <see cref="IUniqueObject"/>s rooted at the current <see cref="UniqueObject"/>.
        /// </summary>
        /// <param name="uid">
        ///     The <see cref="Guid"/> idenfiying the <see cref="IUniqueObject"/> in the current 
        ///     <see cref="UniqueObject"/>.
        /// </param>
        /// <returns>
        ///     A <see cref="UniqueObjectPath"/> containing the path to the specified <see cref="IUniqueObject"/>; 
        ///     otherwise, <see cref="UniqueObjectPath.Empty">UniqueObjectPath Empty</see>.
        /// </returns>
        /// <remarks>
        ///     <note type="implementnotes">
        ///         <see cref="Find"/> uses <see cref="Members"/> to find the matching <see cref="IUniqueObject"/>. If 
        ///         you create a subclass of <see cref="UniqueObject"/> that contains other <see cref="UniqueObject"/>, 
        ///         you must reimplment <see cref="Members"/> to return these objects.
        ///     </note>
        /// </remarks>
        /// <seealso cref="Members"/>
        public UniqueObjectPath Find(Guid uid)
        {
            if (uid == this.Uid) { return new UniqueObjectPath(this); }


            var path = this.Members.SelectFirstOrDefault(
                uo => uo.Find(uid),
                uop => uop != UniqueObjectPath.Empty,
                UniqueObjectPath.Empty);

            if (path != UniqueObjectPath.Empty)
            {
                path = new UniqueObjectPath(this, path.Path);
            }

            return path;
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="UniqueObject"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }
        #endregion Public Methods

        #region Public Operators
        /// <summary>
        ///     Returns whether two instances of <see cref="UniqueObject"/> are equal.
        /// </summary>
        /// <param name="left">
        ///     A <see cref="UniqueObject"/>.
        /// </param>
        /// <param name="right">
        ///     A <see cref="UniqueObject"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public static bool operator ==(UniqueObject left, UniqueObject right)
        {
            if ((object)left == (object)right)
            {
                return true;
            }

            if ((object)left == null)
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        ///     Returns whether two instances of <see cref="UniqueObject"/> are not equal.
        /// </summary>
        /// <param name="left">
        ///     A <see cref="UniqueObject"/>.
        /// </param>
        /// <param name="right">
        ///     A <see cref="UniqueObject"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public static bool operator !=(UniqueObject left, UniqueObject right)
        {
            if ((object)left == (object)right)
            {
                return false;
            }

            if ((object)left == null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion Public Operators


        #region Protected Methods
        /// <summary>
        ///     Gets the internal graph of <see cref="IUniqueObject"/> instances as a sequence.
        /// </summary>
        /// <value>
        ///     An <see cref="IEnumerable{IUniqueObject}"/>.
        /// </value>
        /// <remarks>
        ///     <para>
        ///     <note type="inheritinfo">
        ///         <see cref="Find"/> uses <see cref="Members"/> to find the matching <see cref="IUniqueObject"/>. If 
        ///         you create a subclass of <see cref="UniqueObject"/> that contains other <see cref="UniqueObject"/>, 
        ///         you must reimplment <see cref="Members"/> to return these objects.
        ///     </note>
        ///     </para><para>
        ///     <note type="implementnotes">
        ///         <see cref="Members"/> must be idempotent: If the internal graph has not changed, or two 
        ///         <see cref="UniqueObject"/> instances have equal contents, the sequence returned must be equal.
        ///     </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="Find"/>
        protected virtual IEnumerable<IUniqueObject> Members
        {
            get
            {
                return new IUniqueObject[] { };
            }
        }
        #endregion Protected Methods


        #region Explicit Interface Implementations
        /// <summary>
        ///     Determines whether the specified <see cref="IUniqueObject"/> is equal to the current 
        ///     <see cref="IUniqueObject"/>.
        /// </summary>
        /// <param name="other">
        ///     <see cref="IUniqueObject"/> to compare with the current <see cref="IUniqueObject"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="IUniqueObject"/> is equal to the current 
        ///     <see cref="IUniqueObject"/>; otherwise, <see langword="false"/>.
        /// </returns>
        bool IEquatable<IUniqueObject>.Equals(IUniqueObject other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Uid == other.Uid;
        }
        #endregion Explicit Interface Implementations
    }

    /// <summary>
    ///     Wraps the specified type to implement <see cref="IEquatable{IUniqueObject}"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The external type to wrap.
    /// </typeparam>
    /// <remarks>
    ///     <see cref="UniqueObject"/> exposes the <see cref="Uid"/> property that ensures uniqueness. Identity is 
    ///     maintained by implementing <see cref="IEquatable{UniqueObject}"/> along with other equality 
    ///     inviariants, such as a reimplemneted <see cref="GetHashCode"/>. A <see cref="Find"/> method is provided to
    ///     enforce a convention of finding <see cref="UniqueObject"/> instances in a graph.
    /// </remarks>
    /// <seealso cref="Uid"/>
    /// <seealso cref="Find"/>
    /// <seealso cref="IEquatable{T}"/>
#if !NETFX_CORE
    [Serializable]
#endif
    public sealed class UniqueObject<T> 
        : IUniqueObject, IEquatable<UniqueObject<T>>, IEquatable<IUniqueObject>
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new <see cref="UniqueObject{T}"/> with default properties. <see cref="Uid"/> will be a 
        ///     random value and <see cref="Value"/> will be assigned the default value for <typeparamref name="T"/>.
        /// </summary>
        /// <seealso cref="Uid"/>
        /// <seealso cref="Value"/>
        public UniqueObject()
            : this(Guid.NewGuid(), default(T))
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="UniqueObject{T}"/> with the specified idenifier. <see cref="Value"/> will 
        ///     be assigned the default value for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="uid">
        ///     A <see cref="Guid"/>.
        /// </param>
        /// <remarks>
        ///     <note type="implementnotes">
        ///     <see cref="Uid"/> is assigned the value of <paramref name="uid"/>.
        ///     </note>
        /// </remarks>
        /// <seealso cref="Uid"/>
        public UniqueObject(Guid uid)
            : this(uid, default(T))
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="UniqueObject{T}"/> with the specified value. <see cref="Uid"/> will be a 
        ///     random value.
        /// </summary>
        /// <param name="value">
        ///     The wrapped value.
        /// </param>
        /// <remarks>
        ///     <note>
        ///         <see cref="Value"/> is assigned the value of <paramref name="value"/>.
        ///     </note>
        /// </remarks>
        public UniqueObject(T value)
            : this(Guid.NewGuid(), value)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="UniqueObject"/> with the specified <see cref="Guid"/> value.
        /// </summary>
        /// <param name="uid">
        ///     A <see cref="Guid"/>.
        /// </param>
        /// <param name="value">
        ///     The wrapped value.
        /// </param>
        /// <remarks>
        ///     <note>
        ///         <see cref="Uid"/> is assigned the value of <paramref name="uid"/> and <see cref="Value"/> is 
        ///         assigned the value of <paramref name="value"/>.
        ///     </note>
        /// </remarks>
        public UniqueObject(Guid uid, T value)
        {
            this.Uid = uid;
            this.Value = value;
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets and sets a unique identifier for the current <see cref="UniqueObject{T}"/>.
        /// </summary>
        /// <value>
        ///     A unique identifier for the current <see cref="UniqueObject{T}"/>.
        /// </value>
        [TransientKey]
        public Guid Uid { get; set; }

        /// <summary>
        ///     Gets and sets the wrapped value.
        /// </summary>
        /// <value>
        ///     The <typeparamref name="T"/> value that was wrapped.
        /// </value>
        public T Value { get; set; }
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        ///     Determines whether the specified <see cref="UniqueObject{T}"/> is equal to the current 
        ///     <see cref="UniqueObject{T}"/>.
        /// </summary>
        /// <param name="other">
        ///     <see cref="UniqueObject{T}"/> to compare with the current <see cref="UniqueObject{T}"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="UniqueObject{T}"/> is equal to the current 
        ///     <see cref="UniqueObject{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(UniqueObject<T> other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Uid == other.Uid;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        ///     <see cref="object"/> to compare with the current <see cref="object"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="object"/> is equal to the current <see cref="object"/>;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is UniqueObject<T>)
            {
                return Equals((UniqueObject<T>)obj);
            }

            return base.Equals(obj);
        }

        /// <summary>
        ///     Returns a <see cref="UniqueObjectPath"/> representing the path to the current 
        ///     <see cref="UniqueObject{T}"/> or <see cref="UniqueObjectPath.Empty">UniqueObjectPath.Empty</see>. 
        /// </summary>
        /// <param name="uid">
        ///     The <see cref="Guid"/> idenfiying the <see cref="IUniqueObject"/> in the current 
        ///     <see cref="UniqueObject"/>.
        /// </param>
        /// <returns>
        ///     a <see cref="UniqueObjectPath"/> representing the path to the current 
        ///     <see cref="UniqueObject{T}"/> or <see cref="UniqueObjectPath.Empty">UniqueObjectPath.Empty</see>. 
        /// </returns>
        /// <remarks>
        ///     Since <see cref="Value"/> is not an implementation of <see cref="IUniqueObject"/>, only the current 
        ///     <see cref="UniqueObject{T}"/> can be found.
        /// </remarks>
        public UniqueObjectPath Find(Guid uid)
        {
            return new UniqueObjectPath((uid == this.Uid) ? this : (UniqueObject<T>)null);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="UniqueObject{T}"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }
        #endregion Public Methods

        #region Public Operators
        /// <summary>
        ///     Wraps a <typeparamref name="T"/> value with an <see cref="UniqueObject{T}"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to wrap.
        /// </param>
        /// <returns>
        ///     An <see cref="UniqueObject{T}"/>.
        /// </returns>
        public static implicit operator UniqueObject<T>(T value)
        {
            return new UniqueObject<T>(value);
        }

        /// <summary>
        ///     Unwraps a <typeparamref name="T"/> value from an <see cref="UniqueObject{T}"/>.
        /// </summary>
        /// <param name="obj">
        ///     A <see cref="UniqueObject{T}"/> wrapper.
        /// </param>
        /// <returns>
        ///     A <typeparamref name="T"/>.
        /// </returns>
        public static implicit operator T(UniqueObject<T> obj)
        {
            if ((object)obj == null)
            {
                return default(T);
            }

            return obj.Value;
        }

        /// <summary>
        ///     Returns whether two instances of <see cref="UniqueObject{T}"/> are equal.
        /// </summary>
        /// <param name="left">
        ///     A <see cref="UniqueObject{T}"/>.
        /// </param>
        /// <param name="right">
        ///     A <see cref="UniqueObject{T}"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public static bool operator ==(UniqueObject<T> left, UniqueObject<T> right)
        {
            if ((object)left == (object)right)
            {
                return true;
            }

            if ((object)left == null)
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        ///     Returns whether two instances of <see cref="UniqueObject{T}"/> are not equal.
        /// </summary>
        /// <param name="left">
        ///     A <see cref="UniqueObject{T}"/>.
        /// </param>
        /// <param name="right">
        ///     A <see cref="UniqueObject{T}"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public static bool operator !=(UniqueObject<T> left, UniqueObject<T> right)
        {
            if ((object)left == (object)right)
            {
                return false;
            }

            if ((object)left == null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion Public Operators


        #region Explicit Interface Implementations
        /// <summary>
        ///     Determines whether the specified <see cref="IUniqueObject"/> is equal to the current 
        ///     <see cref="IUniqueObject"/>.
        /// </summary>
        /// <param name="other">
        ///     <see cref="IUniqueObject"/> to compare with the current <see cref="IUniqueObject"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="IUniqueObject"/> is equal to the current 
        ///     <see cref="IUniqueObject"/>; otherwise, <see langword="false"/>.
        /// </returns>
        bool IEquatable<IUniqueObject>.Equals(IUniqueObject other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Uid == other.Uid;
        }
        #endregion Explicit Interface Implementations
    }
}
