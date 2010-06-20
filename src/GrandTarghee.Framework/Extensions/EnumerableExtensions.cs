using System;
using System.Collections.Generic;

namespace GrandTarghee.Framework
{
    public static class EnumerableExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "The action cannot be null.");
            }

            foreach (var item in list)
            {
                action(item);
            }
        }

        #endregion
    }
}
