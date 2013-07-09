using Lw.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;


namespace Lw.Dynamic
{

    /// <summary>
    ///     Currently only supports properties.
    /// </summary>
    public class DynamicInstance : DynamicObject
    {
        public void AddProperty<T>(string name)
        {
            AddProperty(name, default(T));
        }
        public void AddProperty<T>(string name, T defaultValue)
        {
            T property = defaultValue;
            AddProperty(name, () => property, t => property = t);
        }
        public void AddProperty<T>(string name, Func<T> getter, Action<T> setter)
        {
            this.propertyDescriptions[name] = new PropertyMetadata
            {
                Getter = ((Func<object>)(()=>getter())),
                Name = name,
                PropertyType = typeof(T),
                Setter = ((Action<object>)((object o)=>setter((T)o))),
            };
        }

        public IDictionary<string, object> GetProperties()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.AddRange(this.propertyDescriptions.Select(kvp=>Tuple.Create(kvp.Key, kvp.Value.Getter())));

            return dictionary;
        }

        public bool RemoveProperty<T>(string name)
        {
            return this.propertyDescriptions.Remove(name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var metadata = this.propertyDescriptions[binder.Name];

            if (metadata == null || !binder.ReturnType.IsAssignableFrom(metadata.PropertyType))
            {
                return false;
            }

            result = metadata.Getter();
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var metadata = this.propertyDescriptions[binder.Name];

            bool propertyCreated = false;
            if (metadata == null)
            {
                propertyCreated = CreateProperty(binder.ReturnType, binder.Name);

                if (!propertyCreated) { return false; }

                metadata = this.propertyDescriptions[binder.Name];

                if (metadata == null)
                {
                    // TODO: DynamicInstance.TrySetMember -- Robust error messages.
                    throw new InvalidOperationException(
                        "CreateProperty did not create a property for Type '{0}' and name '{1}'."
                        .DoFormat(binder.ReturnType, binder.Name));
                }
            }

            if (!metadata.PropertyType.IsAssignableFrom(binder.ReturnType))
            {
                if (propertyCreated)
                {
                    throw new InvalidOperationException(
                        "CreateProperty did not create a property assignable from Type '{0}' for name '{1}'. Type was '{2}'"
                        .DoFormat(binder.ReturnType, binder.Name, metadata.PropertyType));
                }

                return false;
            }

            metadata.Setter(value);
            return true;
        }


        protected virtual bool CreateProperty(Type propertyType, string name)
        {
            return false;
        }

        private class PropertyMetadata
        {
            public Type PropertyType { get; set;}
            public string Name { get; set; }
            public Func<object> Getter { get; set; }
            public Action<object> Setter { get; set; }
        }

        private readonly Dictionary<string, PropertyMetadata> propertyDescriptions = 
            new Dictionary<string, PropertyMetadata>();
    }
}
