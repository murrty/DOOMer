namespace DOOMer.Controls;

using System.Windows.Forms;

public sealed class TreeNodeFileDroppedEventArgs : FileDroppedEventArgs {
    /// <summary>
    ///     The tree node that the file(s) were dropped on.
    /// </summary>
    public TreeNode Node { get; }

    public TreeNodeFileDroppedEventArgs(DragEventArgs backingEvent, string[] paths, TreeNode node) : base(backingEvent: backingEvent, paths: paths) {
        this.Node = node;
    }
}
