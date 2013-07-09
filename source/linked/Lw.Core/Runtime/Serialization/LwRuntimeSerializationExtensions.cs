using System.Runtime.Serialization;

namespace Lw.Runtime.Serialization
{
    public static class Extensions
    {
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }

        public static void AddValue<T>(this SerializationInfo info, string name, T value)
        {
            info.AddValue(name, value, typeof(T));
        }
    }
}
