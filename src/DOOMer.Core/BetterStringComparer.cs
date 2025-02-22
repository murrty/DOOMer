namespace DOOMer.Core;

using System.Collections.Generic;
using static BetterStringComparerHelper;

public sealed class BetterStringComparer : IComparer<string> {
    public static BetterStringComparer Default { get; } = new();
    public int Compare(string? x, string? y) {
        x ??= string.Empty;
        y ??= string.Empty;

        if (EqualsNumbered(x, y, out int nIndex)) {
            if (nIndex == 0) {
                if (x.Length != y.Length) {
                    return x.Length - y.Length;
                }
            }
            else if (nIndex > 0) {
                x = x[nIndex..];
                y = y[nIndex..];

                if (x.Length != y.Length) {
                    return x.Length - y.Length;
                }

                return string.Compare(x, y);
            }
        }

        //if (IsNumeric(x) && IsNumeric(y) && x.Length != y.Length) {
        //    return x.Length - y.Length;
        //}

        return string.Compare(x, y);
    }
}

public sealed class BetterStringNullableComparer : IComparer<string?> {
    public static BetterStringNullableComparer Default { get; } = new();
    public int Compare(string? x, string? y) {
        x ??= string.Empty;
        y ??= string.Empty;

        if (EqualsNumbered(x, y, out int nIndex)) {
            if (nIndex == 0) {
                if (x.Length != y.Length) {
                    return x.Length - y.Length;
                }
            }
            else if (nIndex > 0) {
                x = x[nIndex..];
                y = y[nIndex..];

                if (x.Length != y.Length) {
                    return x.Length - y.Length;
                }

                return string.Compare(x, y);
            }
        }

        //if (IsNumeric(x) && IsNumeric(y) && x.Length != y.Length) {
        //    return x.Length - y.Length;
        //}

        return string.Compare(x, y);
    }
}

file static class BetterStringComparerHelper {
    public static bool IsNumeric(string txt) {
        if (txt.Length < 1) {
            return false;
        }

        for (int i = 0; i < txt.Length; i++) {
            if (!char.IsDigit(txt[i])) {
                return false;
            }
        }

        return true;
    }
    public static bool StartsWithThe(string txt) {
        // Lazy.
        return txt.Length > 3 && txt.StartsWith("the", System.StringComparison.InvariantCultureIgnoreCase) && !char.IsLetter(txt[3]);
    }
    public static bool StartsWithA(string txt) {
        // So lazy.
        return txt.Length > 1 && txt[0] is 'a' or 'A' && !char.IsLetter(txt[1]);
    }

    public static bool EqualsNumbered(string txt, string txt2, out int numberIndex) {
        for (int i = 0; i < txt.Length && i < txt2.Length; i++) {
            if (char.IsNumber(txt[i])) {
                if (char.IsNumber(txt2[i])) {
                    numberIndex = i;
                    return true;
                }
                break;
            }
            else if (char.IsNumber(txt2[i])) {
                break;
            }

            if (char.ToLower(txt[i]) == char.ToLower(txt2[i])) {
                continue;
            }

            break;
        }

        numberIndex = -1;
        return false;
    }
}
