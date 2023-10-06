using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Bearz.Extra.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Determines whether the array contains the specified item.
    /// </summary>
    /// <param name="array">The array to check.</param>
    /// <param name="item">The item.</param>
    /// <typeparam name="T">The element type.</typeparam>
    /// <returns>Returns <see langword="true" /> when the item is found in the array; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this T[] array, T item)
        => Array.IndexOf(array, item) != -1;
}