﻿//----------------------
// <auto-generated>
//     Backwards compatibility with StatusBars.
// </auto-generated>
//----------------------

#nullable enable
namespace System.Windows.Forms;

#if NETCOREAPP3_1_OR_GREATER && ENABLE_LEGACY_MENUS
/// <summary>
///     Provides data for the <see cref='StatusBar.OnPanelClick'/> event.
/// </summary>
internal class StatusBarPanelClickEventArgs : MouseEventArgs {
    /// <summary>
    ///     Specifies the <see cref='Forms.StatusBarPanel'/> that represents the clicked panel.
    /// </summary>
    public StatusBarPanel StatusBarPanel { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref='StatusBarPanelClickEventArgs'/> class.
    /// </summary>
    public StatusBarPanelClickEventArgs(StatusBarPanel statusBarPanel, MouseButtons button, int clicks, int x, int y)
        : base(button, clicks, x, y, 0) {
        this.StatusBarPanel = statusBarPanel;
    }
}
#endif
