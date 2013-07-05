using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Lw
{
    [global::System.AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = true)]
    public sealed class EquivalentEnumValueSelectorAttribute : Attribute
    {
        public EquivalentEnumValueSelectorAttribute(string enumValue, string selectedEquivalentValue)
        {
            this.EnumValue = enumValue;
            this.SelectedEquivalentValue = selectedEquivalentValue;
        }

        public string SelectName(Enum value)
        {
            return SelectName(value.GetType(), value.GetName());
        }

        public string SelectName<TEnum>(string enumValue)
            where TEnum : struct
        {
            EnumOperations.VerifyEnumType(typeof(TEnum), "TEnum");

            return SelectName(typeof(TEnum), enumValue);
        }

        public static string SelectName(Type enumType, string enumValue)
        {
            Contract.Requires<ArgumentNullException>(enumValue != null, "enumValue");

            EnumOperations.VerifyEnumType(enumType, () => enumType);

            var attributes = enumType.GetTypeInfo().GetCustomAttributes<EquivalentEnumValueSelectorAttribute>(false);

            string selectedValue;

            if (attributes.Count() == 0)
            {
                selectedValue = enumValue;
            }
            else
            {
                selectedValue = attributes
                    .SingleOrDefault(eevsa => eevsa.EnumValue.EqualsOrdinal(enumValue))
                    .GetValueOrDefault(eevsa => eevsa.SelectedEquivalentValue, enumValue);
            }

            string[] names;

            names = Enum.GetNames(enumType);

            names.Cast<string>()
                .SingleOrDefault(s => s.EqualsOrdinal(selectedValue))
                .ThrowIfNull(() => new InvalidOperationException());

            return selectedValue;
        }

        public string EnumValue { get; private set; }
        public string SelectedEquivalentValue { get; private set; }
    }
}
