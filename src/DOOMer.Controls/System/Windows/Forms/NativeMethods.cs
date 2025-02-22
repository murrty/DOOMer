//----------------------
// <auto-generated>
//     Backwards compatibility with Menus.
// </auto-generated>
//----------------------

namespace System.Windows.Forms.Natives;

#if NETCOREAPP3_1_OR_GREATER && ENABLE_LEGACY_MENUS
using System.Runtime.InteropServices;

// System.Windows.Forms.NativeMethods
internal static partial class NativeMethods {
    public struct RECT {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public System.Drawing.Size Size => new(right - left, bottom - top);

        public RECT(int left, int top, int right, int bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) {
            left = r.Left;
            top = r.Top;
            right = r.Right;
            bottom = r.Bottom;
        }
        public static RECT FromXYWH(int x, int y, int width, int height) {
            return new RECT(x, y, x + width, y + height);
        }
    }

    public struct NMHDR {
        public IntPtr hwndFrom;
        public IntPtr idFrom;
        public int code;
        public NMHDR(IntPtr hwndFrom, IntPtr idFrom, int code) {
            this.hwndFrom = hwndFrom;
            this.idFrom = idFrom;
            this.code = code;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TOOLINFO_T {
        public int cbSize = Marshal.SizeOf(typeof(TOOLINFO_T));
        public int uFlags;
        public IntPtr hwnd;
        public IntPtr uId;
        public RECT rect;
        public IntPtr hinst = IntPtr.Zero;
        public string lpszText;
        public IntPtr lParam = IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MENUINFO_T {
        public int cbSize = Marshal.SizeOf(typeof(MENUINFO_T));
        public int fMask;
        public int dwStyle;
        public uint cyMax;
        public nint hbrBack;
        public int dwContextHelpID;
        public nint dwMenuData;

        public MENUINFO_T() { }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MENUITEMINFO_T {
        public int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO_T));
        public int fMask;
        public int fType;
        public int fState;
        public int wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        public string dwTypeData;
        public int cch;
        public IntPtr hbmpItem;

        public MENUITEMINFO_T() { }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MENUITEMINFO_T_RW {
        public int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO_T_RW));
        public int fMask;
        public int fType;
        public int fState;
        public int wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        public IntPtr dwTypeData;
        public int cch;
        public IntPtr hbmpItem;

        public MENUITEMINFO_T_RW() { }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MSAAMENUINFO {
        public int dwMSAASignature;
        public int cchWText;
        public string pszWText;

        public MSAAMENUINFO(string text) {
            dwMSAASignature = -1441927155;
            cchWText = text.Length;
            pszWText = text;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CHARRANGE {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class ENPROTECTED {
        public NMHDR nmhdr;
        public int msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public CHARRANGE chrg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class INITCOMMONCONTROLSEX {
        public int dwSize = 8;
        public int dwICC;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DRAWITEMSTRUCT {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemAction;
        public int itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public RECT rcItem;
        public IntPtr itemData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MEASUREITEMSTRUCT {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemWidth;
        public int itemHeight;
        public IntPtr itemData;
    }

    public static class Util {
        public static int MAKELONG(int low, int high) {
            return (high << 16) | (low & 0xFFFF);
        }
        public static IntPtr MAKELPARAM(int low, int high) {
            return (IntPtr)((high << 16) | (low & 0xFFFF));
        }
        public static int HIWORD(int n) {
            return (n >> 16) & 0xFFFF;
        }
        public static int HIWORD(IntPtr n) {
            return HIWORD((int)(long)n);
        }
        public static int LOWORD(int n) {
            return n & 0xFFFF;
        }
        public static int LOWORD(IntPtr n) {
            return LOWORD((int)(long)n);
        }
        public static int SignedHIWORD(IntPtr n) {
            return SignedHIWORD((int)(long)n);
        }
        public static int SignedLOWORD(IntPtr n) {
            return SignedLOWORD((int)(long)n);
        }
        public static int SignedHIWORD(int n) {
            return (short)((n >> 16) & 0xFFFF);
        }
        public static int SignedLOWORD(int n) {
            return (short)(n & 0xFFFF);
        }
    }

    public sealed class CommonHandles {
        public static readonly int GDI;
        public static readonly int HDC;
        public static readonly int Menu;

        static CommonHandles() {
            GDI = System.Internal.HandleCollector.RegisterType("GDI", 50, 500);
            HDC = System.Internal.HandleCollector.RegisterType("HDC", 100, 2);
            Menu = System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);
        }
    }
    public class ActiveX {
        public const int DISPID_BORDERSTYLE = -504;
    }

    public const int ICC_BAR_CLASSES = 4;
    public const int CCS_NORESIZE = 4;
    public const int CCS_NOPARENTALIGN = 8;
    public const int SBARS_SIZEGRIP = 256;
    public const int SB_SETTEXTA = 1025;
    public const int SB_SETTEXTW = 1035;
    public const int SB_GETTEXTA = 1026;
    public const int SB_GETTEXTW = 1037;
    public const int SB_GETTEXTLENGTHA = 1027;
    public const int SB_GETTEXTLENGTHW = 1036;
    public const int SB_SETPARTS = 1028;
    public const int SB_SIMPLE = 1033;
    public const int SB_GETRECT = 1034;
    public const int SB_SETICON = 1039;
    public const int SB_SETTIPTEXTA = 1040;
    public const int SB_SETTIPTEXTW = 1041;
    public const int SB_GETTIPTEXTA = 1042;
    public const int SB_GETTIPTEXTW = 1043;
    public const int SBT_OWNERDRAW = 4096;
    public const int SBT_NOBORDERS = 256;
    public const int SBT_POPOUT = 512;
    public const int SBT_RTLREADING = 1024;
    public const int HTCLIENT = 1;
    public const int WM_NCHITTEST = 132;
    public const int WM_REFLECT = 8192;
    public const int WM_DRAWITEM = 43;
    public const int WM_NOTIFY = 78;
    public const int NM_CLICK = -2;
    public const int NM_DBLCLK = -3;
    public const int NM_RCLICK = -5;
    public const int NM_RDBLCLK = -6;
    public const int ICC_TAB_CLASSES = 8;
    public const int TTS_ALWAYSTIP = 1;
    public const int SWP_NOSIZE = 1;
    public const int SWP_NOMOVE = 2;
    public const int SWP_NOACTIVATE = 16;
    public const int TTM_SETMAXTIPWIDTH = 1048;
    public const int WM_SETFOCUS = 7;
    public const int TTF_SUBCLASS = 16;
    public const int TTF_TRANSPARENT = 256;
    public const int TTF_RTLREADING = 4;

    public const string WC_STATUSBAR = "msctls_statusbar32";
    public const string TOOLTIPS_CLASS = "tooltips_class32";

    public static HandleRef NullHandleRef;
    public static HandleRef HWND_TOPMOST;

    public static readonly int SB_SETTEXT;
    public static readonly int SB_GETTEXT;
    public static readonly int SB_GETTEXTLENGTH;
    public static readonly int SB_SETTIPTEXT;
    public static readonly int SB_GETTIPTEXT;
    public static readonly int TTM_DELTOOL;
    public static readonly int TTM_SETTOOLINFO;
    public static readonly int TTM_ADDTOOL;

    static NativeMethods() {
        NullHandleRef = new HandleRef(null, IntPtr.Zero);
        HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));

        if (Marshal.SystemDefaultCharSize == 1) {
            SB_SETTEXT = 1025;
            SB_GETTEXT = 1026;
            SB_GETTEXTLENGTH = 1027;
            SB_SETTIPTEXT = 1040;
            SB_GETTIPTEXT = 1042;
            TTM_ADDTOOL = 1028;
            TTM_DELTOOL = 1029;
            TTM_SETTOOLINFO = 1033;
        }
        else {
            SB_SETTEXT = 1035;
            SB_GETTEXT = 1037;
            SB_GETTEXTLENGTH = 1036;
            SB_SETTIPTEXT = 1041;
            SB_GETTIPTEXT = 1043;
            TTM_ADDTOOL = 1074;
            TTM_DELTOOL = 1075;
            TTM_SETTOOLINFO = 1078;
        }
    }
}
#endif
