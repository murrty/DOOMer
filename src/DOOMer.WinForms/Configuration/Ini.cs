namespace DOOMer.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal static class Ini {
    internal static bool TryReadKey(string ini, string section, string key, [NotNullWhen(true)] out string? value) {
        const int length = 4097;
        const string EmptyString = "{empty_key_value}";

        char[] ReadValue = new char[length];
        int ReadChars = NativeMethods.GetPrivateProfileStringW(section, key, EmptyString, ReadValue, length, ini);
        value = new(ReadValue, 0, ReadChars);

        if (string.IsNullOrWhiteSpace(value)) {
            value = null;
            return false;
        }

        if (string.Equals(value, EmptyString, StringComparison.OrdinalIgnoreCase)) {
            value = null;
            return false;
        }

        return true;
    }
    internal static bool WriteKey(string ini, string section, string key, string value) {
        return NativeMethods.WritePrivateProfileStringW(section, key, value, ini) == 1;
    }
    internal static int[] ReadIntArray(string ini, string section, string key) {
        if (!TryReadKey(ini, section, key, out string? val)) {
            return [];
        }
        string[] splits = val.Split(',');
        List<int> ints = [];
        if (splits.Length > 1) {
            foreach (var split in splits) {
                if (!int.TryParse(split, out int x)) {
                    return [];
                }
                ints.Add(x);
            }
        }
        return ints.Count > 0 ?[..ints] : [];
    }
}
