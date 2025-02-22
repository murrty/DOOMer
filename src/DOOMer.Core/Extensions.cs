namespace DOOMer.Core;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

[DebuggerStepThrough, DebuggerNonUserCode]
public static class Extensions {
    public static bool IsNullEmptyWhitespace([NotNullWhen(false)] this string? str) {
        if (str is null) {
            return true;
        }

        if (str.Length < 1) {
            return true;
        }

        for (int i = 0; i < str.Length; i++) {
            if (!char.IsWhiteSpace(str[i])) {
                return false;
            }
        }

        return true;
    }
    public static string UnlessNullEmptyWhitespace(this string? str, string other) {
        return str.IsNullEmptyWhitespace() ? other : str;
    }
    public static string SubstringBeforeLastChar(this string str, char ch) {
        for (int i = str.Length - 1; i > -1; i--) {
            if (str[i] == ch) {
                if (i == str.Length - 1) {
                    return str;
                }
                return str[..i];
            }
        }
        return str;
    }
    public static string TrimStartWhitespace(this string str, params char[] chs) {
        if (str?.Length is not > 0) {
            return string.Empty;
        }

        bool valid(char ch) {
            if (char.IsWhiteSpace(ch)) {
                return false;
            }
            return !chs.Any(x => x == ch);
        }

        int index = -1;
        while (valid(str[++index]));
        return index > 0 && index < str.Length - 1 ? str[(index + 1)..] : str;
    }
    public static string TrimEndWhitespace(this string str, params char[] chs) {
        if (str?.Length is not > 0) {
            return string.Empty;
        }

        bool valid(char ch) {
            if (char.IsWhiteSpace(ch)) {
                return false;
            }
            return !chs.Any(x => x == ch);
        }

        int index = str.Length;
        while (valid(str[--index]));
        return index < str.Length - 1 && index > 0 ? str[..index] : str;
    }
    public static bool PathExists(this FileSystemInfo file) {
        file.Refresh();
        return file.Exists;
    }
    public static int Clamp(this int i, int min, int max) {
        if (i < min) {
            return min;
        }
        if (i > max) {
            return max;
        }
        return i;
    }
    public static uint Clamp(this uint i, uint min, uint max) {
        if (i < min) {
            return min;
        }
        if (i > max) {
            return max;
        }
        return i;
    }
    public static bool IsPathRelativeCompatible([NotNullWhen(true)] this string? pathString) {
        if (pathString.IsNullEmptyWhitespace() || Environment.CurrentDirectory.Length < 1) {
            return false;
        }

        if (pathString.Length <= Environment.CurrentDirectory.Length + 1) {
            return false;
        }

        if (!pathString.StartsWith(Environment.CurrentDirectory, StringComparison.OrdinalIgnoreCase)) {
            return false;
        }

        for (int i = 0; i < Environment.CurrentDirectory.Length; i++) {
            char ch = Environment.CurrentDirectory[i];

            if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar) {
                ch = pathString[i];
                if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar) {
                    continue;
                }
                return false;
            }

            if (char.ToUpperInvariant(ch) != char.ToUpperInvariant(pathString[i])) {
                return false;
            }
        }

        char ch2 = pathString[Environment.CurrentDirectory.Length];
        return ch2 == Path.DirectorySeparatorChar || ch2 == Path.AltDirectorySeparatorChar;
    }
    public static T? FirstOrDefaultCast<T>(this IEnumerable enumerable, Predicate<T> predicate) {
        foreach (var item in enumerable) {
            if (item is not T casted || !predicate(casted)) {
                continue;
            }
            return casted;
        }
        return default;
    }
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
        foreach (T item in enumerable) {
            action(item);
        }
    }
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action) {
        int index = 0;
        foreach (T item in enumerable) {
            action(item, index++);
        }
    }
    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action) {
        foreach (var item in dictionary) {
            action(item.Key, item.Value);
        }
    }
    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue, int> action) {
        int index = 0;
        foreach (var item in dictionary) {
            action(item.Key, item.Value, index++);
        }
    }
    public static void For<T>(this T[] array, Action<T> action) {
        for (int i = 0; i < array.Length; i++) {
            action(array[i]);
        }
    }
    public static void For<T>(this T[] array, Action<T, int> action) {
        for (int i = 0; i < array.Length; i++) {
            action(array[i], i);
        }
    }
    public static void For<T>(this IReadOnlyList<T> collection, Action<T> action) {
        for (int i = 0; i < collection.Count; i++) {
            action(collection[i]);
        }
    }
    public static void For<T>(this IReadOnlyList<T> collection, Action<T, int> action) {
        for (int i = 0; i < collection.Count; i++) {
            action(collection[i], i);
        }
    }
    public static void WhereForEach<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, Action<T> action) {
        foreach (T item in enumerable) {
            if (!predicate(item)) {
                continue;
            }
            action(item);
        }
    }
    public static void WhereForEach<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, Action<T, int> action) {
        int index = 0;
        foreach (T item in enumerable) {
            if (!predicate(item)) {
                continue;
            }
            action(item, index++);
        }
    }
    public static void WhereFor<T>(this T[] array, Predicate<T> predicate, Action<T> action) {
        for (int i = 0; i < array.Length; i++) {
            T item = array[i];
            if (!predicate(item)) {
                continue;
            }
            action(item);
        }
    }
    public static void WhereFor<T>(this T[] array, Predicate<T> predicate, Action<T, int> action) {
        for (int i = 0; i < array.Length; i++) {
            T item = array[i];
            if (!predicate(item)) {
                continue;
            }
            action(item, i);
        }
    }
    public static void WhereFor<T>(this IReadOnlyList<T> list, Predicate<T> predicate, Action<T> action) {
        for (int i = 0; i < list.Count; i++) {
            T item = list[i];
            if (!predicate(item)) {
                continue;
            }
            action(item);
        }
    }
    public static void WhereFor<T>(this IReadOnlyList<T> list, Predicate<T> predicate, Action<T, int> action) {
        for (int i = 0; i < list.Count; i++) {
            T item = list[i];
            if (!predicate(item)) {
                continue;
            }
            action(item, i);
        }
    }
}
