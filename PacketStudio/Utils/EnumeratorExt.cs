using System.Collections;

namespace PacketStudio.Utils
{
    internal static class EnumeratorExt
    {
        internal static IEnumerable Iterate(this IEnumerator iterator)
        {
            while (iterator.MoveNext())
                yield return iterator.Current;
        }
    }
}