namespace Bearz.Extra.Arrays;

public static partial class ArrayExtensions
{
    public static void Sort<T>(this T[] array)
        => Array.Sort(array);

    public static void Sort<T>(this T[] array, IComparer<T>? comparer)
        => Array.Sort(array, comparer);

    public static void Sort<T>(this T[] array, int index)
        => Array.Sort(array, index, array.Length - index);

    public static void Sort<T>(this T[] array, int index, int length, IComparer<T>? comparer = null)
        => Array.Sort(array, index, length, comparer);
}