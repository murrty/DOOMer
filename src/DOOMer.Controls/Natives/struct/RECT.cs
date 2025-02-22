#nullable enable
namespace DOOMer.Controls;

using System.Drawing;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct RECT {
    public static readonly RECT Empty = new(0, 0, 0, 0);

    [MarshalAs(UnmanagedType.I4)]
    public int left;
    [MarshalAs(UnmanagedType.I4)]
    public int top;
    [MarshalAs(UnmanagedType.I4)]
    public int right;
    [MarshalAs(UnmanagedType.I4)]
    public int bottom;

    public RECT(int left, int top, int right, int bottom) {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }

    public static implicit operator Rectangle(RECT rect) => new(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
}
