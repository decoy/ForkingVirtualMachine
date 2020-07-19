namespace ForkingVirtualMachine.Utility
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static Dictionary<TKey[], TValue> Fork<TKey, TValue>(this Dictionary<TKey[], TValue> dictionary)
        {
            if (dictionary == null)
            {
                return new Dictionary<TKey[], TValue>(new ArrayEqualityComparer<TKey>());
            }

            return new Dictionary<TKey[], TValue>(dictionary, new ArrayEqualityComparer<TKey>());
        }

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
