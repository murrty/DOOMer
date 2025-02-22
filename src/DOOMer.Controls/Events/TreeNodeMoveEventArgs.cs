namespace DOOMer.Controls.Events;

using System;
using System.Windows.Forms;

public sealed class TreeNodeMoveEventArgs : EventArgs {
    public TreeNode TreeNode { get; }
    public TreeNode? HoveredTreeNode { get; }
    public bool Cancel { get; set; }
    public bool OverrideSameCollectionRestriction { get; set; }
    public DragDropEffects Effect { get; set; }
    public MouseButtons Button { get; }
    public TreeNodeMoveEventArgs(TreeNode node, TreeNode? hoveredTreeNode, MouseButtons button) {
        this.TreeNode = node;
        this.HoveredTreeNode = hoveredTreeNode;
        this.Button = button;
    }
}
