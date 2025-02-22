#define DRAG_IMAGE_DEBUG_
namespace DOOMer.Controls;

using System;
using System.CodeDom;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using DOOMer.Controls.Events;
using DOOMer.Core;
using static NativeMethods.Constants;

public sealed class ExplorerTreeView : TreeView {
    private RECT BorderRect;
    private int scrollHoverCount;
    private readonly Timer scrollTimer = new() { Interval = 100, };

    [System.ComponentModel.DefaultValue(true)]
    public bool ShowBorders {
        get;
        set {
            if (field != value) {
                field = value;
                if (!this.IsHandleCreated) {
                    return;
                }
                this.RecreateHandle();
            }
        }
    } = true;

    [System.ComponentModel.DefaultValue(BorderStyle.None)]
    public new BorderStyle BorderStyle {
        get => base.BorderStyle;
        set => base.BorderStyle = value;
    }

    static ExplorerTreeView() {
        NativeMethods.InitCommonControls();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExplorerTreeView"/> class.
    /// </summary>
    public ExplorerTreeView() : base() {
        base.BorderStyle = BorderStyle.None;
    }

    private TreeNode? draggingNode;
    private TreeNode? selectedDragDestinationNode;
    private TreeNode? hoveredNode;

    private static bool IsNodeInSameCollection(TreeNode a, TreeNode b) {
        return a.TreeView == b.TreeView && a.Parent == b.Parent;
    }

    #region Legacy Menu
#if NETCOREAPP3_1_OR_GREATER && ENABLE_LEGACY_MENUS
    [System.ComponentModel.DefaultValue(null)]
    public ContextMenu? ContextMenu { get; set {
        if (field == value) {
            return;
        }

        void ShowContextMenu(object? sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right && this.ContextMenu is not null) {
                this.ContextMenu.Show(this, new(e.X, e.Y));
            }
        }

        this.MouseUp -= ShowContextMenu;

        if (value != null && field == null) {
            this.MouseUp += ShowContextMenu;
        }

        field = value;
    } }
#endif
    #endregion Legacy Menu

    #region Drag Timer
    [DebuggerStepThrough]
    private void Drag_TimerTick(object? sender, EventArgs e) {
        if (this.draggingNode is null || this.OnlyAllowDraggedNodesInSameCollection) {
            scrollHoverCount = 0;
            this.scrollTimer.Stop();
            return;
        }

        Point pt = this.PointToClient(MousePosition);
        TreeNode? hoveredNode = this.GetNodeAt(pt);

        if (hoveredNode is null) {
            return;
        }

        if (pt.Y < 30) {
            if (hoveredNode.PrevVisibleNode is not null) {
                hoveredNode = hoveredNode.PrevVisibleNode;
                NativeMethods.ImageList_DragShowNolock(false);
                hoveredNode.EnsureVisible();
                NativeMethods.ImageList_DragShowNolock(true);
                return;
            }
        }
        else if (pt.Y > this.Size.Height - 30 && hoveredNode.NextVisibleNode is not null) {
            hoveredNode = hoveredNode.NextVisibleNode;
            NativeMethods.ImageList_DragShowNolock(false);
            hoveredNode.EnsureVisible();
            NativeMethods.ImageList_DragShowNolock(true);
            return;
        }

        if (this.hoveredNode == hoveredNode && ++this.scrollHoverCount == 5 && hoveredNode.Nodes.Count > 0) {
            this.scrollHoverCount = -10;

            NativeMethods.ImageList_DragShowNolock(false);
            if (hoveredNode.IsExpanded) {
                hoveredNode.Collapse();
            }
            else {
                hoveredNode.Expand();
            }

            this.Refresh();
            NativeMethods.ImageList_DragShowNolock(true);
        }
    }
    #endregion Drag Timer

    #region Node Drag Events
    private DragDropEffects effectDragNode = DragDropEffects.Move;
    private readonly ImageList imgDragDrop = new();

    [System.ComponentModel.DefaultValue(false)]
    public bool MouseDragNodes {
        get;
        set {
            if (field == value) {
                return;
            }

            this.ItemDrag -= this.NodeDrag_ItemDrag;
            this.DragEnter -= this.NodeDrag_DragEnter;
            this.DragOver -= this.NodeDrag_DragOver;
            this.DragDrop -= this.NodeDrag_DragDrop;
            this.DragLeave -= this.NodeDrag_DragLeave;
            this.GiveFeedback -= this.NodeDrag_GiveFeedback;
            this.scrollTimer.Tick -= this.Drag_TimerTick;

            field = value;
            if (value) {
                this.ImageList ??= new();
                this.ItemDrag += this.NodeDrag_ItemDrag;
                this.DragEnter += this.NodeDrag_DragEnter;
                this.DragOver += this.NodeDrag_DragOver;
                this.DragDrop += this.NodeDrag_DragDrop;
                this.DragLeave += this.NodeDrag_DragLeave;
                this.GiveFeedback += this.NodeDrag_GiveFeedback;
                this.scrollTimer.Tick += this.Drag_TimerTick;
            }
        }
    }

    [System.ComponentModel.DefaultValue(false)]
    public bool OnlyAllowDraggedNodesInSameCollection { get; set; }

    /// <summary>
    ///     Occurs before a node begins to be moved.
    /// </summary>
    public event EventHandler<TreeNodeMoveEventArgs>? BeforeNodeDrag;
    /// <summary>
    ///     Occurs before a node being moved hovers over another node.
    /// </summary>
    public event EventHandler<TreeNodeMoveEventArgs>? BeforeNodeDragHover;
    /// <summary>
    ///     Occurs after a node being moved hovers over another node.
    /// </summary>
    public event EventHandler<TreeNodeMoveEventArgs>? AfterNodeDragHover;
    /// <summary>
    ///     Occurs before the node moves.
    /// </summary>
    public event EventHandler<TreeNodeMoveEventArgs>? BeforeNodeDragMove;
    /// <summary>
    ///     Occurs after the node moves.
    /// </summary>
    public event EventHandler<TreeNodeMoveEventArgs>? AfterNodeDrag;

    [MemberNotNullWhen(true, nameof(draggingNode))]
    private bool EventHasNodeDrag(DragEventArgs e) {
        return e.Data?.GetDataPresent("System.Windows.Forms.TreeView", autoConvert: false) == true || this.draggingNode is not null;
    }

    [DebuggerStepThrough]
    private void NodeDrag_ItemDrag(object? sender, ItemDragEventArgs e) {
        if (e.Item is not TreeNode node) {
            return;
        }

        if (this.Cursor.Handle != Cursors.Default.Handle) {
            this.Cursor = Cursors.Default;
        }

        TreeNodeMoveEventArgs mevent = new(node, null, e.Button);
        this.OnBeforeNodeDragged(mevent);

        if (mevent.Cancel) {
            return;
        }

        this.effectDragNode = mevent.Effect;
        this.ignoreAfterSelect = true;
        this.selectedDragDestinationNode = this.SelectedNode;
        this.SelectedNode = node;
        this.hoveredNode = node;
        this.draggingNode = node;
        this.ignoreAfterSelect = false;

        this.imgDragDrop.Images.Clear();
        Size imageSize = new(this.Width.Clamp(64, 256), node.Bounds.Height);
        Rectangle imageBounds = new(0, 0, imageSize.Width, imageSize.Height);
        this.imgDragDrop.ImageSize = imageSize;

        Bitmap bmp = new(imageSize.Width, imageSize.Height);
        Graphics gfx = Graphics.FromImage(bmp);
#if DRAG_IMAGE_DEBUG
        gfx.FillRectangle(Brushes.Red, new Rectangle(0, 0, bmp.Width, bmp.Height));
#endif

        const int margin = 4;
        int xoffset = margin;
        int largestHeight = imageSize.Height - (margin * 2);
        string text = node.Nodes.Count > 0 ? ("[" + node.Nodes.Count + "] " + node.Text) : node.Text;

        void DrawImage() {
            if (this.ImageList.Images.Count < 1) {
                return;
            }

            Rectangle imgRect = new(xoffset + margin, margin, largestHeight, largestHeight);
            xoffset += imgRect.Width + margin;
            gfx.DrawImage(this.ImageList.Images[node.ImageIndex], imgRect);
        }

        Rectangle GetExpandoRect() {
            Rectangle expandoRect = new(xoffset + margin, margin, largestHeight, largestHeight);
            xoffset += expandoRect.Width + margin;
            return expandoRect;
        }

        Rectangle GetCheckBoxRect(Size refSize) {
            Rectangle checkBoxRect = new(xoffset + margin,
                (imageSize.Height / 2) - (refSize.Height / 2),
                refSize.Width,
                refSize.Height);
            xoffset += checkBoxRect.Width + margin;
            return checkBoxRect;
        }

        Rectangle GetTextRectangle() {
            SizeF textSize = gfx.MeasureString(text, node.NodeFont ?? this.Font);

            int x = xoffset + margin;
            int y = (int)Math.Ceiling((imageBounds.Height / 2) - (textSize.Height / 2));
            xoffset += x + margin;

            return new Rectangle(x, y, imageSize.Width - xoffset - 12, imageSize.Height);
        }

        if (VisualStyle.UseRenderer) {
            var renderer = VisualStyle.SelectedNoFocus;
            renderer.DrawBackground(gfx, imageBounds);

            if (node.Nodes.Count > 0) {
                var expando = node.IsExpanded ? VisualStyle.ExpandoOpen : VisualStyle.ExpandoClosed;
                Rectangle expandoRect = GetExpandoRect();
                expando.DrawEdge(gfx, expandoRect, default, EdgeStyle.Bump, EdgeEffects.Soft);
                expando.DrawBackground(gfx, expandoRect);
            }

            if (this.CheckBoxes) {
                var chk = node.Checked ? VisualStyle.CheckBoxChecked : VisualStyle.CheckBoxUnchecked;
                Rectangle chkRect = GetCheckBoxRect(chk.GetPartSize(gfx, ThemeSizeType.True));
                chk.DrawEdge(gfx, chkRect, default, EdgeStyle.Bump, EdgeEffects.Soft);
                chk.DrawBackground(gfx, chkRect);
            }

            DrawImage();

            var textRect = GetTextRectangle();
            //Rectangle textRect = new(xoffset + margin + 6,
            //    (int)Math.Floor(ty),
            //    imageSize.Width - xoffset + margin - 12,
            //    imageSize.Height);

            renderer.DrawText(gfx, textRect, text, false,
                (this.RightToLeft == RightToLeft.Yes ? TextFormatFlags.RightToLeft : 0x0) | TextFormatFlags.VerticalCenter);
        }
        else {
            gfx.FillRectangle(new SolidBrush(node.BackColor), imageBounds);

            if (this.CheckBoxes) {
                Rectangle chkRect = GetCheckBoxRect(CheckBoxRenderer.GetGlyphSize(gfx, CheckBoxState.MixedNormal));
                CheckBoxRenderer.DrawCheckBox(gfx, chkRect.Location,
                    node.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
                //ControlPaint.DrawCheckBox(gfx, new Rectangle(0, 4, 14, 14), node.Checked ? ButtonState.Checked : ButtonState.Normal);
            }

            DrawImage();

            SizeF textSize = gfx.MeasureString(text, node.NodeFont ?? this.Font);
            float ty = (imageSize.Width / 2) - (textSize.Height / 2);

            //IncrementOffset((int)Math.Ceiling(textSize.Width));
            gfx.DrawString(text,
                this.Font,
                new SolidBrush(this.ForeColor),
                (float)this.Indent, ty);
        }

        this.imgDragDrop.Images.Add(bmp);
        Point p = this.PointToClient(MousePosition);
        int dx = p.X + this.Indent - node.Bounds.Left;
        int dy = p.Y - node.Bounds.Top;

        if (!NativeMethods.ImageList_BeginDrag(this.imgDragDrop.Handle, 0, dx, dy - 26)) {
            return;
        }

        this.scrollTimer.Start();
        this.DoDragDrop(node, mevent.Effect);
        NativeMethods.ImageList_EndDrag();
    }
    [DebuggerStepThrough]
    private void NodeDrag_DragEnter(object? sender, DragEventArgs e) {
        if (!this.EventHasNodeDrag(e)) {
            return;
        }

        if (this.Cursor.Handle != Cursors.Default.Handle) {
            this.Cursor = Cursors.Default;
        }

        e.Effect = this.effectDragNode;
        NativeMethods.ImageList_DragEnter(this.Handle, e.X - this.Left, e.Y - this.Top);
    }
    [DebuggerStepThrough]
    private void NodeDrag_DragOver(object? sender, DragEventArgs e) {
        if (!this.EventHasNodeDrag(e)) {
            return;
        }

        if (this.Cursor.Handle != Cursors.Default.Handle) {
            this.Cursor = Cursors.Default;
        }

        Point fp = this.PointToClient(new Point(e.X, e.Y));
        TreeNode? hover = this.GetNodeAt(fp);

        if (hover != this.hoveredNode) {
            this.hoveredNode = hover;
            scrollHoverCount = 0;
        }

        e.Effect = this.effectDragNode;
        NativeMethods.ImageList_DragMove(fp.X - this.Left, fp.Y - this.Top);
    }
    [DebuggerStepThrough]
    private void NodeDrag_DragDrop(object? sender, DragEventArgs e) {
        if (!this.EventHasNodeDrag(e)) {
            return;
        }

        if (this.Cursor.Handle != Cursors.Default.Handle) {
            this.Cursor = Cursors.Default;
        }

        this.scrollTimer.Stop();

        if (e.AllowedEffect == DragDropEffects.None) {
            return;
        }

        NativeMethods.ImageList_DragLeave(this.Handle);

        TreeNode node = this.draggingNode;
        TreeNode? destinationNode = this.selectedDragDestinationNode;
        this.selectedDragDestinationNode = null;

        if (node.TreeView != this) {
            this.ignoreAfterSelect = true;
            this.SelectedNode = destinationNode;
            this.draggingNode = null;
            this.ignoreAfterSelect = false;
            return;
        }

        Point pt = this.PointToClient(new Point(e.X, e.Y));
        TreeNode? destination = this.GetNodeAt(pt);

        if (destination is null || destination == node) {
            this.ignoreAfterSelect = true;
            this.SelectedNode = destinationNode;
            this.draggingNode = null;
            this.ignoreAfterSelect = false;
            return;
        }

        TreeNodeMoveEventArgs nevent = new(node, destination, default);
        this.OnBeforeNodeDragMove(nevent);
        if (nevent.Cancel) {
            this.ignoreAfterSelect = true;
            this.SelectedNode = destinationNode;
            this.draggingNode = null;
            this.ignoreAfterSelect = false;
            return;
        }

        if (!nevent.OverrideSameCollectionRestriction && this.OnlyAllowDraggedNodesInSameCollection && !IsNodeInSameCollection(node, destination)) {
            this.ignoreAfterSelect = true;
            this.SelectedNode = destinationNode;
            this.draggingNode = null;
            this.ignoreAfterSelect = false;
            return;
        }

        var collection = destination.Parent?.Nodes ?? this.Nodes;
        int index = destination.Index;
        this.Nodes.Remove(node);
        collection.Insert(index, node);
        this.ignoreAfterSelect = true;
        this.SelectedNode = destinationNode ?? node ?? destination;
        this.ignoreAfterSelect = false;
        this.draggingNode = null;
        this.OnAfterNodeDragMove(nevent);
    }
    [DebuggerStepThrough]
    private void NodeDrag_DragLeave(object? sender, EventArgs e) {
        if (this.draggingNode is null) {
            return;
        }

        if (this.Cursor.Handle != Cursors.Default.Handle) {
            this.Cursor = Cursors.Default;
        }

        NativeMethods.ImageList_DragLeave(this.Handle);
        this.scrollTimer.Stop();
    }
    [DebuggerStepThrough]
    private void NodeDrag_GiveFeedback(object? sender, GiveFeedbackEventArgs e) {
        if (this.draggingNode is null) {
            if (this.Cursor.Handle != Cursors.Default.Handle) {
                this.Cursor = Cursors.Default;
            }
            return;
        }

        Point pt = this.PointToClient(MousePosition);
        TreeNode? destination = this.GetNodeAt(pt);

        if (destination == this.draggingNode) {
            this.ignoreAfterSelect = true;
            NativeMethods.ImageList_DragShowNolock(false);
            this.SelectedNode = this.draggingNode;
            NativeMethods.ImageList_DragShowNolock(true);
            this.ignoreAfterSelect = false;
            e.UseDefaultCursors = false;
            this.Cursor = Cursors.No;
            return;
        }

        TreeNodeMoveEventArgs mevent = new(this.draggingNode, destination, default);
        this.OnBeforeNodeDragHover(mevent);

        if (mevent.Cancel
        || destination is null
        || destination == this.draggingNode
        || (!mevent.OverrideSameCollectionRestriction && this.OnlyAllowDraggedNodesInSameCollection && !IsNodeInSameCollection(this.draggingNode, destination))
        ) {
            this.ignoreAfterSelect = true;
            NativeMethods.ImageList_DragShowNolock(false);
            this.SelectedNode = this.draggingNode;
            NativeMethods.ImageList_DragShowNolock(true);
            this.ignoreAfterSelect = false;
            e.UseDefaultCursors = false;
            this.Cursor = Cursors.No;
            return;
        }

        e.UseDefaultCursors = true;
        this.Cursor = Cursors.Default;
        this.effectDragNode = mevent.Effect;

        this.ignoreAfterSelect = true;
        NativeMethods.ImageList_DragShowNolock(false);
        this.SelectedNode = destination;
        NativeMethods.ImageList_DragShowNolock(true);
        this.ignoreAfterSelect = false;
        this.OnAfterNodeDragHover(mevent);
    }

    [DebuggerStepThrough]
    private void OnBeforeNodeDragged(TreeNodeMoveEventArgs mevent) {
        this.BeforeNodeDrag?.Invoke(this, mevent);
    }
    [DebuggerStepThrough]
    private void OnBeforeNodeDragHover(TreeNodeMoveEventArgs mevent) {
        this.BeforeNodeDragHover?.Invoke(this, mevent);
    }
    [DebuggerStepThrough]
    private void OnAfterNodeDragHover(TreeNodeMoveEventArgs mevent) {
        this.AfterNodeDragHover?.Invoke(this, mevent);
    }
    [DebuggerStepThrough]
    private void OnBeforeNodeDragMove(TreeNodeMoveEventArgs nevent) {
        this.BeforeNodeDragMove?.Invoke(this, nevent);
    }
    [DebuggerStepThrough]
    private void OnAfterNodeDragMove(TreeNodeMoveEventArgs nevent) {
        this.AfterNodeDrag?.Invoke(this, nevent);
    }

    #region Visual Styles
    public static class VisualStyle {
        private static readonly VisualStyleElement expandoOpen = VisualStyleElement.CreateElement("Explorer::TreeView", 2, 2);
        private static readonly VisualStyleElement expandoClosed = VisualStyleElement.CreateElement("Explorer::TreeView", 2, 1);
        private static readonly VisualStyleElement expandoHoverOpen = VisualStyleElement.CreateElement("Explorer::TreeView", 4, 2);
        private static readonly VisualStyleElement expandoHoverClosed = VisualStyleElement.CreateElement("Explorer::TreeView", 4, 1);
        private static readonly VisualStyleElement hovered = VisualStyleElement.CreateElement("Explorer::TreeView", 1, 2);
        private static readonly VisualStyleElement selected = VisualStyleElement.CreateElement("Explorer::TreeView", 1, 3);
        private static readonly VisualStyleElement selectedNoFocus = VisualStyleElement.CreateElement("Explorer::TreeView", 1, 5);
        private static readonly VisualStyleElement unknown = VisualStyleElement.CreateElement("Explorer::TreeView", 1, 6);

        public static VisualStyleRenderer? ExpandoOpen { get; }
        public static VisualStyleRenderer? ExpandoClosed { get; }
        public static VisualStyleRenderer? ExpandoExpandoOpenHover { get; }
        public static VisualStyleRenderer? ExpandoExpandoCloseHover { get; }
        public static VisualStyleRenderer? Hovering { get; }
        public static VisualStyleRenderer? Selected { get; }
        public static VisualStyleRenderer? SelectedNoFocus { get; }
        public static VisualStyleRenderer? Unknown { get; }
        public static VisualStyleRenderer? CheckBoxChecked { get; }
        public static VisualStyleRenderer? CheckBoxUnchecked { get; }

        [MemberNotNullWhen(true, nameof(ExpandoOpen), nameof(ExpandoClosed), nameof(ExpandoExpandoOpenHover), nameof(ExpandoExpandoCloseHover), nameof(Hovering), nameof(Selected), nameof(SelectedNoFocus), nameof(CheckBoxChecked), nameof(CheckBoxUnchecked))]
        public static bool Created { get; }

        [MemberNotNullWhen(true, nameof(ExpandoOpen), nameof(ExpandoClosed), nameof(ExpandoExpandoOpenHover), nameof(ExpandoExpandoCloseHover), nameof(Hovering), nameof(Selected), nameof(SelectedNoFocus), nameof(CheckBoxChecked), nameof(CheckBoxUnchecked))]
        public static bool UseRenderer {
            get {
                return Created && VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser;
            }
        }

        static VisualStyle() {
            try {
                ExpandoOpen = new(expandoOpen);
                ExpandoClosed = new(expandoClosed);
                ExpandoExpandoOpenHover = new(expandoHoverOpen);
                ExpandoExpandoCloseHover = new(expandoHoverClosed);
                Hovering = new(hovered);
                Selected = new(selected);
                SelectedNoFocus = new(selectedNoFocus);
                Unknown = new(unknown);
                CheckBoxChecked = new(VisualStyleElement.Button.CheckBox.CheckedNormal);
                CheckBoxUnchecked = new(VisualStyleElement.Button.CheckBox.UncheckedNormal);
                Created = true;
            }
            catch { }
        }
    }
    #endregion Visual Styles

    #endregion Node Drag Events

    #region File Drag Events
    private TreeNode? fileSelectedNode;

    [System.ComponentModel.DefaultValue(false)]
    public bool FileDragging {
        get;
        set {
            if (field == value) {
                return;
            }

            this.DragEnter -= this.FileDrag_DragEnter;
            this.DragOver -= this.FileDrag_DragOver;
            this.DragDrop -= this.FileDrag_DragDrop;
            this.DragLeave -= this.FileDrag_DragLeave;

            field = value;
            if (value) {
                this.DragEnter += this.FileDrag_DragEnter;
                this.DragOver += this.FileDrag_DragOver;
                this.DragDrop += this.FileDrag_DragDrop;
                this.DragLeave += this.FileDrag_DragLeave;
            }
        }
    }

    /// <summary>
    ///     Occurs when a file was dragged into the control.
    /// </summary>
    public event EventHandler<TreeNodeFileDraggedEventArgs>? FileDrag;
    /// <summary>
    ///     Occurs before a file gets dragged onto another node.
    /// </summary>
    public event EventHandler<TreeNodeFileDraggedEventArgs>? BeforeFileDragHover;
    /// <summary>
    ///     Occurs after a file gets dragged onto another node.
    /// </summary>
    public event EventHandler<TreeNodeFileDraggedEventArgs>? AfterFileDragHover;
    /// <summary>
    ///     Occurs when a file gets dropped onto the control.
    /// </summary>
    public event EventHandler<TreeNodeFileDroppedEventArgs>? FileDrop;

    private void FileDrag_DragEnter(object? sender, DragEventArgs e) {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true || e.Data.GetData(DataFormats.FileDrop) is not string[] paths) {
            return;
        }

        Point fp = this.PointToClient(new Point(e.X, e.Y));
        TreeNode? hoveredNode = this.GetNodeAt(fp);

        TreeNodeFileDraggedEventArgs mevent = new(e, paths, hoveredNode);
        this.OnFileDrag(mevent);

        if (mevent.Cancel) {
            e.Effect = DragDropEffects.None;
            return;
        }

        scrollTimer.Start();
    }
    //private void FileDrag_DragEnterN(object? sender, DragEventArgs e) {
    //    if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true || e.AllowedEffect == DragDropEffects.None) {
    //        return;
    //    }
    //}
    private void FileDrag_DragOver(object? sender, DragEventArgs e) {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true
        || e.AllowedEffect == DragDropEffects.None
        || e.Data.GetData(DataFormats.FileDrop) is not string[] paths) {
            return;
        }

        Point fp = this.PointToClient(new Point(e.X, e.Y));
        TreeNode? hoveredNode = this.GetNodeAt(fp);

        if (hoveredNode == this.SelectedNode) {
            return;
        }

        TreeNodeFileDraggedEventArgs mevent = new(backingEvent: e, paths: paths, node: hoveredNode);
        this.OnBeforeFileDragHover(mevent);

        if (mevent.Cancel || e.Effect == DragDropEffects.None) {
            e.Effect = DragDropEffects.None;
            this.SilentlyChangeNode(this.fileSelectedNode);
            return;
        }

        this.fileSelectedNode = this.SelectedNode;
        this.SilentlyChangeNode(hoveredNode);
        this.OnAfterFileDragHover(mevent);
    }
    private void FileDrag_DragDrop(object? sender, DragEventArgs e) {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true
        || e.AllowedEffect == DragDropEffects.None
        || e.Data.GetData(DataFormats.FileDrop) is not string[] paths) {
            return;
        }

        Point pt = this.PointToClient(new Point(e.X, e.Y));
        TreeNode? hoveredNode = this.GetNodeAt(pt);

        this.ignoreAfterSelect = true;
        this.SelectedNode = this.fileSelectedNode;
        this.fileSelectedNode = null;
        this.ignoreAfterSelect = false;

        TreeNodeFileDroppedEventArgs nevent = new(backingEvent: e, paths: paths, node: hoveredNode);
        this.OnFileDrop(nevent);
    }
    private void FileDrag_DragLeave(object? sender, EventArgs e) {
        if (this.draggingNode is not null) {
            return;
        }

        this.ignoreAfterSelect = true;
        this.SelectedNode = this.fileSelectedNode;
        this.fileSelectedNode = null;
        this.ignoreAfterSelect = false;
    }

    private void OnFileDrag(TreeNodeFileDraggedEventArgs e) {
        this.FileDrag?.Invoke(sender: this, e: e);
    }
    private void OnBeforeFileDragHover(TreeNodeFileDraggedEventArgs e) {
        this.BeforeFileDragHover?.Invoke(sender: this, e: e);
    }
    private void OnAfterFileDragHover(TreeNodeFileDraggedEventArgs e) {
        this.AfterFileDragHover?.Invoke(sender: this, e: e);
    }
    private void OnFileDrop(TreeNodeFileDroppedEventArgs e) {
        this.FileDrop?.Invoke(sender: this, e: e);
    }
    #endregion File Drag Events

    public void SetCheckState(TreeNode node, CheckState state) {
        if (node.TreeView is not ExplorerTreeView etv) {
            return;
        }

        const int TVIF_HANDLE = 0x10;
        const int TVIF_STATE = 0x08;
        const int TVIS_STATEIMAGEMASK = 0xF000;

        const int TV_FIRST = 0x1100,
            TVM_SETITEM = TV_FIRST + 13;

        etv.ignoreAfterCheck = true;
        if (state == CheckState.Indeterminate) {
            TV_ITEM it = new() {
                mask = TVIF_HANDLE | TVIF_STATE,
                hItem = node.Handle,
                stateMask = TVIS_STATEIMAGEMASK,
                state = (uint)(3 << 12),
            };

            _ = NativeMethods.SendMessage(this.Handle, TVM_SETITEM, 0, it);
            etv.ignoreAfterCheck = false;
            return;
        }

        node.Checked = state == CheckState.Checked;
        etv.ignoreAfterCheck = false;
    }
    private void SilentlyChangeNode(TreeNode? node) {
        this.ignoreAfterSelect = true;
        this.SelectedNode = node;
        this.ignoreAfterSelect = false;
    }

    public bool MoveNodeUp() {
        if (this.SelectedNode is null) {
            return false;
        }

        TreeNode node = this.SelectedNode;
        var nodeCollection = this.SelectedNode.Parent?.Nodes ?? this.Nodes;

        if (nodeCollection is null || node.Index <= 0) {
            return false;
        }

        this.ignoreAfterSelect = true;
        int newIndex = node.Index - 1;
        nodeCollection.Remove(node);
        nodeCollection.Insert(newIndex, node);
        this.SelectedNode = node;
        this.ignoreAfterSelect = false;
        return true;
    }
    public bool MoveNodeDown() {
        if (this.SelectedNode is null) {
            return false;
        }

        TreeNode node = this.SelectedNode;
        var nodeCollection = this.SelectedNode.Parent?.Nodes ?? this.Nodes;

        if (nodeCollection is null || node.Index >= nodeCollection.Count - 1) {
            return false;
        }

        this.ignoreAfterSelect = true;
        int newIndex = node.Index + 1;
        nodeCollection.Remove(node);
        nodeCollection.Insert(newIndex, node);
        this.SelectedNode = node;
        this.ignoreAfterSelect = false;
        return true;
    }

    protected override CreateParams CreateParams {
        get {
            CreateParams param = base.CreateParams;
            param.ExStyle |= TVS_EX_DOUBLEBUFFER | TVS_EX_PARTIALCHECKBOXES;

            if (this.ShowBorders) {
                // blah.
                param.ExStyle |= 0x10000; // WS_EX_CONTROLPARENT = 0x10000
                param.ExStyle &= 0x200; // WS_EX_CLIENTEDGE = 0x200

                // Ensure WS_BORDER is enabled? i forgor.
                param.Style &= ~0x800000; // WS_BORDER = 0x800000
                param.Style |= 0x800000; // WS_BORDER again
            }

            return param;
        }
    }
    protected override void OnHandleCreated(EventArgs e) {
        _ = NativeMethods.SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, TVS_EX_DOUBLEBUFFER, TVS_EX_DOUBLEBUFFER);
        _ = NativeMethods.SetWindowTheme(this.Handle, "Explorer", null);
        base.OnHandleCreated(e);
    }
    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        if (this.BorderStyle == BorderStyle.None) {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
    }
    protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e) {
        if (e.Button == MouseButtons.Right && e.Node is not null) {
            this.SelectedNode = e.Node;
        }
        base.OnNodeMouseClick(e);
    }
    protected override void OnKeyDown(KeyEventArgs e) {
        base.OnKeyDown(e);
        switch (e.KeyCode) {
            case Keys.F2 when this.LabelEdit && this.SelectedNode is not null: {
                this.SelectedNode.BeginEdit();
            } break;
        }
    }
    //[DebuggerStepThrough]
    protected override void OnGotFocus(EventArgs e) {
        if (this.ShowBorders) {
            _ = NativeMethods.SendMessage(this.Handle, WM_NCPAINT, 0, 0);
        }
        base.OnGotFocus(e);
    }
    //[DebuggerStepThrough]
    protected override void OnLostFocus(EventArgs e) {
        if (this.ShowBorders) {
            _ = NativeMethods.SendMessage(this.Handle, WM_NCPAINT, 0, 0);
        }
        base.OnLostFocus(e);
    }
    [DebuggerStepThrough]
    protected override void WndProc(ref Message m) {
        switch (m.Msg) {
            case WM_NCPAINT when this.ShowBorders: {
                base.WndProc(ref m);

                if (System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser) {
                    RECT r = RECT.Empty;
                    _ = NativeMethods.GetWindowRect(this.Handle, ref r);
                    r.right -= r.left + 1;
                    r.bottom -= r.top + 1;
                    r.top = 1;
                    r.left = 1;

                    r.left += this.BorderRect.left;
                    r.top += this.BorderRect.top;
                    r.right -= this.BorderRect.right;
                    r.bottom -= this.BorderRect.bottom;

                    nint hDc = NativeMethods.GetWindowDC(this.Handle);
                    _ = NativeMethods.ExcludeClipRect(hDc, r.left, r.top, r.right, r.bottom);

                    using Graphics g = Graphics.FromHdc(hDc);
                    Color DrawColor = this.Enabled ?
                        (this.Focused ? SystemColors.ControlDark : SystemColors.ControlLight) :
                        SystemColors.ControlDarkDark;
                    g.Clear(DrawColor);

                    _ = NativeMethods.ReleaseDC(this.Handle, hDc);
                }

                m.Result = IntPtr.Zero;
            } break;

            case WM_NCCALCSIZE when this.ShowBorders: {
                base.WndProc(ref m);
                if (!System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser) {
                    return;
                }

                NCCALCSIZE_PARAMS param = new();
                RECT winRect = RECT.Empty;

                if (m.WParam != IntPtr.Zero) {
                    param = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(m.LParam);
                    winRect = param.rgrc0;
                }

                RECT clientRect = winRect;
                // 2px border
                //clientRect.left += 2;
                //clientRect.top += 2;
                //clientRect.right -= 2;
                //clientRect.bottom -= 2;

                // 1px border
                //clientRect.left++;
                //clientRect.top++;
                //clientRect.right--;
                //clientRect.bottom--;

                this.BorderRect = new(clientRect.left - winRect.left,
                    clientRect.top - winRect.top,
                    winRect.right - clientRect.right,
                    winRect.bottom - clientRect.bottom);

                if (m.WParam == IntPtr.Zero) {
                    Marshal.StructureToPtr(clientRect, m.LParam, false);
                }
                else {
                    param.rgrc0 = clientRect;
                    Marshal.StructureToPtr(param, m.LParam, false);
                }

                const int WVR_HREDRAW = 0x100;
                const int WVR_VREDRAW = 0x200;
                const int WVR_REDRAW = WVR_HREDRAW | WVR_VREDRAW;

                m.Result = (IntPtr)WVR_REDRAW;
            } break;

            // WM_THEMECHANGED
            case WM_THEMECHANGED: {
                base.WndProc(ref m);
                this.UpdateStyles();
            } break;

            default: {
                base.WndProc(ref m);
            } break;
        }
    }

    private bool ignoreAfterSelect;
    protected override void OnAfterSelect(TreeViewEventArgs e) {
        if (this.ignoreAfterSelect) {
            return;
        }
        base.OnAfterSelect(e);
    }

    private bool ignoreAfterCheck;
    protected override void OnAfterCheck(TreeViewEventArgs e) {
        if (this.ignoreAfterCheck) {
            return;
        }

        base.OnAfterCheck(e);
    }
}
