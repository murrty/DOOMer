namespace DOOMer.Controls;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static NativeMethods.Constants;

public sealed class ComboBoxEx : ComboBox {
    [DefaultValue("")]
    public string CueText { get; set { field = value; this.SetHint(); } } = "";

    [DefaultValue(ComboBoxStyle.DropDown)]
    public new ComboBoxStyle DropDownStyle {
        get => base.DropDownStyle;
        set {
            if (value == ComboBoxStyle.DropDown) {
                if (base.DropDownStyle != ComboBoxStyle.DropDown) {
                    base.DropDownStyle = value;
                    this.SetHint();
                 }
                return;
            }

            if (base.DropDownStyle == value) {
                return;
            }

            if (base.DropDownStyle == ComboBoxStyle.DropDown) {
                this.RemoveHint();
            }

            base.DropDownStyle = value;
        }
    }

    #region File Drag Events
    [DefaultValue(false)]
    public bool FileDragging {
        get;
        set {
            if (field == value) {
                return;
            }

            this.DragEnter -= this.FileDrag_DragEnter;
            this.DragDrop -= this.FileDrag_DragDrop;

            field = value;
            if (value) {
                this.DragEnter += this.FileDrag_DragEnter;
                this.DragDrop += this.FileDrag_DragDrop;
            }
        }
    }

    /// <summary>
    ///     Occurs when a file was dragged into the control.
    /// </summary>
    public event EventHandler<FileDraggedEventArgs>? FileDrag;
    /// <summary>
    ///     Occurs when a file gets dropped onto the control.
    /// </summary>
    public event EventHandler<FileDroppedEventArgs>? FileDrop;

    private void FileDrag_DragEnter(object? sender, DragEventArgs e) {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true || e.Data.GetData(DataFormats.FileDrop) is not string[] paths) {
            return;
        }

        FileDraggedEventArgs mevent = new(e, paths);
        this.OnFileDrag(mevent);

        if (mevent.Cancel) {
            e.Effect = DragDropEffects.None;
        }
    }
    private void FileDrag_DragDrop(object? sender, DragEventArgs e) {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true
        || e.AllowedEffect == DragDropEffects.None
        || e.Data.GetData(DataFormats.FileDrop) is not string[] paths) {
            return;
        }

        FileDroppedEventArgs nevent = new(backingEvent: e, paths: paths);
        this.OnFileDrop(nevent);
    }

    private void OnFileDrag(FileDraggedEventArgs e) {
        this.FileDrag?.Invoke(sender: this, e: e);
    }
    private void OnFileDrop(FileDroppedEventArgs e) {
        this.FileDrop?.Invoke(sender: this, e: e);
    }
    #endregion File Drag Events

    protected override void OnHandleCreated(EventArgs e) {
        base.OnHandleCreated(e);
        this.SetHint();
    }

    private void SetHint() {
        if (!this.IsHandleCreated || this.DropDownStyle != ComboBoxStyle.DropDown) {
            return;
        }

        if (string.IsNullOrWhiteSpace(this.CueText)) {
            this.RemoveHint();
            return;
        }

        nint ptrStr = Marshal.StringToHGlobalUni(this.CueText);
        _ = NativeMethods.SendMessageW(this.Handle, CB_SETCUEBANNER, 0x1, ptrStr);
        Marshal.FreeHGlobal(ptrStr);
    }
    private void RemoveHint() {
        _ = NativeMethods.SendMessageW(this.Handle, EM_SETCUEBANNER, 0x0, 0x0);
    }
}
