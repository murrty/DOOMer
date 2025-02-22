#nullable enable
namespace DOOMer.WinForms;

using System.Runtime.InteropServices;
using static EntryDll;

/// <summary>
///     Entry point DLL files.
/// </summary>
file static class EntryDll {
    internal const string Kernel32 = "kernel32.dll";
}

/// <summary>
///     Natively called methods.
/// </summary>
internal static partial class NativeMethods {
    [LibraryImport(Kernel32, EntryPoint = "GetPrivateProfileStringW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int GetPrivateProfileStringW(string lpAppName, string lpKeyName, string lpDefault, [Out] char[] lpReturnedString, int nSize, string lpFileName);

    [LibraryImport(Kernel32, EntryPoint = "WritePrivateProfileStringW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int WritePrivateProfileStringW(string lpAppName, string lpKeyName, string lpString, string lpFileName);
}
