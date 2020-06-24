namespace ForkingVirtualMachine
{
    using System.Collections.Generic;

    public static class IEnumeratorExtensions
    {
        public static T Next<T>(this IEnumerator<T> enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }

        public static bool TryNext<T>(this IEnumerator<T> enumerator, out T result)
        {
            if (enumerator.MoveNext())
            {
                result = enumerator.Current;
                return true;
            }

            result = default;
            return false;
        }
    }
}
