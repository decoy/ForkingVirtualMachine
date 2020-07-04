using System;
using System.Collections.Generic;
using System.Text;

namespace ForkingVirtualMachine.Store.Utility
{
    public static class DictionaryExtensions
    {
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}
