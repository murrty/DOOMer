#nullable enable
namespace DOOMer.Controls;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct NCCALCSIZE_PARAMS {
    public RECT rgrc0, rgrc1, rgrc2;
    [MarshalAs(UnmanagedType.SysInt)]
    public nint lppos;
}
