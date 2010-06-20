using System;
using System.Collections.Generic;
using System.Linq;

namespace GrandTarghee.Framework
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string enumValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception(string.Format("T needs to be an enum type not {0}.", typeof(T).ToString()));
            }

            return (T)Enum.Parse(typeof(T), enumValue, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IList<object> GetEnumValues(this Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new Exception(string.Format("Type needs to be an enum type not {0}.", enumType.ToString()));
            }

            return enumType.GetFields()
                    .Where(x => x.IsLiteral)
                    .Select(x => x.GetValue(null))
                    .ToList();
        }
    }
}
