namespace DOOMer.WinForms;

using System.Windows.Forms;
using DOOMer.Core;

internal static class Extensions {
    internal static void ScanExternalFile(this LoadedExternal external) {
        if (!external.IsFile || !external.FileExists || external.File.Length < 5) {
            return;
        }
        using frmPackageExplorer explorer = new(external.File);
        explorer.ShowDialog();
    }
    internal static void ScanIWAD(this LoadedInternal iwad) {
        if (!iwad.FileExists || iwad.File.Length < 5) {
            return;
        }
        using frmPackageExplorer explorer = new(iwad.File);
        explorer.ShowDialog();
    }
    internal static bool ShowDialog(this CommonDialog dialog, DialogResult expectedResult) {
        return dialog.ShowDialog() == expectedResult;
    }
    internal static bool ShowDialog(this Form form, DialogResult expectedResult) {
        return form.ShowDialog() == expectedResult;
    }
}
