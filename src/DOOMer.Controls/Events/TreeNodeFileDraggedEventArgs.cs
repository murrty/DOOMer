namespace DOOMer.Controls;

using System;
using System.Windows.Forms;

public class TreeNodeFileDraggedEventArgs : FileDraggedEventArgs {
    /// <summary>
    ///     The tree node associated with the file drag event.
    /// </summary>
    public TreeNode? Node { get; }

    public TreeNodeFileDraggedEventArgs(DragEventArgs backingEvent, string[] paths, TreeNode? node) : base(backingEvent: backingEvent, paths: paths) {
        this.Node = node;
    }
}
