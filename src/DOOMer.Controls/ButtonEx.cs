namespace DOOMer.Controls;

using System;
using System.Windows.Forms;

public class ButtonEx : Button {
    #region File Drag Events
    [System.ComponentModel.DefaultValue(false)]
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
}
