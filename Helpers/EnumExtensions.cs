using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the name of the constant in the specified enumeration that has the specified value. 
        /// </summary>
        /// <typeparam name="TEnum">An enumaration type.</typeparam>
        /// <param name="value">The value of a particular enumerated constant in terms of its underlying type.</param>
        /// <returns>
        /// A string containing the name of the enumerated constant in enumType whose value
        /// is value; or null if no such constant is found.
        /// </returns>
        public static string GetName<TEnum>(this TEnum value)
        {
            return Enum.GetName(typeof(TEnum), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumarated object.
        /// This extension method is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type TEnum whose value is represented by value.</returns>
        public static TEnum ParseEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }
    }
}
