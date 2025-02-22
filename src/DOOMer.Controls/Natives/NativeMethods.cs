#nullable enable
namespace DOOMer.Controls;

using System;
using System.Runtime.InteropServices;
using static EntryDll;

/// <summary>
///     Entry point DLL files.
/// </summary>
file static class EntryDll {
    internal const string Uxtheme = "uxtheme.dll";
    internal const string User32 = "user32.dll";
    internal const string Gdi32 = "gdi32.dll";
}

/// <summary>
///     Natively called methods.
/// </summary>
internal static partial class NativeMethods {
    [LibraryImport(Uxtheme, EntryPoint = "SetWindowTheme", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int SetWindowTheme(nint hwnd, string pszSubAppName, string? pszSubIdList);

    [LibraryImport(User32, EntryPoint = "SendMessageA", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int SendMessage(nint hWnd, nint wMsg, nint wParam, nint lParam);

    [DllImport(User32, EntryPoint = "SendMessageA", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054")]
    internal static extern int SendMessage(nint hWnd, nint wMsg, nint wParam, TV_ITEM lParam);

    [LibraryImport(User32, EntryPoint = "SendMessageW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int SendMessageW(nint hWnd, nint wMsg, nint wParam, nint lParam);

    [DllImport(User32, EntryPoint = "SendMessageW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054")]
    internal static extern int SendMessageW(nint hWnd, nint wMsg, nint wParam, TV_ITEM lParam);

    [LibraryImport(User32, EntryPoint = "GetWindowDC", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.SysInt)]
    internal static partial nint GetWindowDC(nint hWnd);

    [LibraryImport(User32, EntryPoint = "ReleaseDC", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ReleaseDC(nint hWnd, nint hDC);

    [LibraryImport(User32, EntryPoint = "GetWindowRect")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetWindowRect(nint hWnd, ref RECT lpRect);

    [LibraryImport(Gdi32, EntryPoint = "ExcludeClipRect")]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int ExcludeClipRect(nint hdc, int nLeftrect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool InitCommonControls();

    [LibraryImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ImageList_BeginDrag(
        IntPtr himlTrack, // Handler of the image list containing the image to drag
        int iTrack,       // Index of the image to drag 
        int dxHotspot,    // x-delta between mouse position and drag image
        int dyHotspot     // y-delta between mouse position and drag image
    );

    [LibraryImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ImageList_DragMove(
        int x,   // X-coordinate (relative to the form,
                 // not the treeview) at which to display the drag image.
        int y    // Y-coordinate (relative to the form,
                 // not the treeview) at which to display the drag image.
    );

    [LibraryImport("comctl32.dll")]
    public static partial void ImageList_EndDrag();

    [LibraryImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ImageList_DragEnter(
        nint hwndLock,  // Handle to the control that owns the drag image.
        int x,            // X-coordinate (relative to the treeview)
                          // at which to display the drag image. 
        int y             // Y-coordinate (relative to the treeview)
                          // at which to display the drag image. 

    );

    [LibraryImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ImageList_DragLeave(
        nint hwndLock  // Handle to the control that owns the drag image.
    );

    [LibraryImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ImageList_DragShowNolock(
        [MarshalAs(UnmanagedType.Bool)]
        bool fShow       // False to hide, true to show the image
    );

    /// <summary>
    ///     Constant integer values.
    /// </summary>
    internal static class Constants {
        /// <summary>
        /// Sent when the size and position of a window's client area must be calculated.
        /// By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
        /// </summary>
        internal const int WM_NCCALCSIZE = 0x0083;
        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame must be painted.
        /// </summary>
        internal const int WM_NCPAINT = 0x0085;
        /// <summary>
        /// Sent to the control when the windows theme changes.
        /// </summary>
        internal const int WM_THEMECHANGED = 0x31A;

        internal const int EM_SETCUEBANNER = 0x1501;
        internal const int CB_SETCUEBANNER = 0x1703;

        internal const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        internal const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        internal const int TVS_EX_DOUBLEBUFFER = 0x4;
        internal const int TVS_EX_PARTIALCHECKBOXES = 0x80;
    }
}
