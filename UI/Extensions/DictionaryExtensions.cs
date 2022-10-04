﻿#region

using System.Collections.Generic;

#endregion

namespace UI.Extensions
{
    public static class DictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> instance, string key)
        {
            return instance.ContainsKey(key) ? (T) instance[key] : default(T);
        }

        public static void Set<T>(this IDictionary<string, object> instance, string key, T value)
        {
            instance[key] = value;
        }
    }
}