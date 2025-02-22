namespace DOOMer.WinForms;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using DOOMer.Controls;
using DOOMer.Controls.Events;
using DOOMer.Core;

public partial class frmMain : Form {
    private bool SaveConfigFile { get; set; }

    private TreeView? SelectedTreeView { get; set; }

    private LoadedExternal? GetParentExternalIfEnabled {
        get {
            return (chkAddAsDependant.Checked || ModifierKeys.HasFlag(Keys.Control)) && tvExternalFiles.SelectedNode?.Tag is LoadedExternal external ? external : null;
        }
    }

    private readonly Timer SaveConfigTimer = new() { Interval = 60_000, };
    private readonly TreeNode InternalWadBrowseNode = new("(Browse)") { Tag = null, ToolTipText = "Browse for an existing wad file...", };
    private readonly TreeNode InternalWadConfigBrowseNode = new("(Browse)") { Tag = null, ToolTipText = "Browse for an existing wad file...", };
    private readonly TreeNode PortConfigBrowseNode = new("(Browse)") { Tag = null, ToolTipText = "Browse for an existing port...", };
    private Size LastSize;

    #region Menus
    readonly ContextMenu cmMultiplayer = new() { Name = nameof(cmMultiplayer), };
    readonly MenuItem mMultiplayerImportSettings = new("import settings") { Name = nameof(mMultiplayerImportSettings), };
    readonly MenuItem mMultiplayerExportCurrentSettings = new("export current settings") { Name = nameof(mMultiplayerExportCurrentSettings), };

    readonly ContextMenu cmMoreOptions = new() { Name = nameof(cmMoreOptions), };
    readonly MenuItem mEnabledPWADsLaunchWIthDroppedFiles = new("Enabled PWADs launch with dropped PWADs") { Name = nameof(mEnabledPWADsLaunchWIthDroppedFiles), };
    readonly MenuItem mOptionsSeparator = new("-") { Name = nameof(mOptionsSeparator), };
    readonly MenuItem mGenerateArguments = new("Generate arguments") { Name = nameof(mGenerateArguments), };
    readonly MenuItem mScanDoomPackage = new("Scan DOOM package...") { Name = nameof(mScanDoomPackage), };
    readonly MenuItem mSaveSettings = new("Save settings now") { Name = nameof(mSaveSettings), Enabled = false, };

    readonly ContextMenu cmFileMenus = new() { Name = nameof(cmFileMenus), };
    readonly MenuItem mAddExistingFile = new("Add existing file...") { Name = nameof(mAddExistingFile), };
    readonly MenuItem mAddExistingFilesAsDependant = new("Add existing file(s) as dependant...") { Name = nameof(mAddExistingFilesAsDependant), Enabled = false, };
    readonly MenuItem mEditSelectedItem = new("Edit selected item") { Name = nameof(mEditSelectedItem), Enabled = false, };
    readonly MenuItem mScanSelectedFile = new("Scan selected file") { Name = nameof(mScanSelectedFile), Enabled = false, };
    readonly MenuItem mRemoveSelectedItem = new("Remove selected item") { Name = nameof(mRemoveSelectedItem), Enabled = false, };
    readonly MenuItem mBrowseToFileOrDirectory = new("Browse to selected file/directory...") { Name = nameof(FormBorderStyle), Enabled = false, };

#if DEBUG
    readonly MenuItem mDisplayArgsInsteadOfLaunch = new("Display arguments instead of launch") { Name = nameof(mDisplayArgsInsteadOfLaunch), };
#endif

    private void InitializeMenus() {
        mMultiplayerImportSettings.Click += this.mMultiplayerImportSettings_Click;
        mMultiplayerExportCurrentSettings.Click += this.mMultiplayerExportCurrentSettings_Click;
        cmMultiplayer.MenuItems.Add(mMultiplayerImportSettings);
        cmMultiplayer.MenuItems.Add(mMultiplayerExportCurrentSettings);
        btnMultiplayerConfig.Click += (_, _) => cmMultiplayer.Show(btnMultiplayerConfig, new Point(0, btnMultiplayer.Height));

        mEnabledPWADsLaunchWIthDroppedFiles.Click += (_, _) => {
            mEnabledPWADsLaunchWIthDroppedFiles.Checked ^= true;
            this.OnConfigSaveRequired();
        };
        mGenerateArguments.Click += this.mGenerateArguments_Click;
        mScanDoomPackage.Click += this.mScanDoomPackage_Click;
        mSaveSettings.Click += this.mSaveSettings_Click;
        cmMoreOptions.MenuItems.Add(mEnabledPWADsLaunchWIthDroppedFiles);
        cmMoreOptions.MenuItems.Add(mOptionsSeparator);
        cmMoreOptions.MenuItems.Add(mGenerateArguments);
        cmMoreOptions.MenuItems.Add(mScanDoomPackage);
        cmMoreOptions.MenuItems.Add(mSaveSettings);
        btnMore.Click += (_, _) => cmMoreOptions.Show(btnMore, new Point(0, btnMore.Height));

        mAddExistingFile.Click += this.mAddExistingFile_Click;
        mAddExistingFilesAsDependant.Click += this.mAddExistingFilesAsDependant_Click;
        mEditSelectedItem.Click += this.mEditSelectedFile_Click;
        mScanSelectedFile.Click += this.mScanSelectedFile_Click;
        mRemoveSelectedItem.Click += this.mRemoveSelectedItem_Click;
        mBrowseToFileOrDirectory.Click += this.mBrowseToFileOrDirectory_Click;
        cmFileMenus.MenuItems.Add(mAddExistingFile);
        cmFileMenus.MenuItems.Add(mAddExistingFilesAsDependant);
        cmFileMenus.MenuItems.Add(mEditSelectedItem);
        cmFileMenus.MenuItems.Add(mScanSelectedFile);
        cmFileMenus.MenuItems.Add(mRemoveSelectedItem);
        cmFileMenus.MenuItems.Add(mBrowseToFileOrDirectory);
        tvConfigIWADs.MouseUp += (_, e) => {
            if (e.Button == MouseButtons.Right) {
                this.SelectedTreeView = tvConfigIWADs;
                cmFileMenus.Show(tvConfigIWADs, new Point(e.X, e.Y));
            }
        };
        tvConfigSourcePorts.MouseUp += (_, e) => {
            if (e.Button == MouseButtons.Right) {
                this.SelectedTreeView = tvConfigSourcePorts;
                cmFileMenus.Show(tvConfigSourcePorts, new Point(e.X, e.Y));
            }
        };
        tvExternalFiles.MouseUp += (_, e) => {
            if (e.Button == MouseButtons.Right) {
                this.SelectedTreeView = tvExternalFiles;
                cmFileMenus.Show(tvExternalFiles, new Point(e.X, e.Y));
            }
        };
        tvIWADs.MouseUp += (_, e) => {
            if (e.Button == MouseButtons.Right) {
                this.SelectedTreeView = tvIWADs;
                cmFileMenus.Show(tvIWADs, new Point(e.X, e.Y));
            }
        };

#if DEBUG
        mDisplayArgsInsteadOfLaunch.Click += (_, _) => mDisplayArgsInsteadOfLaunch.Checked ^= true;
        cmMoreOptions.MenuItems.Add(mDisplayArgsInsteadOfLaunch);
#endif
    }

    private void mMultiplayerImportSettings_Click(object? sender, EventArgs e) {
        using OpenFileDialog ofd = new() {
            Title = "Load multiplayer settings...",
            Filter = "JavaScript Ojbect Notation file (*.json)|*.json",
        };

        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        string mpInstanceString = File.ReadAllText(ofd.FileName);
        try {
            MultiplayerArguments? instance = JsonSerializer.Deserialize<MultiplayerArguments>(mpInstanceString)
                ?? throw new NullReferenceException("Deserialized instance is null.");
            this.LoadMultiplayerSettings(instance);
            this.OnConfigSaveRequired();
        }
        catch {
        }
    }
    private void mMultiplayerExportCurrentSettings_Click(object? sender, EventArgs e) {
        if (cbMultiplayerGameMode.SelectedIndex < 1) {
            return;
        }

        using SaveFileDialog sfd = new() {
            Title = "Save multiplayer settings as...",
            Filter = "JavaScript Object Notation file (*.json)|*.json",
        };

        if (!sfd.ShowDialog(DialogResult.OK)) {
            return;
        }

        try {
            MultiplayerArguments? instance = this.GenerateMultiplayer(nullNonMultiplayerGameMode: true);

            if (instance?.IsConfigDefault() != false) {
                return;
            }

            string mpInstanceString = JsonSerializer.Serialize(instance);
            File.WriteAllText(sfd.FileName, mpInstanceString);
        }
        catch { }
    }

    private void mGenerateArguments_Click(object? sender, EventArgs e) {
        if (cbSourcePorts.SelectedItem is not LoadedPort port) {
            return;
        }

        DoomArguments? args = this.GenerateArguments(port, tvIWADs.SelectedNode?.Tag is LoadedInternal liwad && liwad.FileExists ? liwad : null);

        if (args is null) {
            return;
        }

        using frmArguments showArgs = new(args);
        showArgs.ShowDialog();
    }
    private void mScanDoomPackage_Click(object? sender, EventArgs e) {
        using OpenFileDialog ofd = new() {
            Title = "Select a file to scan...",
            Filter = "All files (*.*)|*.*",
            Multiselect = false,
        };

        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FileInfo file = new(ofd.FileName);
        if (!file.Exists || file.Length < 1) {
            return;
        }

        using frmPackageExplorer display = new(file);
        display.ShowDialog();
    }
    private void mSaveSettings_Click(object? sender, EventArgs e) {
        this.SaveConfig(forceSave: true);
    }

    private void mAddExistingFile_Click(object? sender, EventArgs e) {
        if (this.SelectedTreeView is null) {
            return;
        }

        TreeView treeView = this.SelectedTreeView;
        this.SelectedTreeView = null;

        if (treeView == tvExternalFiles) {
            this.AddExternalFiles(parent: null, multiSelect: true);
            return;
        }

        if (treeView == tvIWADs || treeView == tvConfigIWADs) {
            this.AddIWAD(selectAfterAdd: false, multiSelect: true);
            return;
        }

        if (treeView == tvConfigSourcePorts) {
            this.AddSourcePort(selectAfterAdd: false, multiSelect: true);
        }
    }
    private void mAddExistingFilesAsDependant_Click(object? sender, EventArgs e) {
        if (tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external) {
            return;
        }

        this.AddExternalDirectoriesOrGroups(parent: external, multiSelect: true);
    }
    private void mEditSelectedFile_Click(object? sender, EventArgs e) {
        if (this.SelectedTreeView is null) {
            return;
        }

        TreeView treeView = this.SelectedTreeView;
        this.SelectedTreeView = null;

        if (treeView == tvExternalFiles) {
            if (treeView.SelectedNode?.Tag is LoadedExternal external) {
                this.EditExternalFile(external);
            }
            return;
        }

        if (treeView == tvIWADs) {
            if (tvIWADs.SelectedNode?.Tag is LoadedInternal iwad) {
                this.EditIWad(iwad);
            }
            return;
        }

        if (treeView == tvConfigIWADs) {
            if (tvConfigIWADs.SelectedNode?.Tag is LoadedInternal iwad) {
                this.EditIWad(iwad);
            }
            return;
        }

        if (treeView == tvConfigSourcePorts) {
            if (tvConfigSourcePorts.SelectedNode?.Tag is LoadedPort port) {
                this.EditSourcePort(port);
            }
            return;
        }
    }
    private void mScanSelectedFile_Click(object? sender, EventArgs e) {
        if (this.SelectedTreeView is null) {
            return;
        }
        TreeView treeView = this.SelectedTreeView;
        this.SelectedTreeView = null;

        if (treeView == tvExternalFiles) {
            (tvExternalFiles.SelectedNode?.Tag as LoadedExternal)?.ScanExternalFile();
            return;
        }

        if (treeView == tvIWADs) {
            (tvIWADs.SelectedNode?.Tag as LoadedInternal)?.ScanIWAD();
            return;
        }

        if (treeView == tvConfigIWADs) {
            (tvConfigIWADs.SelectedNode?.Tag as LoadedInternal)?.ScanIWAD();
        }
    }
    private void mRemoveSelectedItem_Click(object? sender, EventArgs e) {
        if (this.SelectedTreeView is null) {
            return;
        }
        TreeView treeView = this.SelectedTreeView;
        this.SelectedTreeView = null;

        if (treeView == tvExternalFiles) {
            if (treeView.SelectedNode?.Tag is not LoadedExternal external) {
                return;
            }
            this.IgnoreExternalFileEvents = true;
            this.RemoveExternalFile(external);
            this.IgnoreExternalFileEvents = false;
            treeView.SelectedNode = null;
            return;
        }

        if (treeView == tvIWADs || treeView == tvConfigIWADs) {
            if (treeView.SelectedNode?.Tag is not LoadedInternal iwad) {
                return;
            }
            this.IgnoreIWADEvents = true;
            this.RemoveIWad(iwad);
            this.IgnoreIWADEvents = false;
            treeView.SelectedNode = null;
            return;
        }

        if (treeView == tvConfigSourcePorts) {
            if (treeView.SelectedNode?.Tag is not LoadedPort port) {
                return;
            }
            this.IgnoreSourcePortEvents = true;
            this.RemoveSourcePort(port);
            this.IgnoreSourcePortEvents = false;
            treeView.SelectedNode = null;
            return;
        }
    }
    private void mBrowseToFileOrDirectory_Click(object? sender, EventArgs e) {
        if (this.SelectedTreeView is null) {
            return;
        }

        TreeView treeView = this.SelectedTreeView;
        this.SelectedTreeView = null;

        if (treeView == tvExternalFiles) {
            if (tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external || !external.IsFile || !external.IsDirectory) {
                return;
            }

            if (external.FileExists) {
                external.LoadWadInfo();
                RunProcess.Show(external.File);
                return;
            }

            if (external.DirectoryExists) {
                RunProcess.Show(external.Directory);
            }

            return;
        }

        if (treeView == tvIWADs) {
            if (tvIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
                return;
            }

            if (!iwad.FileExists) {
                return;
            }

            iwad.LoadWadInfo();
            RunProcess.Show(iwad.File);
            return;
        }

        if (treeView == tvConfigIWADs) {
            if (tvConfigIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
                return;
            }

            if (!iwad.FileExists) {
                return;
            }

            iwad.LoadWadInfo();
            RunProcess.Show(iwad.File);
            return;
        }

        if (treeView == tvConfigSourcePorts) {
            if (tvConfigIWADs.SelectedNode?.Tag is not LoadedPort port) {
                return;
            }

            if (!port.FileExists) {
                return;
            }

            port.LoadPortInfo();
            RunProcess.Show(port.File);
        }
    }
    #endregion Menus

    #region DragDrop support
    private void InitializeDragDrop() {
        tvExternalFiles.BeforeNodeDrag += (_, e) => e.Effect = ModifierKeys.HasFlag(Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move;
        tvExternalFiles.BeforeNodeDragHover += (_, e) => e.Effect = ModifierKeys.HasFlag(Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move;
        tvExternalFiles.AfterNodeDrag += (_, _) => this.OnConfigSaveRequired();
        tvExternalFiles.BeforeFileDragHover += (_, e) => e.Effect = ModifierKeys.HasFlag(Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move;
        tvExternalFiles.FileDrop += this.External_FilesDropped;

        cbSourcePorts.FileDrag += (_, e) => e.Effect = (e.Cancel = !e.Paths.Any(x => File.Exists(x))) ? DragDropEffects.None : DragDropEffects.Move;
        cbSourcePorts.FileDrop += this.Ports_FilesDropped;

        tvIWADs.BeforeNodeDrag += (_, e) => e.Effect = (e.Cancel = e.TreeNode.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvIWADs.BeforeNodeDragHover += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvIWADs.BeforeNodeDragMove += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvIWADs.AfterNodeDrag += this.IWAD_AfterNodeDraggedMoves;
        tvIWADs.FileDrag += (_, e) => e.Effect = (e.Cancel = !e.Paths.Any(x => File.Exists(x))) ? DragDropEffects.None : DragDropEffects.Move;
        tvIWADs.BeforeFileDragHover += (_, e) => e.Cancel = !e.Paths.Any(x => File.Exists(x));
        tvIWADs.FileDrop += this.IWADs_FilesDropped;

        tvConfigIWADs.BeforeNodeDrag += (_, e) => e.Effect = (e.Cancel = e.TreeNode.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigIWADs.BeforeNodeDragHover += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigIWADs.BeforeNodeDragMove += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigIWADs.AfterNodeDrag += this.IWADConfig_AfterNodeDraggedMoves;
        tvConfigIWADs.BeforeFileDragHover += (_, e) => e.Cancel = !e.Paths.Any(x => File.Exists(x));
        tvConfigIWADs.FileDrop += this.IWADs_FilesDropped;

        tvConfigSourcePorts.BeforeNodeDrag += (_, e) => e.Effect = (e.Cancel = e.TreeNode.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigSourcePorts.BeforeNodeDragHover += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigSourcePorts.BeforeNodeDragMove += (_, e) => e.Effect = (e.Cancel = e.HoveredTreeNode?.Index is not > 0) ? DragDropEffects.None : DragDropEffects.Move;
        tvConfigSourcePorts.AfterNodeDrag += this.Port_AfterNodeDraggedMoves;
        tvConfigSourcePorts.BeforeFileDragHover += (_, e) => e.Cancel = !e.Paths.Any(x => File.Exists(x));
        tvConfigSourcePorts.FileDrop += this.Ports_FilesDropped;

        cbMultiplayerPlayers.LostFocus += this.cbMultiplayerPlayers_SelectedIndexChanged;
        cbMultiplayerNetMode.SelectedIndexChanged += (_, _) => this.OnConfigSaveRequired();
        cbMultiplayerDup.SelectedIndexChanged += (_, _) => this.OnConfigSaveRequired();
        cbMultiplayerExtratic.SelectedIndexChanged += (_, _) => this.OnConfigSaveRequired();
        txtMultiplayerHostnameIp.TextChanged += (_, _) => this.OnConfigSaveRequired();

        btnPlay.FileDrag += (_, e) => e.Effect = (e.Cancel = !e.Paths.Any(x => File.Exists(x))) ? DragDropEffects.None : DragDropEffects.Move;
        btnPlay.FileDrop += this.Launch_FilesDropped;
        btnMultiplayer.FileDrag += (_, e) => e.Effect = (e.Cancel = !e.Paths.Any(x => File.Exists(x))) ? DragDropEffects.None : DragDropEffects.Move;
        btnMultiplayer.FileDrop += this.Launch_FilesDropped;
    }

    private void IWAD_AfterNodeDraggedMoves(object? sender, TreeNodeMoveEventArgs e) {
        if (e.TreeNode?.Tag is not LoadedInternal iwad || iwad.TreeNodeConfig is not TreeNode nodeConfig) {
            return;
        }

        this.IgnoreIWADEvents = true;
        TreeNode? lastSelected = tvIWADs.SelectedNode;
        tvConfigIWADs.Nodes.Remove(nodeConfig);
        tvConfigIWADs.Nodes.Insert(e.TreeNode.Index, nodeConfig);
        tvConfigIWADs.SelectedNode = lastSelected;
        this.IgnoreIWADEvents = false;

        this.OnConfigSaveRequired();
    }
    private void IWADConfig_AfterNodeDraggedMoves(object? sender, TreeNodeMoveEventArgs e) {
        if (e.TreeNode?.Tag is not LoadedInternal iwad || iwad.TreeNode is not TreeNode node) {
            return;
        }

        this.IgnoreIWADEvents = true;
        TreeNode? lastSelected = tvConfigIWADs.SelectedNode;
        tvIWADs.Nodes.Remove(node);
        tvIWADs.Nodes.Insert(e.TreeNode.Index, node);
        tvIWADs.SelectedNode = lastSelected;
        this.IgnoreIWADEvents = false;
        this.OnConfigSaveRequired();
    }
    private void Port_AfterNodeDraggedMoves(object? sender, TreeNodeMoveEventArgs e) {
        if (e.TreeNode?.Tag is not LoadedPort port) {
            return;
        }

        object? lastSelectedPort = cbSourcePorts.SelectedItem;

        this.IgnoreSourcePortEvents = true;
        cbSourcePorts.Items.Remove(port);
        cbSourcePorts.Items.Insert(e.TreeNode.Index + 1, port);
        cbSourcePorts.SelectedItem = lastSelectedPort;
        this.IgnoreSourcePortEvents = false;

        this.OnConfigSaveRequired();
    }

    private void External_FilesDropped(object? sender, TreeNodeFileDroppedEventArgs e) {
        bool pathsAdded = false;
        bool refreshMaps = false;

        LoadedExternal? parent = ModifierKeys.HasFlag(Keys.Control) ? e.Node?.Tag as LoadedExternal : null;

        e.Paths?.WhereFor(path => !path.IsNullEmptyWhitespace(), path => {
            var external = this.AddExternal(path: path, parent: parent, externalType: AddExternalType.Unknown);

            if (external is not null) {
                pathsAdded = true;

                if (external.Enabled && (external.IsFile || external.IsGroup) && external.UpdateAllMaps() > 0) {
                    refreshMaps = true;
                }
            }
        });

        if (!pathsAdded) {
            return;
        }

        this.OnConfigSaveRequired();
        this.RefreshExternalFiles();

        if (refreshMaps) {
            this.RefreshMaps();
        }
    }
    private void IWADs_FilesDropped(object? sender, TreeNodeFileDroppedEventArgs e) {
        for (int i = 0; i < e.Paths.Length; i++) {
            string path = e.Paths[i];

            if (!File.Exists(path)) {
                continue;
            }

            this.AddIWad(filePath: path, selectAfterAdd: sender == tvIWADs);
        }
    }
    private void Ports_FilesDropped(object? sender, FileDroppedEventArgs e) {
        for (int i = 0; i < e.Paths.Length; i++) {
            string path = e.Paths[i];

            if (!File.Exists(path)) {
                continue;
            }

            this.AddSourcePort(filePath: path, selectAfterAdd: sender == cbSourcePorts);
        }
    }
    private void Launch_FilesDropped(object? sender, FileDroppedEventArgs e) {
        if (e.Paths?.Length is not > 0) {
            return;
        }

        this.LaunchFromFiles(paths: e.Paths,
            iwad: tvIWADs.SelectedNode?.Tag as LoadedInternal,
            firstPwadAsIwad: ModifierKeys.HasFlag(Keys.Control));
    }
    #endregion DragDrop support

    public frmMain() {
        this.InitializeComponent();
        this.InitializeMenus();
        this.InitializeDragDrop();

        this.StartPosition = FormStartPosition.CenterScreen;

        tvIWADs.Nodes.Add(InternalWadBrowseNode);
        tvConfigIWADs.Nodes.Add(InternalWadConfigBrowseNode);
        tvConfigSourcePorts.Nodes.Add(PortConfigBrowseNode);

        tvIWADs.ShowRootLines = false;
        tvConfigIWADs.ShowRootLines = false;
        tvConfigSourcePorts.ShowRootLines = false;

        cbSkillLevel.SelectedIndex = 0;
        cbMaps.Text = string.Empty;
        cbMultiplayerGameMode.SelectedIndex = 0;
        cbMultiplayerPlayers.SelectedIndex = 0;
        cbMultiplayerNetMode.SelectedIndex = 0;
        cbMultiplayerDup.SelectedIndex = 0;
        cbMultiplayerExtratic.SelectedIndex = 0;

        cbMaps.LostFocus += (_,_) => {
            if (cbMaps.SelectedIndex > 0 && !cbMaps.Text.IsNullEmptyWhitespace()) {
                return;
            }
            cbMaps.Text = string.Empty;
        };
        cbCommandLineArguments.LostFocus += (_,_) => {
            if (cbCommandLineArguments.SelectedIndex > 0 && !cbCommandLineArguments.Text.IsNullEmptyWhitespace()) {
                return;
            }
            cbCommandLineArguments.Text = string.Empty;
        };
        cbMultiplayerSaveGame.LostFocus += (_,_) => {
            if (cbMultiplayerSaveGame.SelectedIndex > 0 && !cbMultiplayerSaveGame.Text.IsNullEmptyWhitespace()) {
                return;
            }
            cbMultiplayerSaveGame.Text = string.Empty;
        };

        this.Load += (_, _) => {
            if (!Configuration.ReloadConfig()) {
                return;
            }
            this.LoadConfig();

            if (!this.DesignMode) {
                this.ResizeControls();
                this.Move += (_, _) => this.OnConfigSaveRequired();
                this.ResizeBegin += (_,_) => this.LastSize = this.Size;
                this.Resize += (_, _) => this.ResizeControls();
                this.ResizeEnd += (_, _) => {
                    if (this.LastSize == this.Size) {
                        return;
                    }
                    this.OnConfigSaveRequired();
                    this.ResizeControls();
                    this.LastSize = this.Size;
                };
            }
        };
        this.Shown += (_, _) => {
            if (tvIWADs.SelectedNode?.Index > (pnMultiplayer.Visible ? 4 : 6)) {
                tvIWADs.Nodes[^1].EnsureVisible();
                tvIWADs.SelectedNode.EnsureVisible();
            }
            this.SaveConfigTimer.Start();
            btnPlay.Focus();
            this.OnConfigSaved();
        };
        this.FormClosing += (_, _) => {
            this.SaveConfigTimer.Stop();
            this.SaveConfig(forceSave: false);
        };
        this.SaveConfigTimer.Tick += (_, _) => {
            Debug.WriteLine("Save config timer ticked");
            if (!this.SaveConfigFile) {
                return;
            }
            this.SaveConfig(forceSave: false);
        };
    }

    private void OnConfigSaveRequired() {
        this.SaveConfigFile = true;
        lbModified.Visible = true;
        mSaveSettings.Enabled = true;
    }
    private void OnConfigSaved() {
        this.SaveConfigFile = false;
        lbModified.Visible = false;
        mSaveSettings.Enabled = false;
    }
    private void SaveConfig(bool forceSave) {
        if (!this.SaveConfigFile && !forceSave) {
            return;
        }

        var config = Configuration.Default;

        config.Ports.Clear();
        config.IWADs.Clear();
        config.Externals.Clear();

        if (tvConfigSourcePorts.Nodes.Count > 0) {
            List<LoadedPort> ports = [];
            for (int i = 0; i < tvConfigSourcePorts.Nodes.Count; i++) {
                if (tvConfigSourcePorts.Nodes[i].Tag is not LoadedPort port) {
                    continue;
                }
                ports.Add(port);
            }

            if (ports.Count > 0) {
                config.Ports = ports;
            }
        }

        if (tvConfigIWADs.Nodes.Count > 0) {
            List<LoadedInternal> iwads = [];
            for (int i = 0; i < tvConfigIWADs.Nodes.Count; i++) {
                if (tvConfigIWADs.Nodes[i].Tag is not LoadedInternal iwad) {
                    continue;
                }
                iwads.Add(iwad);
            }

            if (iwads.Count > 0) {
                config.IWADs = iwads;
            }
        }

        if (tvExternalFiles.Nodes.Count > 0) {
            List<LoadedExternal> externals = [];
            for (int i = 0; i < tvExternalFiles.Nodes.Count; i++) {
                if (tvExternalFiles.Nodes[i].Tag is not LoadedExternal external) {
                    continue;
                }
                externals.Add(external);
            }

            if (externals.Count > 0) {
                config.Externals = externals;
            }
        }

        string? currentCommandLineArg = cbCommandLineArguments.SelectedIndex != 0 && !cbCommandLineArguments.Text.IsNullEmptyWhitespace() ? cbCommandLineArguments.Text.Trim() : null;
        if (currentCommandLineArg != null && !cbCommandLineArguments.Items.Contains(currentCommandLineArg)) {
            cbCommandLineArguments.Items.Add(currentCommandLineArg);
        }

        if (cbCommandLineArguments.Items.Count > 1) {
            List<string> commandLineArgs = [];
            for (int i = 1; i < cbCommandLineArguments.Items.Count; i++) {
                string? arg = cbCommandLineArguments.Items[i]?.ToString();
                if (arg.IsNullEmptyWhitespace()) {
                    continue;
                }
                commandLineArgs.Add(arg);
            }

            if (commandLineArgs.Count > 0) {
                config.CommandLineArguments = commandLineArgs;
                if (currentCommandLineArg is not null) {
                    config.LastCommandLineArg = currentCommandLineArg;
                }
            }
        }

        string? currentMultiplayerSaveGame = cbMultiplayerSaveGame.SelectedIndex > 1 && !cbMultiplayerSaveGame.Text.IsNullEmptyWhitespace() ? cbMultiplayerSaveGame.Text : null;
        if (currentMultiplayerSaveGame != null && !cbMultiplayerSaveGame.Items.Contains(currentMultiplayerSaveGame)) {
            cbMultiplayerSaveGame.Items.Add(currentMultiplayerSaveGame);
        }

        if (cbMultiplayerSaveGame.Items.Count > 2) {
            List<string> multiplayerSaveFiles = [];
            for (int i = 2; i < cbMultiplayerSaveGame.Items.Count; i++) {
                string? saveFile = cbMultiplayerSaveGame.Items[i]?.ToString();
                if (saveFile.IsNullEmptyWhitespace()) {
                    continue;
                }
                multiplayerSaveFiles.Add(saveFile);
            }

            if (multiplayerSaveFiles.Count > 0) {
                config.MultiplayerSaveFiles = multiplayerSaveFiles;
                if (currentMultiplayerSaveGame is not null) {
                    config.LastMultiplayerSaveFile = currentMultiplayerSaveGame;
                }
            }
        }

        config.FormLocation = this.Location;
        config.FormSize = this.Size;
        config.LastLoadedIWAD = tvIWADs.SelectedNode?.Tag is LoadedInternal liwad ? liwad.Name : null;
        config.LastLoadedPort = cbSourcePorts.SelectedItem is LoadedPort lport ? lport.Name : null;
        config.LastMap = cbMaps.SelectedIndex != 0 ? cbCommandLineArguments.Text.UnlessNullEmptyWhitespace(null!) : null;
        config.LastCommandLineArg = cbCommandLineArguments.SelectedIndex != 0 ? cbCommandLineArguments.Text.UnlessNullEmptyWhitespace(null!) : null;
        config.LastMultiplayerSaveFile = cbMultiplayerSaveGame.SelectedIndex is not 0 and not 1 ? cbMultiplayerSaveGame.Text.UnlessNullEmptyWhitespace(null!) : null;
        config.ShowMultiplayer = pnMultiplayer.Visible;
        config.EnabledPWADsLaunchWithDroppedFiles = mEnabledPWADsLaunchWIthDroppedFiles.Checked;
        Configuration.SaveConfig();
        this.OnConfigSaved();
    }
    private void LoadConfig() {
        tvConfigIWADs.Nodes.Clear();
        tvConfigSourcePorts.Nodes.Clear();
        tvIWADs.Nodes.Clear();
        tvExternalFiles.Nodes.Clear();
        cbSourcePorts.Items.Clear();
        cbCommandLineArguments.Items.Clear();
        cbMultiplayerSaveGame.Items.Clear();

        tvIWADs.Nodes.Add(InternalWadBrowseNode);
        tvConfigIWADs.Nodes.Add(InternalWadConfigBrowseNode);
        tvConfigSourcePorts.Nodes.Add(PortConfigBrowseNode);
        cbSourcePorts.Items.Add("(Browse)");
        cbCommandLineArguments.Items.Add("(None)");
        cbMultiplayerSaveGame.Items.Add("(None)");
        cbMultiplayerSaveGame.Items.Add("(Browse)");
        cbMultiplayerSaveGame.SelectedIndex = -1;

        bool saveConfig = false;
        var config = Configuration.Default;

        if (config.IWADs?.Count > 0) {
            foreach (var iwad in config.IWADs) {
                TreeNode nodeMain = new(iwad.Name) {
                    Tag = iwad,
                    ToolTipText = iwad.ToolTipText,
                };
                iwad.TreeNode = nodeMain;
                tvIWADs.Nodes.Add(nodeMain);

                TreeNode nodeConfig = new(iwad.Name) {
                    Tag = iwad,
                    ToolTipText = iwad.ToolTipText,
                };
                iwad.TreeNodeConfig = nodeConfig;
                tvConfigIWADs.Nodes.Add(nodeConfig);

                if (tvIWADs.SelectedNode is null && iwad.IsNameEqual(config.LastLoadedIWAD)) {
                    tvIWADs.SelectedNode = nodeMain;
                }
            }
        }

        if (config.Ports?.Count > 0) {
            foreach (var port in config.Ports) {
                TreeNode node = new(port.Name) {
                    Tag = port,
                    ToolTipText = port.ToolTipText,
                };
                port.TreeNode = node;
                tvConfigSourcePorts.Nodes.Add(node);
                cbSourcePorts.Items.Add(port);

                if (cbSourcePorts.SelectedItem is null && port.IsNameEqual(config.LastLoadedPort)) {
                    cbSourcePorts.SelectedIndex = cbSourcePorts.Items.Count - 1;
                }
            }
        }

        if (config.Externals?.Count > 0) {
            foreach (var external in config.Externals) {
                TreeNode node = new(external.Name) {
                    Tag = external,
                    ToolTipText = external.ToolTipText,
                    Checked = external.Enabled,
                };

                if (external.Dependants?.Count > 0) {
                    for (int i = 0; i < external.Dependants.Count; i++) {
                        var dependant = external.Dependants[i];
                        TreeNode dependantNode = new(dependant.Name) {
                            Tag = dependant,
                            ToolTipText = dependant.ToolTipText,
                            Checked = dependant.Enabled,
                        };
                        node.Nodes.Add(dependantNode);
                        dependant.TreeNode = dependantNode;
                    }
                }

                external.TreeNode = node;
                tvExternalFiles.Nodes.Add(node);
            }
            this.RefreshExternalFiles();
        }

        string? lastCommandArgFound = !config.LastCommandLineArg.IsNullEmptyWhitespace() ? config.LastCommandLineArg.Trim() : null;
        if (config.CommandLineArguments?.Count > 0) {
            foreach (var arg in config.CommandLineArguments.Select(x => x.Trim()).Where(x => !x.IsNullEmptyWhitespace())) {
                if (cbCommandLineArguments.Items.Contains(arg)) {
                    continue;
                }

                cbCommandLineArguments.Items.Add(arg);
                if (lastCommandArgFound is not null && arg.Equals(lastCommandArgFound, StringComparison.InvariantCultureIgnoreCase)) {
                    cbCommandLineArguments.SelectedIndex = cbCommandLineArguments.Items.Count - 1;
                    lastCommandArgFound = null;
                }
            }
        }

        if (lastCommandArgFound is not null) {
            cbCommandLineArguments.Items.Add(lastCommandArgFound);
            cbCommandLineArguments.SelectedIndex = cbCommandLineArguments.Items.Count - 1;
            saveConfig = true;
            this.OnConfigSaveRequired();
        }

        string? lastMultiplayerSavesFound = !config.LastMultiplayerSaveFile.IsNullEmptyWhitespace() ? config.LastMultiplayerSaveFile.Trim() : null;
        if (config.MultiplayerSaveFiles?.Count > 0) {
            foreach (var arg in config.MultiplayerSaveFiles.Select(x => x.Trim()).Where(x => !x.IsNullEmptyWhitespace())) {
                if (cbMultiplayerSaveGame.Items.Contains(arg)) {
                    continue;
                }

                cbMultiplayerSaveGame.Items.Add(arg);
                if (lastMultiplayerSavesFound is not null && arg.Equals(config.LastMultiplayerSaveFile, StringComparison.InvariantCultureIgnoreCase)) {
                    cbMultiplayerSaveGame.SelectedIndex = cbMultiplayerSaveGame.Items.Count - 1;
                    lastMultiplayerSavesFound = null;
                }
            }
        }

        if (lastMultiplayerSavesFound is not null) {
            cbMultiplayerSaveGame.Items.Add(lastMultiplayerSavesFound);
            cbMultiplayerSaveGame.SelectedIndex = cbMultiplayerSaveGame.Items.Count - 1;
            saveConfig = true;
            this.OnConfigSaveRequired();
        }

        pnMultiplayer.Visible = config.ShowMultiplayer;
        mEnabledPWADsLaunchWIthDroppedFiles.Checked = config.EnabledPWADsLaunchWithDroppedFiles;

        if (config.MultiplayerInstance is not null) {
            cbMultiplayerGameMode.SelectedIndex = (int)config.MultiplayerInstance.GameMode;
            cbMultiplayerNetMode.SelectedIndex = (int)config.MultiplayerInstance.NetMode;
            cbMultiplayerPlayers.SelectedIndex = (int)config.MultiplayerInstance.Players;
            txtMultiplayerHostnameIp.Text = config.MultiplayerInstance.HostnameIp;
            txtMultiplayerPort.Text = config.MultiplayerInstance.Port;
            txtMultiplayerFragLimit.Text = config.MultiplayerInstance.FragLimit.ToString();
            txtMultiplayerTimeLimit.Text = config.MultiplayerInstance.TimeLimit.ToString();
            txtMultiplayerDMFLAGS.Text = config.MultiplayerInstance.DMFLAGS.ToString();
            txtMultiplayerDMFLAGS2.Text = config.MultiplayerInstance.DMFLAGS2.ToString();
            cbMultiplayerDup.SelectedIndex = (int)config.MultiplayerInstance.Dup;
            cbMultiplayerExtratic.SelectedIndex = config.MultiplayerInstance.Extratic ? 1 : 0;

            int index = cbMultiplayerSaveGame.Items.IndexOf(config.MultiplayerInstance.Savegame);
            cbMultiplayerSaveGame.SelectedIndex = index > 1 ? index : 0;
        }
        else {
            cbMultiplayerGameMode.SelectedIndex = 0;
            cbMultiplayerNetMode.SelectedIndex = 0;
            cbMultiplayerPlayers.SelectedIndex = 0;
            txtMultiplayerHostnameIp.Text = string.Empty;
            txtMultiplayerPort.Text = string.Empty;
            txtMultiplayerFragLimit.Text = string.Empty;
            txtMultiplayerTimeLimit.Text = string.Empty;
            txtMultiplayerDMFLAGS.Text = string.Empty;
            txtMultiplayerDMFLAGS2.Text = string.Empty;
            cbMultiplayerDup.SelectedIndex = 0;
            cbMultiplayerExtratic.SelectedIndex = 0;
            cbMultiplayerSaveGame.SelectedIndex = 0;
        }

        if (config.FormLocation != default) {
            this.Location = config.FormLocation;
        }

        if (config.FormSize != default) {
            this.Size = config.FormSize;
        }

        if (!config.LastMap.IsNullEmptyWhitespace()) {
            cbMaps.Text = config.LastMap;
        }

        this.RefreshMaps();

        if (saveConfig) {
            this.SaveConfig(forceSave: false);
            return;
        }

        this.OnConfigSaved();
    }
    private void LoadFromZdlIni(string ini) {
        if (!Import.TryImportZdl(Configuration.Default, ini)) {
            return;
        }

        this.LoadConfig();
    }
    private void LoadMultiplayerSettings(MultiplayerArguments instance) {
        cbMultiplayerGameMode.SelectedIndex = instance.GameMode switch {
            MultiplayerGameMode.Coop => 1,
            MultiplayerGameMode.Deathmatch => 2,
            MultiplayerGameMode.AltDeathmatch => 3,
            _ => 0,
        };

        cbMultiplayerNetMode.SelectedIndex = instance.NetMode switch {
            MultiplayerNetMode.ClassicP2P => 1,
            MultiplayerNetMode.ClientServer => 2,
            _ => 0,
        };

        if (instance.Players > 0) {
            cbMultiplayerPlayers.Text = instance.Players.ToString();
        }
        else {
            cbMultiplayerPlayers.SelectedIndex = 0;
        }

        txtMultiplayerHostnameIp.Text = instance.HostnameIp;
        txtMultiplayerPort.Text = instance.Port;
        if (instance.FragLimit > 0) {
            txtMultiplayerFragLimit.Text = instance.FragLimit.ToString();
        }
        if (instance.TimeLimit > 0) {
            txtMultiplayerTimeLimit.Text = instance.TimeLimit.ToString();
        }
        if (instance.DMFLAGS > 0) {
            txtMultiplayerDMFLAGS.Text = instance.DMFLAGS.ToString();
        }
        if (instance.DMFLAGS2 > 0) {
            txtMultiplayerDMFLAGS2.Text = instance.DMFLAGS2.ToString();
        }

        if (instance.Dup > 9) {
            cbMultiplayerDup.SelectedIndex = 9;
        }
        else if (instance.Dup < 1) {
            cbMultiplayerDup.SelectedIndex = 0;
        }
        else {
            cbMultiplayerDup.SelectedIndex = (int)instance.Dup;
        }

        cbMultiplayerExtratic.SelectedIndex = instance.Extratic ? 1 : 0;

        if (!instance.Savegame.IsNullEmptyWhitespace()) {
            int saveIndex = cbMultiplayerSaveGame.Items.IndexOf(instance.Savegame);
            if (saveIndex > -1) {
                cbMultiplayerSaveGame.SelectedIndex = saveIndex;
            }
            else {
                cbMultiplayerSaveGame.Text = instance.Savegame;
            }
        }
        else {
            cbMultiplayerSaveGame.SelectedIndex = 0;
        }

        this.OnConfigSaveRequired();
    }
    private DoomArguments? GenerateArguments(LoadedPort? port, LoadedInternal? iwad, bool skipExternal = false) {
        List<LoadedExternal> external = [];

        if (!skipExternal) {
            for (int i = 0; i < tvExternalFiles.Nodes.Count; i++) {
                if (tvExternalFiles.Nodes[i].Tag is not LoadedExternal wad || !wad.Enabled) {
                    continue;
                }
                external.Add(wad);
            }
        }

        MultiplayerArguments? mpSettings = this.GenerateMultiplayer(nullNonMultiplayerGameMode: true);

        if (mpSettings?.Savegame.IsNullEmptyWhitespace() == false && !cbMultiplayerSaveGame.Items.Contains(mpSettings.Savegame)) {
            cbMultiplayerSaveGame.Items.Add(mpSettings.Savegame);
            this.OnConfigSaveRequired();
        }

        string? commandLineArgs = null;

        if (cbCommandLineArguments.SelectedIndex != 0 && !cbCommandLineArguments.Text.IsNullEmptyWhitespace()) {
            commandLineArgs = cbCommandLineArguments.Text.Trim();
            if (!cbCommandLineArguments.Items.Contains(commandLineArgs)) {
                cbCommandLineArguments.Items.Add(commandLineArgs);
                this.OnConfigSaveRequired();
            }
        }

        return new DoomArguments(
            port: port,
            iwad: iwad,
            externalFiles: external,
            multiplayer: mpSettings,
            commandLineArgs: commandLineArgs,
            skill: cbSkillLevel.SelectedIndex,
            map: cbMaps.SelectedIndex == 0 || cbMaps.Text.IsNullEmptyWhitespace() ? null : cbMaps.Text);
    }
    private MultiplayerArguments? GenerateMultiplayer(bool nullNonMultiplayerGameMode) {
        MultiplayerGameMode gameMode = cbMultiplayerGameMode.SelectedIndex switch {
            1 => MultiplayerGameMode.Coop,
            2 => MultiplayerGameMode.Deathmatch,
            3 => MultiplayerGameMode.AltDeathmatch,
            _ => MultiplayerGameMode.Singleplayer,
        };

        return nullNonMultiplayerGameMode && gameMode == MultiplayerGameMode.Singleplayer ? null : new MultiplayerArguments(gameMode: gameMode) {
            NetMode = cbMultiplayerNetMode.SelectedIndex switch {
                1 => MultiplayerNetMode.ClassicP2P,
                2 => MultiplayerNetMode.ClientServer,
                _ => MultiplayerNetMode.Default,
            },

            Players = uint.TryParse(cbMultiplayerPlayers.Text, out uint pl) ? pl : 0,
            HostnameIp = txtMultiplayerHostnameIp.Text.IsNullEmptyWhitespace() ? null : txtMultiplayerHostnameIp.Text,
            Port = txtMultiplayerPort.Text.IsNullEmptyWhitespace() ? null : txtMultiplayerPort.Text,

            FragLimit = uint.TryParse(txtMultiplayerFragLimit.Text, out uint fl) ? fl : 0,
            TimeLimit = uint.TryParse(txtMultiplayerTimeLimit.Text, out uint tl) ? tl : 0,
            DMFLAGS = uint.TryParse(txtMultiplayerDMFLAGS.Text, out uint dw) ? dw : 0,
            DMFLAGS2 = uint.TryParse(txtMultiplayerDMFLAGS2.Text, out uint dw2) ? dw2 : 0,
            Dup = cbMultiplayerDup.SelectedIndex > 0 ? (uint)cbMultiplayerDup.SelectedIndex.Clamp(0, 9) : 0u,
            Extratic = cbMultiplayerExtratic.SelectedIndex == 1,
            Savegame = cbMultiplayerSaveGame.SelectedIndex > 1 && !cbMultiplayerSaveGame.Text.IsNullEmptyWhitespace() ? cbMultiplayerSaveGame.Text : null,
        };
    }
    private static bool RequireNewName<T>(TreeView treeView, T? skipped, string existingMessage, frmAddItem editor) where T : LoadedInfo {
        if (!editor.ShowDialog(DialogResult.OK)) {
            return false;
        }

        bool anyExistingNames() {
            for (int i = 0; i < treeView.Nodes.Count; i++) {
                if (treeView.Nodes[i].Tag is not T other || other == skipped) {
                    continue;
                }

                if (other.IsNameEqual(editor.NewName)) {
                    return true;
                }
            }

            return false;
        }

        while (anyExistingNames()) {
            if (MessageBox.Show(existingMessage, "DOOMer", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                return false;
            }

            if (!editor.ShowDialog(DialogResult.OK)) {
                return false;
            }
        }

        return true;
    }
    private void Launch(DoomArguments args) {
#if DEBUG
        if (mDisplayArgsInsteadOfLaunch.Checked) {
            using frmArguments display = new(args);
            display.ShowDialog();
        }
#endif
        args.Launch();
    }
    private void LaunchFromFiles(string[] paths, LoadedInternal? iwad, bool firstPwadAsIwad) {
        string? portFile = null;
        InternalWad? foundPort = null;
        string? firstWad = null;
        List<string> pwads = [];
        List<string> dehacks = [];
        List<LoadedExternal>? externals = null;

        foreach (string path in paths.Select(x => x.Trim())) {
            if (!File.Exists(path)) {
                if (Directory.Exists(path)) {
                    pwads.Add($"\"{path}\"");
                }
                continue;
            }

            if (portFile is null
            && path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
            && foundPort is null
            && (foundPort = InternalWad.FindAnyInternalPort(fileName: Path.GetFileName(path))) is not null) {
                portFile = path;
                continue;
            }

            if (InternalWad.IsWadFile(path)) {
                pwads.Add($"\"{path}\"");
                if (firstPwadAsIwad) {
                    firstWad ??= path;
                }
                continue;
            }

            if (InternalWad.IsAnyDehack(path)) {
                dehacks.Add($"\"{path}\"");
            }
        }

        if (mEnabledPWADsLaunchWIthDroppedFiles.Checked && tvExternalFiles.Nodes.Count > 0) {
            externals = [];
            for (int i = 0; i < tvExternalFiles.Nodes.Count; i++) {
                if (tvExternalFiles.Nodes[i].Tag is not LoadedExternal external) {
                    continue;
                }
                externals.Add(external);
            }
        }

        bool AnyExternalFiles() => pwads.Count > 0
            || dehacks.Count > 0 || externals?.Count is not > 0
            || iwad is not null || firstWad is not null;

        DoomArguments? arguments;

        bool CreateArgs(LoadedPort? port, [NotNullWhen(true)] out DoomArguments? args) {
            args = this.GenerateArguments(port: port, iwad: iwad, skipExternal: !mEnabledPWADsLaunchWIthDroppedFiles.Checked);

            if (args is null) {
                return false;
            }

            args.ExternalPWADs = pwads;
            args.ExternalDehacks = dehacks;
            return true;
        }

        if (portFile is null || foundPort is null) {
            if (cbSourcePorts.SelectedItem is not LoadedPort selectedPort || !AnyExternalFiles() || !CreateArgs(port: selectedPort, out arguments)) {
                return;
            }
            portFile = selectedPort.Path;
        }
        else {
            if (!AnyExternalFiles() || !CreateArgs(port: null, out arguments)) {
                return;
            }

            using frmUnknownPortLaunch alert = new(filePath: portFile,
                port: foundPort,
                args: arguments,
                allowSavePort: false);

            if (!alert.ShowDialog(expectedResult: DialogResult.OK)) {
                arguments.Clear();
                return;
            }
        }

        arguments.LaunchWith(portFile);
    }

    private void RefreshExternalFiles() {
        int enabledCount = 0;
        int disabledCount = 0;

        for (int i = 0; i < tvExternalFiles.Nodes.Count; i++) {
            if (tvExternalFiles.Nodes[i].Checked) {
                enabledCount++;
                continue;
            }
            disabledCount++;
        }

        lbExternalFiles.Text = $"External files and directories ({enabledCount} / {disabledCount + enabledCount})";
    }
    private void RefreshMaps() {
        string? selectedMap = cbMaps.SelectedIndex != 0 && !cbMaps.Text.IsNullEmptyWhitespace() ? cbMaps.Text : null;
        if (cbMaps.Items.Count > 0) {
            cbMaps.Items.Clear();
        }

        List<string> maps = [];

        if (tvIWADs.SelectedNode?.Tag is LoadedInternal iwad) {
            iwad.UpdateMaps();
            iwad.Maps.For(maps.Add);
        }

        for (int i = 0; i < tvExternalFiles.Nodes.Count; i++) {
            if (tvExternalFiles.Nodes[i].Tag is not LoadedExternal external || !external.Enabled) {
                continue;
            }

            external.UpdateAllMaps();
            external.Maps.For(maps.Add);
        }

        maps.Sort(BetterStringComparer.Default);
        cbMaps.Items.Add("(Default)");
        cbMaps.Items.AddRange([.. maps]);

        if (selectedMap is not null) {
            int index = cbMaps.Items.IndexOf(selectedMap);
            if (index > 0) {
                cbMaps.SelectedIndex = index;
            }
        }

        lbExternalFiles.Focus();
    }
    private void ResizeControls() {
        // ( ( Panel width - Margin * 2 (6) ) / 2 Combo Boxes ) - Combo Box Margin
        int width = ((pnInternalWADs.Width - 6) / 2) - 1;

        static void AddAbove(Control topControl, Control bottomControl) {
            topControl.Location = new(bottomControl.Location.X, bottomControl.Location.Y - topControl.Height);
        }
        static void MoveNextTo(Control rightControl, Control leftControl, Control rightControlLabel) {
            rightControl.Location = new(leftControl.Location.X + leftControl.Width + 2, rightControl.Location.Y);
            AddAbove(topControl: rightControlLabel, bottomControl: rightControl);
        }

        cbMaps.Width = width;
        cbMaps.Location = new(3, pnInternalWADs.Height - cbMaps.Height - 3);
        AddAbove(topControl: lbMaps, bottomControl: cbMaps);

        cbSkillLevel.Width = width;
        cbSkillLevel.Location = new(pnInternalWADs.Width - width - 3, pnInternalWADs.Height - cbSkillLevel.Height - 3);
        AddAbove(topControl: lbSkillLevel, bottomControl: cbSkillLevel);

        if (pnMultiplayer.Visible) {
            width = (int)Math.Floor((decimal)(((pnMultiplayer.Width - cbMultiplayerPlayers.Width - 6) / 4) - 1));

            txtMultiplayerFragLimit.Size = new(width, txtMultiplayerFragLimit.Height);
            txtMultiplayerTimeLimit.Size = new(width, txtMultiplayerTimeLimit.Height);
            txtMultiplayerDMFLAGS.Size = new(width, txtMultiplayerDMFLAGS.Height);
            txtMultiplayerDMFLAGS2.Size = new(width, txtMultiplayerDMFLAGS2.Height);

            MoveNextTo(rightControl: txtMultiplayerFragLimit, leftControl: cbMultiplayerPlayers, rightControlLabel: lbMultiplayerFragLimit);
            MoveNextTo(rightControl: txtMultiplayerTimeLimit, leftControl: txtMultiplayerFragLimit, rightControlLabel: lbMultiplayerTimeLimit);
            MoveNextTo(rightControl: txtMultiplayerDMFLAGS, leftControl: txtMultiplayerTimeLimit, rightControlLabel: lbMultiplayerDMFLAGS);

            int maxX = txtMultiplayerPort.Location.X + txtMultiplayerPort.Width;

            while (txtMultiplayerDMFLAGS.Location.X + txtMultiplayerDMFLAGS.Width + 2 + width > maxX) {
                width--;
            }

            txtMultiplayerDMFLAGS2.Size = new(width, txtMultiplayerDMFLAGS2.Height);
            MoveNextTo(rightControl: txtMultiplayerDMFLAGS2, leftControl: txtMultiplayerDMFLAGS, rightControlLabel: lbMultiplayerDMFLAGS2);
        }
    }

    private void btnImportZdl_Click(object? sender, EventArgs e) {
        string globalIni = Configuration.ZdlGlobalIni;

        using OpenFileDialog ofd = new() {
            Title = "Select ZDL.ini file to import...",
            Filter = "Initialization file (*.ini)|*.ini|All files (*.*)|*.*",
            Multiselect = false,
            FileName = Path.GetFileName(globalIni),
            InitialDirectory = Path.GetDirectoryName(globalIni),
        };

        if (!ofd.ShowDialog(DialogResult.OK) || !File.Exists(ofd.FileName)) {
            return;
        }

        this.LoadFromZdlIni(ofd.FileName);
    }
    private void txtNumericOnly_KeyDown(object? sender, KeyEventArgs e) {
        if (e.Control || e.Alt) {
            return;
        }

        if (e.KeyCode is Keys.D0 or Keys.D1 or Keys.D2 or Keys.D3 or Keys.D4 or Keys.D5 or Keys.D6 or Keys.D7 or Keys.D8 or Keys.D9
        or Keys.NumPad0 or Keys.NumPad1 or Keys.NumPad2 or Keys.NumPad3 or Keys.NumPad4 or Keys.NumPad5 or Keys.NumPad6 or Keys.NumPad7 or Keys.NumPad8 or Keys.NumPad9) {
            if (sender == cbMultiplayerPlayers) {
                if (cbMultiplayerPlayers.SelectedIndex == 0) {
                    cbMultiplayerPlayers.SelectedIndex = -1;
                }
                btnPlay.Text = "Host";
            }
            this.OnConfigSaveRequired();
            return;
        }

        if (e.KeyCode == Keys.Back) {
            if (sender == cbMultiplayerPlayers) {
                if (cbMultiplayerPlayers.SelectedIndex == 0) {
                    cbMultiplayerPlayers.SelectedIndex = -1;
                }
                else if (cbMultiplayerPlayers.Text.Length == 1) {
                    cbMultiplayerPlayers.SelectedIndex = 0;
                }
                else {
                    btnPlay.Text = "Host";
                }
            }
            this.OnConfigSaveRequired();
            return;
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
    }
    private void tvRightClick_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e) {
        if (e.Button != MouseButtons.Right) {
            return;
        }

        this.SelectedTreeView = e.Node.TreeView;
        this.SelectedTreeView.SelectedNode = e.Node;

        if (this.SelectedTreeView.SelectedNode is null) {
            mAddExistingFilesAsDependant.Enabled = false;
            mAddExistingFilesAsDependant.Visible = this.SelectedTreeView == tvExternalFiles;
            mEditSelectedItem.Enabled = false;
            mScanSelectedFile.Enabled = false;
            mRemoveSelectedItem.Enabled = false;
            mBrowseToFileOrDirectory.Enabled = false;
            return;
        }

        TreeNode? selectedTreeNode = this.SelectedTreeView.SelectedNode;

        switch (selectedTreeNode?.Tag) {
            case LoadedInternal:
            case LoadedPort:
                mAddExistingFilesAsDependant.Enabled = mAddExistingFilesAsDependant.Visible = false;
                mScanSelectedFile.Enabled = true;
                mBrowseToFileOrDirectory.Enabled = true;
                mEditSelectedItem.Enabled = true;
                mRemoveSelectedItem.Enabled = true;
                break;

            case LoadedExternal external:
                mAddExistingFilesAsDependant.Enabled = mAddExistingFilesAsDependant.Visible = true;
                mBrowseToFileOrDirectory.Enabled = true;
                mScanSelectedFile.Enabled = external.IsFile && external.FileExists;
                mEditSelectedItem.Enabled = true;
                mRemoveSelectedItem.Enabled = true;
                break;

            default:
                mAddExistingFilesAsDependant.Enabled = mAddExistingFilesAsDependant.Visible = false;
                mScanSelectedFile.Enabled = false;
                mBrowseToFileOrDirectory.Enabled = false;
                mEditSelectedItem.Enabled = false;
                mRemoveSelectedItem.Enabled = false;
                break;
        }
    }

    #region PWAD / directory / groups
    private bool IgnoreExternalFileEvents { get; set; }
    private void AddExternalFiles(LoadedExternal? parent, bool multiSelect) {
        using var ofd = FindResourceFile.GetExternalFileDialog(multiSelect: multiSelect);
        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FindResourceFile.UpdateLastDirectory(ofd);
        bool pathsAdded = false;
        bool refreshMaps = false;
        bool checkFirstPath = true;

        void AddPath(string path) {
            if (path.IsNullEmptyWhitespace()) {
                return;
            }

            var external = this.AddExternal(path: path, parent: parent, externalType: AddExternalType.ExternalFile);

            if (external is not null) {
                pathsAdded = true;

                if (external.Enabled && (external.IsFile || external.IsGroup) && external.UpdateAllMaps() > 0) {
                    refreshMaps = true;
                }
            }

            if (checkFirstPath && ofd.FileName.Equals(path,
                StringComparison.InvariantCultureIgnoreCase)) {
                checkFirstPath = false;
            }
        }

        ofd.FileNames?.For(AddPath);

        if (checkFirstPath) {
            checkFirstPath = false;
            AddPath(ofd.FileName);
        }

        if (!pathsAdded) {
            return;
        }

        this.OnConfigSaveRequired();
        this.RefreshExternalFiles();

        if (refreshMaps) {
            this.RefreshMaps();
        }
    }
    private void AddExternalDirectoriesOrGroups(LoadedExternal? parent, bool multiSelect) {
        using var fbd = FindResourceFile.GetExternalDirectoryDialog(externalType: AddExternalType.ExternalGroupOrDirectory, multiSelect: multiSelect);
        if (!fbd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FindResourceFile.UpdateLastDirectory(fbd);
        bool pathsAdded = false;
        bool refreshMaps = false;
        bool checkFirstPath = !fbd.SelectedPath.IsNullEmptyWhitespace();

        void AddPath(string path) {
            if (path.IsNullEmptyWhitespace()) {
                return;
            }

            var external = this.AddExternal(path: path, parent: parent, externalType: AddExternalType.ExternalGroupOrDirectory);

            if (external is not null) {
                pathsAdded = true;

                if (external.Enabled && (external.IsFile || external.IsGroup) && external.UpdateAllMaps() > 0) {
                    refreshMaps = true;
                }
            }

            if (checkFirstPath && fbd.SelectedPath.Equals(path,
                StringComparison.InvariantCultureIgnoreCase)) {
                checkFirstPath = false;
            }
        }

        //fbd.SelectedPaths.For(AddPath);

        if (checkFirstPath) {
            checkFirstPath = false;
            AddPath(fbd.SelectedPath);
        }

        if (!pathsAdded) {
            return;
        }

        this.OnConfigSaveRequired();
        this.RefreshExternalFiles();

        if (refreshMaps) {
            this.RefreshMaps();
        }
    }
    private LoadedExternal? AddExternal(string path, LoadedExternal? parent, AddExternalType externalType) {
        frmAddItem ContinueAddingExternal(string name, string path, bool scanExistingExternal = false) {
            bool pathRelative = path.IsPathRelativeCompatible();
            using frmAddItem editor = new(name: name,
                path: path,
                parent: parent,
                externalType: externalType,
                isPathRelative: pathRelative,
                allowRelative: pathRelative) {
                    IsEditing = false,
                };

            if (scanExistingExternal) {
                RequireNewName<LoadedExternal>(tvExternalFiles, null, "An existing external file or group with that name is present, try another name.", editor);
                return editor;
            }

            editor.ShowDialog();
            return editor;
        }

        LoadedExternal external;
        FileInfo? file = null;
        DirectoryInfo? directory = null;

        switch (externalType) {
            case AddExternalType.ExternalFile: {
                file ??= new(path);

                InternalWad? intwad = file.Exists && file.Length > 5 ? InternalWad.FindAnyInternalWad(file) : null;
                string name = intwad?.WadType is not null and not WadType.Unknown ?
                    intwad.GetFullString() : Path.GetFileName(path);

                using var afd = ContinueAddingExternal(name: name,
                    path: path);
                if (afd?.DialogResult != DialogResult.OK) {
                    return null;
                }

                external = new(name: afd.NewName, file: file) {
                    Enabled = true,
                    IsPathRelative = afd.IsPathRelative,
                };
            } break;

            case AddExternalType.ExternalGroupOrDirectory: {
                externalType = AddExternalType.ExternalGroupOrDirectory;

                using var afd = ContinueAddingExternal(name: Path.GetFileName(path), path: path);
                if (afd?.DialogResult != DialogResult.OK) {
                    return null;
                }

                external = new(name: afd.NewName, directory: directory ?? new DirectoryInfo(afd.NewPath), asGroup: afd.SelectedExternalType == ExternalType.Group, relativePath: afd.IsPathRelative) {
                    Enabled = true,
                };
            } break;

            default:
                if ((file = new FileInfo(path)).Exists) {
                    externalType = AddExternalType.ExternalFile;
                    goto case AddExternalType.ExternalFile;
                }

                if ((directory = new DirectoryInfo(path)).Exists) {
                    externalType = AddExternalType.ExternalGroupOrDirectory;
                    goto case AddExternalType.ExternalGroupOrDirectory;
                }

                return null;
        }

        TreeNode node = new(external.Name) {
            Tag = external,
            ToolTipText = external.ToolTipText,
            Checked = external.Enabled,
        };
        external.TreeNode = node;

        external.Dependants.For(dependant => {
            TreeNode dependantNode = new(dependant.Name) {
                Tag = dependant,
                ToolTipText = dependant.ToolTipText,
                Checked = dependant.Enabled,
            };
            dependant.TreeNode = dependantNode;
            node.Nodes.Add(dependantNode);
        });

        if (parent is not null) {
            parent.AddDependant(external);

            if (parent.TreeNode is TreeNode parentNode) {
                parentNode.Nodes.Add(node);
            }
        }
        else {
            tvExternalFiles.Nodes.Add(node);
        }

        return external;
    }
    private static void CheckNodeDependants(TreeNode node) {
        if (node.TreeView is not ExplorerTreeView etv || node.Tag is not LoadedExternal external || external.Dependants.Count < 1) {
            return;
        }

        static bool AnyEnabled(LoadedExternal external) {
            if (external.Enabled) {
                return true;
            }

            for (int i = 0; i < external.Dependants.Count; i++) {
                if (AnyEnabled(external.Dependants[i])) {
                    return true;
                }
            }

            return false;
        }

        if (!AnyEnabled(external)) {
            etv.SetCheckState(node, CheckState.Indeterminate);
            return;
        }

        etv.SetCheckState(node, external.Enabled ? CheckState.Checked : CheckState.Unchecked);
    }
    private void EditExternalFile(LoadedExternal external) {
        AddExternalType externalType = external.ExternalType switch {
            ExternalType.File => AddExternalType.ExternalFile,
            ExternalType.Group => AddExternalType.ExternalGroup,
            ExternalType.Directory => AddExternalType.ExternalDirectory,
            _ => AddExternalType.Unknown,
        };

        if (externalType is not AddExternalType.ExternalFile and not AddExternalType.ExternalGroup and not AddExternalType.ExternalDirectory) {
            return;
        }

        using frmAddItem editor = new(name: external.Name,
            path: external.Path ?? string.Empty,
            externalType: externalType,
            isPathRelative: external.IsPathRelative,
            allowRelative: external.Path.IsPathRelativeCompatible()) {
                IsEditing = true,
            };

        if (!editor.ShowDialog(DialogResult.OK)) {
            return;
        }

        if (!external.HasNameAndPathChanged(name: editor.NewName, path: editor.NewPath, isPathRelative: editor.IsPathRelative)) {
            return;
        }

        external.Enabled = external.Enabled;
        if (external.TreeNode is TreeNode node) {
            node.Text = external.Name;
            node.ToolTipText = external.ToolTipText;

            if (node.TreeView is not null) {
                bool selected = node.IsSelected;
                node.TreeView.SuspendLayout();

                this.IgnoreExternalFileEvents = true;
                node.Checked = !external.Enabled;
                if (selected) {
                    node.TreeView.SelectedNode = null;
                }

                this.IgnoreExternalFileEvents = false;
                node.Checked = external.Enabled;
                if (selected) {
                    node.TreeView.SelectedNode = node;
                }

                node.TreeView.ResumeLayout(performLayout: false);
            }
        }

        this.OnConfigSaveRequired();
    }
    private void RemoveExternalFile(LoadedExternal external) {
        bool refreshMaps = false;

        void RemoveFile(LoadedExternal external2) {
            if (external2.TreeNode is TreeNode node) {
                node.TreeView?.Nodes.Remove(node);
            }

            external2.Dependants?.For(RemoveFile);
            external2.RemoveDependant();

            if (external2.Maps?.Count > 0) {
                refreshMaps = true;
            }
        }

        RemoveFile(external);

        this.OnConfigSaveRequired();
        this.RefreshExternalFiles();

        if (refreshMaps) {
            this.RefreshMaps();
        }
    }
    private void ToggleExternalFile(LoadedExternal external) {
        if (external is null) {
            return;
        }

        external.Enabled ^= true;

        if (external.TreeNode is not TreeNode node) {
            return;
        }

        this.IgnoreExternalFileEvents = true;
        node.Checked = !external.Enabled;
        this.IgnoreExternalFileEvents = false;
        node.Checked = external.Enabled;
    }
    private void tvExternalFiles_AfterCheck(object? sender, TreeViewEventArgs e) {
        if (this.IgnoreExternalFileEvents || e.Node?.Tag is not LoadedExternal external) {
            return;
        }

        bool reloadMaps = false;
        external.Enabled = e.Node.Checked;

        if (e.Node.Checked) {
            reloadMaps = external.UpdateAllMaps() > 0;
        }

        if (reloadMaps) {
            this.RefreshMaps();
        }

        //CheckNodeDependants(e.Node);
        this.RefreshExternalFiles();
        this.OnConfigSaveRequired();
    }
    private void tvExternalFiles_AfterSelect(object? sender, TreeViewEventArgs e) {
        if (this.IgnoreExternalFileEvents) {
            return;
        }

        if (tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external) {
            return;
        }

        chkAddAsDependant.Enabled = e.Node is not null;

        if (!external.IsFile) {
            return;
        }

        external.LoadWadInfo();

        if (external.TreeNode is TreeNode node) {
            node.ToolTipText = external.ToolTipText;
        }
    }
    private void tvExternalFiles_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e) {
        if (this.IgnoreExternalFileEvents || tvExternalFiles.SelectedNode is null) {
            return;
        }
        tvExternalFiles.SelectedNode.Checked ^= true;
        // The checks and everything is done on the 'AfterCheck' event.
    }
    private void btnExternalFileAddDirectory_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents) {
            return;
        }

        this.AddExternalDirectoriesOrGroups(parent: this.GetParentExternalIfEnabled, multiSelect: true);
    }
    private void btnExternalFileAddFile_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents) {
            return;
        }

        this.AddExternalFiles(parent: this.GetParentExternalIfEnabled, multiSelect: true);
    }
    private void btnExternalFileRemoveSelected_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents || tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external) {
            return;
        }

        this.RemoveExternalFile(external: external);
    }
    private void btnExternalFileEdit_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents || tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external) {
            return;
        }

        this.EditExternalFile(external: external);
    }
    private void btnExternalFileToggle_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents || tvExternalFiles.SelectedNode?.Tag is not LoadedExternal external) {
            return;
        }

        this.ToggleExternalFile(external: external);
    }
    private void btnExternalFileMoveUp_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents) {
            return;
        }

        var tv = tvExternalFiles;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedExternal external) {
            return;
        }

        this.IgnoreSourcePortEvents = true;
        if (tv.MoveNodeUp()) {
            external.MoveDependantUp();
        }
        this.IgnoreSourcePortEvents = false;
    }
    private void btnExternalFileMoveDown_Click(object? sender, EventArgs e) {
        if (this.IgnoreExternalFileEvents) {
            return;
        }

        var tv = tvExternalFiles;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedExternal external) {
            return;
        }

        this.IgnoreSourcePortEvents = true;
        if (tv.MoveNodeDown()) {
            external.MoveDependantDown();
            this.OnConfigSaveRequired();
        }
        this.IgnoreExternalFileEvents = false;
    }
    #endregion PWAD / directory / groups

    #region Source Ports
    private bool IgnoreSourcePortEvents { get; set; }
    private int LastSourcePortIndex { get; set; } = -1;
    private TreeNode? LastSourcePortNode { get; set; }
    private void AddSourcePort(bool selectAfterAdd, bool multiSelect) {
        using var ofd = FindResourceFile.GetPortDialog(multiSelect: multiSelect);
        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FindResourceFile.UpdateLastDirectory(ofd);

        bool pathsAdded = false;
        bool checkFirstPath = true;
        LoadedPort? LastPort = null;

        void AddFile(string path) {
            if (ofd.FileName.IsNullEmptyWhitespace()) {
                return;
            }

            LoadedPort? port = this.AddSourcePort(path, false);
            LastPort = port ?? LastPort;

            if (port is null) {
                return;
            }

            pathsAdded = true;

            if (checkFirstPath && ofd.FileName.Equals(path,
                StringComparison.InvariantCultureIgnoreCase)) {
                checkFirstPath = false;
            }
        }

        ofd.FileNames?.For(AddFile);

        if (checkFirstPath) {
            checkFirstPath = false;
            AddFile(ofd.FileName);
        }

        if (!pathsAdded) {
            return;
        }

        if (selectAfterAdd && LastPort is not null) {
            cbSourcePorts.SelectedItem = LastPort;
            return;
        }

        this.OnConfigSaveRequired();
    }
    private LoadedPort? AddSourcePort(string filePath, bool selectAfterAdd) {
        bool pathRelative = filePath.IsPathRelativeCompatible();
        using frmAddItem editor = new(name: Path.GetFileName(filePath),
            path: filePath,
            externalType: AddExternalType.SourcePort,
            isPathRelative: pathRelative,
            allowRelative: pathRelative) {
                IsEditing = false,
            };

        if (!RequireNewName<LoadedPort>(tvConfigSourcePorts, null, "An existing port with that name is present, try another name.", editor)) {
            return null;
        }

        FileInfo file = new(editor.NewPath);
        LoadedPort port = new(name: editor.NewName, file: file) {
            IsPathRelative = editor.IsPathRelative,
        };

        TreeNode node = new(port.Name) {
            Tag = port,
            ToolTipText = port.ToolTipText,
        };
        port.TreeNode = node;
        tvConfigSourcePorts.Nodes.Add(node);

        int index = cbSourcePorts.Items.Add(port);
        if (selectAfterAdd) {
            cbSourcePorts.SelectedIndex = index;
        }

        return port;
    }
    private void EditSourcePort(LoadedPort port) {
        using frmAddItem editor = new(name: port.Name,
            path: port.Path,
            externalType: AddExternalType.SourcePort,
            isPathRelative: port.IsPathRelative,
            allowRelative: port.Path.IsPathRelativeCompatible()) {
                IsEditing = true,
            };

        if (!RequireNewName(tvConfigSourcePorts, port, "An existing port with that name is present, try another name.", editor)
        || !port.UpdateNameAndPath(name: editor.NewName, path: editor.NewPath, isPathRelative: editor.IsPathRelative)) {
            return;
        }

        if (port.TreeNode is TreeNode node) {
            node.Text = port.Name;
            node.ToolTipText = port.ToolTipText;

            if (node.TreeView is not null && node.IsSelected) {
                node.TreeView.SuspendLayout();
                this.IgnoreSourcePortEvents = true;
                node.TreeView.SelectedNode = null;
                this.IgnoreSourcePortEvents = false;
                node.TreeView.SelectedNode = node;
                node.TreeView.ResumeLayout(performLayout: false);
            }

            if (cbSourcePorts.SelectedItem == port) {
                btnPlay.Enabled = port.FileExists;
            }
        }

        this.OnConfigSaveRequired();
    }
    private void RemoveSourcePort(LoadedPort port) {
        if (port.TreeNode is TreeNode node) {
            if (tvConfigSourcePorts.Nodes.Count <= 2) {
                this.DisableSourcePort();
            }
            (node.Parent?.Nodes ?? node.TreeView?.Nodes)?.Remove(node);
        }

        cbSourcePorts.Items.Remove(port);

        this.OnConfigSaveRequired();
    }
    private void DisableSourcePort() {
        cbSourcePorts.SelectedIndex = -1;
        ttHints.SetToolTip(cbSourcePorts, null);
    }
    private void cbSourcePorts_SelectedIndexChanged(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        if (cbSourcePorts.SelectedIndex == 0) {
            this.IgnoreSourcePortEvents = true;
            cbSourcePorts.SelectedIndex = this.LastSourcePortIndex > 0 ? this.LastSourcePortIndex : -1;
            this.IgnoreSourcePortEvents = false;
            this.AddSourcePort(selectAfterAdd: true, multiSelect: false);
            return;
        }

        if (cbSourcePorts.SelectedIndex < 0) {
            btnPlay.Enabled = false;
            return;
        }

        if (this.LastSourcePortIndex == cbSourcePorts.SelectedIndex) {
            return;
        }

        if (cbSourcePorts.SelectedItem is LoadedPort port) {
            port.LoadPortInfo();
            ttHints.SetToolTip(cbSourcePorts, port.ToolTipText);
            if (port.TreeNode is TreeNode node) {
                node.ToolTipText = port.ToolTipText;
            }
            btnPlay.Enabled = port.FileExists;
            this.OnConfigSaveRequired();
        }
        else {
            cbSourcePorts.SelectedIndex = this.LastSourcePortIndex > 0 ? this.LastSourcePortIndex : -1;
        }

        this.LastSourcePortIndex = cbSourcePorts.SelectedIndex;
    }
    private void tvConfigSourcePorts_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        if (e.Node == this.PortConfigBrowseNode) {
            this.IgnoreSourcePortEvents = true;
            e.Cancel = true;
            this.AddSourcePort(selectAfterAdd: true, multiSelect: false);
            this.IgnoreSourcePortEvents = false;
        }
    }
    private void tvConfigSourcePorts_AfterSelect(object sender, TreeViewEventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        if (tvConfigSourcePorts.SelectedNode?.Tag is not LoadedPort port) {
            if (tvConfigSourcePorts.SelectedNode == this.PortConfigBrowseNode) {
                this.LastSourcePortNode = null;
            }
            tvConfigSourcePorts.SelectedNode = this.LastSourcePortNode;
            return;
        }

        port.LoadPortInfo();

        if (port.TreeNode is TreeNode node) {
            node.ToolTipText = port.ToolTipText;
        }

        if (cbSourcePorts.SelectedItem == port) {
            ttHints.SetToolTip(cbSourcePorts, port.ToolTipText);
        }

        this.LastSourcePortNode = port.TreeNode as TreeNode;
    }
    private void btnConfigAddSourcePort_Click(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        this.AddSourcePort(selectAfterAdd: false, multiSelect: true);
    }
    private void btnConfigRemoveSourcePort_Click(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents || tvConfigSourcePorts.SelectedNode?.Tag is not LoadedPort port) {
            return;
        }

        this.RemoveSourcePort(port: port);
    }
    private void btnConfigEditSourcePort_Click(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents || tvConfigSourcePorts.SelectedNode?.Tag is not LoadedPort port) {
            return;
        }
        this.EditSourcePort(port: port);
    }
    private void btnConfigMoveSourcePortUp_Click(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        var tv = tvConfigSourcePorts;
        var tv2 = cbSourcePorts;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedPort port) {
            return;
        }

        this.IgnoreSourcePortEvents = true;
        if (tv.MoveNodeUp()) {
            var lastItem = tv2.SelectedItem;
            tv2.Items.Remove(port);
            tv2.Items.Insert(node.Index, port);
            tv2.SelectedItem = lastItem;
            this.OnConfigSaveRequired();
        }
        this.IgnoreSourcePortEvents = false;
    }
    private void btnConfigMoveSourcePortDown_Click(object? sender, EventArgs e) {
        if (this.IgnoreSourcePortEvents) {
            return;
        }

        var tv = tvConfigSourcePorts;
        var tv2 = cbSourcePorts;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedPort port) {
            return;
        }

        bool insert = node.Index != tv.Nodes.Count - 1;
        this.IgnoreSourcePortEvents = true;
        if (tv.MoveNodeDown()) {
            var lastItem = tv2.SelectedItem;
            tv2.Items.Remove(port);
            if (insert) {
                tv2.Items.Insert(node.Index, port);
            }
            else {
                tv2.Items.Add(port);
            }
            tv2.SelectedItem = lastItem;
            this.OnConfigSaveRequired();
        }
        this.IgnoreSourcePortEvents = false;
    }
    #endregion Source ports

    #region IWAD
    private bool IgnoreIWADEvents { get; set; }
    private TreeNode? LastIWADNode { get; set; }
    private TreeNode? LastIWADConfigNode { get; set; }
    private void AddIWAD(bool selectAfterAdd, bool multiSelect) {
        using var ofd = FindResourceFile.GetIWadDialog(multiSelect: multiSelect);
        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FindResourceFile.UpdateLastDirectory(ofd);
        bool pathsAdded = false;
        bool checkFirstPath = true;
        LoadedInternal? LastIWAD = null;

        void AddFile(string path) {
            if (ofd.FileName.IsNullEmptyWhitespace()) {
                return;
            }

            LoadedInternal? iwad = this.AddIWad(path, false);
            LastIWAD = iwad ?? LastIWAD;

            if (iwad is null) {
                return;
            }

            pathsAdded = true;

            if (checkFirstPath && ofd.FileName.Equals(path,
                StringComparison.InvariantCultureIgnoreCase)) {
                checkFirstPath = false;
            }
        }

        ofd.FileNames?.For(AddFile);

        if (checkFirstPath) {
            checkFirstPath = false;
            AddFile(ofd.FileName);
        }

        if (!pathsAdded) {
            return;
        }

        if (selectAfterAdd && LastIWAD?.TreeNode is TreeNode node) {
            tvIWADs.SelectedNode = node;
            return;
        }

        this.OnConfigSaveRequired();
    }
    private LoadedInternal? AddIWad(string filePath, bool selectAfterAdd) {
        bool pathRelative = filePath.IsPathRelativeCompatible();

        FileInfo file = new(filePath);
        InternalWad? intwad = InternalWad.FindAnyInternalWad(file);
        string name = intwad is not null && intwad.WadType != WadType.Unknown ?
            intwad.GetFullString() : Path.GetFileName(filePath);

        using frmAddItem editor = new(name: name,
            path: filePath,
            externalType: AddExternalType.IWad,
            isPathRelative: pathRelative,
            allowRelative: pathRelative) {
                IsEditing = false,
            };

        if (!RequireNewName<LoadedInternal>(tvConfigIWADs, null, "An existing IWAD with that name is present, try another name.", editor)) {
            return null;
        }

        file = new(editor.NewPath);
        LoadedInternal iwad = new(name: editor.NewName, file: file) {
            IsPathRelative = editor.IsPathRelative,
        };

        TreeNode node = new(iwad.Name) {
            Tag = iwad,
            ToolTipText = iwad.ToolTipText,
        };
        iwad.TreeNode = node;
        tvIWADs.Nodes.Add(node);

        TreeNode nodeConfig = new(iwad.Name) {
            Tag = iwad,
            ToolTipText = iwad.ToolTipText,
        };
        iwad.TreeNodeConfig = nodeConfig;
        tvConfigIWADs.Nodes.Add(nodeConfig);

        if (selectAfterAdd) {
            tvIWADs.SelectedNode = node;
        }

        return iwad;
    }
    private void EditIWad(LoadedInternal iwad) {
        using frmAddItem editor = new(name: iwad.Name,
            path: iwad.Path,
            externalType: AddExternalType.IWad,
            isPathRelative: iwad.IsPathRelative,
            allowRelative: iwad.Path.IsPathRelativeCompatible()) {
                IsEditing = true,
            };

        if (!RequireNewName(tvConfigIWADs, iwad, "An existing IWAD with that name is present, try another name.", editor)
        || !iwad.UpdateNameAndPath(name: editor.NewName, path: editor.NewPath, isPathRelative: editor.IsPathRelative)) {
            return;
        }

        if (iwad.TreeNode is TreeNode node) {
            node.Text = iwad.Name;
            node.ToolTipText = iwad.ToolTipText;

            if (node.TreeView is not null && node.IsSelected) {
                node.TreeView.SuspendLayout();
                this.IgnoreIWADEvents = true;
                node.TreeView.SelectedNode = null;
                this.IgnoreIWADEvents = false;
                node.TreeView.SelectedNode = node;
                node.TreeView.ResumeLayout(performLayout: false);
            }
        }

        if (iwad.TreeNodeConfig is TreeNode nodeConfig) {
            nodeConfig.Text = iwad.Name;
            nodeConfig.ToolTipText = iwad.ToolTipText;

            if (nodeConfig.TreeView is not null && nodeConfig.IsSelected) {
                nodeConfig.TreeView.SuspendLayout();
                this.IgnoreIWADEvents = true;
                nodeConfig.TreeView.SelectedNode = null;
                this.IgnoreIWADEvents = false;
                nodeConfig.TreeView.SelectedNode = nodeConfig;
                nodeConfig.TreeView.ResumeLayout(performLayout: false);
            }
        }

        this.OnConfigSaveRequired();
    }
    private void RemoveIWad(LoadedInternal iwad) {
        if (iwad.TreeNode is TreeNode node) {
            if (tvIWADs.Nodes.Count <= 2) {
                tvIWADs.SelectedNode = null;
                this.DisableIWAD();
                this.RefreshMaps();
            }
            node.TreeView?.Nodes.Remove(node);
        }

        if (iwad.TreeNodeConfig is TreeNode nodeConfig) {
            if (tvConfigIWADs.Nodes.Count <= 2) {
                tvConfigIWADs.SelectedNode = null;
            }
            nodeConfig.TreeView?.Nodes.Remove(nodeConfig);
        }

        this.OnConfigSaveRequired();
    }
    private void DisableIWAD() {
        tvIWADs.SelectedNode = null;
        lbSelectedIWAD.Text = "none";
        ttHints.SetToolTip(lbSelectedIWAD, "Loaded IWAD information will appear here when you select a known WAD.");
    }
    private void tvIWADs_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        if (e.Node == this.InternalWadBrowseNode) {
            this.IgnoreIWADEvents = true;
            this.AddIWAD(selectAfterAdd: true, multiSelect: false);
            this.IgnoreIWADEvents = false;
            e.Cancel = true;
        }
    }
    private void tvIWADs_AfterSelect(object? sender, TreeViewEventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        if (tvIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
            if (tvIWADs.SelectedNode == this.InternalWadBrowseNode) {
                this.LastIWADNode = null;
            }
            tvIWADs.SelectedNode = this.LastIWADNode;
            return;
        }

        iwad.LoadWadInfo();

        if (iwad.TreeNode is TreeNode nodem) {
            nodem.ToolTipText = iwad.ToolTipText;
        }

        if (iwad.TreeNodeConfig is TreeNode nodec) {
            nodec.ToolTipText = iwad.ToolTipText;
        }

        ttHints.SetToolTip(lbSelectedIWAD, iwad.ToolTipText);

        if (iwad.UpdateMaps() > 0) {
            this.RefreshMaps();
        }

        lbSelectedIWAD.Text = iwad.Name;
        this.LastIWADNode = tvIWADs.SelectedNode;
        this.OnConfigSaveRequired();
    }
    private void tvIWADs_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }
        this.DisableIWAD();
        this.RefreshMaps();
        this.OnConfigSaveRequired();
    }
    private void tvConfigIWADs_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        if (e.Node == this.InternalWadConfigBrowseNode) {
            this.IgnoreIWADEvents = true;
            this.AddIWAD(selectAfterAdd: false, multiSelect: false);
            this.IgnoreIWADEvents = false;
            e.Cancel = true;
        }
    }
    private void tvConfigIWADs_AfterSelect(object? sender, TreeViewEventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        if (tvConfigIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
            if (tvConfigIWADs.SelectedNode == this.InternalWadConfigBrowseNode) {
                this.LastIWADConfigNode = null;
            }
            tvConfigIWADs.SelectedNode = this.LastIWADConfigNode;
            return;
        }

        iwad.LoadWadInfo();

        if (iwad.TreeNode is TreeNode nodem) {
            nodem.ToolTipText = iwad.ToolTipText;
        }

        if (iwad.TreeNodeConfig is TreeNode nodec) {
            nodec.ToolTipText = iwad.ToolTipText;
        }

        this.LastIWADConfigNode = tvConfigIWADs.SelectedNode;
    }
    private void btnConfigAddIWAD_Click(object? sender, EventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        this.AddIWAD(selectAfterAdd: false, multiSelect: true);
    }
    private void btnConfigRemoveIWAD_Click(object? sender, EventArgs e) {
        if (this.IgnoreIWADEvents || tvConfigIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
            return;
        }

        this.RemoveIWad(iwad: iwad);
    }
    private void btnConfigEditIWAD_Click(object? sender, EventArgs e) {
        if (this.IgnoreIWADEvents || tvConfigIWADs.SelectedNode?.Tag is not LoadedInternal iwad) {
            return;
        }

        this.EditIWad(iwad: iwad);
    }
    private void btnConfigMoveIWADUp_Click(object? sender, EventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        var tv = tvConfigIWADs;
        var tv2 = tvIWADs;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedInternal) {
            return;
        }

        this.IgnoreIWADEvents = true;
        if (tv.MoveNodeUp()) {
            var lastItem = tv2.SelectedNode;
            tv2.Nodes.Remove(lastItem);
            tv2.Nodes.Insert(node.Index, lastItem);
            tv2.SelectedNode = lastItem;
            this.OnConfigSaveRequired();
        }
        this.IgnoreIWADEvents = false;
    }
    private void btnConfigMoveIWADDown_Click(object? sender, EventArgs e) {
        if (this.IgnoreIWADEvents) {
            return;
        }

        var tv = tvConfigIWADs;
        var tv2 = tvIWADs;
        var node = tv.SelectedNode;

        if (node?.Tag is not LoadedInternal) {
            return;
        }

        bool insert = node.Index != tv.Nodes.Count - 1;
        this.IgnoreIWADEvents = true;
        if (tv.MoveNodeDown()) {
            var lastItem = tv2.SelectedNode;
            tv2.Nodes.Remove(lastItem);
            if (insert) {
                tv2.Nodes.Insert(node.Index, lastItem);
            }
            else {
                tv2.Nodes.Add(lastItem);
            }
            tv2.SelectedNode = lastItem;
            this.OnConfigSaveRequired();
        }
        this.IgnoreIWADEvents = false;
    }
    #endregion IWAD

    #region Multiplayer
    private bool IgnoreMultiplayerEvents { get; set; }
    private int LastMultiplayerSaveGameIndex { get; set; } = -1;
    private void btnMultiplayer_Click(object? sender, EventArgs e) {
        pnMultiplayer.Visible ^= true;
        this.ResizeControls();
        //this.OnConfigSaveRequired();
    }
    private void cbMultiplayerGameMode_SelectedIndexChanged(object? sender, EventArgs e) {
        if (this.IgnoreMultiplayerEvents) {
            return;
        }

        btnPlay.Text = cbMultiplayerGameMode.SelectedIndex > 0 ?
            (cbMultiplayerPlayers.SelectedIndex == 0 ? "Connect server" : "Host server") : "Play singleplayer";

        this.OnConfigSaveRequired();
    }
    private void cbMultiplayerPlayers_KeyDown(object? sender, KeyEventArgs e) {
        if (this.IgnoreMultiplayerEvents) {
            return;
        }

        this.IgnoreMultiplayerEvents = true;
        if (cbMultiplayerPlayers.Text.Length >= 9) {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        this.txtNumericOnly_KeyDown(sender, e);
        this.IgnoreMultiplayerEvents = false;
    }
    private void cbMultiplayerPlayers_SelectedIndexChanged(object? sender, EventArgs e) {
        if (this.IgnoreMultiplayerEvents) {
            return;
        }

        if (cbMultiplayerGameMode.SelectedIndex > -1) {
            this.IgnoreMultiplayerEvents = true;

            if (cbMultiplayerPlayers.SelectedIndex >= cbMultiplayerPlayers.Items.Count - 1) {
                cbMultiplayerPlayers.DropDownStyle = ComboBoxStyle.DropDown;
                cbMultiplayerPlayers.Text = string.Empty;
            }
            else {
                btnPlay.Text = cbMultiplayerGameMode.SelectedIndex > 0 ? (cbMultiplayerPlayers.SelectedIndex != 0 ? "Host" : "Connect") : "Play singleplayer";
                cbMultiplayerPlayers.DropDownStyle = ComboBoxStyle.DropDownList;
                this.OnConfigSaveRequired();
            }

            this.IgnoreMultiplayerEvents = false;
        }
    }
    private void cbMultiplayerNetMode_SelectedIndexChanged(object sender, EventArgs e) {
        txtMultiplayerHostnameIp.CueText = cbMultiplayerNetMode.SelectedIndex switch {
            1 => "127.0.0.1",
            2 => "hostname.com",
            _ => "hostname.com / 127.0.0.1",
        };
    }
    private void cbMultiplayerSaveGame_SelectedIndexChanged(object? sender, EventArgs e) {
        if (this.IgnoreMultiplayerEvents || cbMultiplayerSaveGame.SelectedIndex < 0) {
            return;
        }

        if (cbMultiplayerSaveGame.SelectedIndex == 0) {
            cbMultiplayerSaveGame.SelectedIndex = -1;
            return;
        }

        if (cbMultiplayerSaveGame.SelectedIndex == 1) {
            this.IgnoreMultiplayerEvents = true;
            cbMultiplayerSaveGame.SelectedIndex = this.LastMultiplayerSaveGameIndex;
            this.IgnoreMultiplayerEvents = false;

            using var ofd = FindResourceFile.GetSaveDialog(multiSelect: true);

            if (!ofd.ShowDialog(DialogResult.OK) || ofd.FileNames.Length < 1) {
                return;
            }

            FindResourceFile.UpdateLastDirectory(ofd);
            ofd.FileNames.For(path => cbMultiplayerSaveGame.Items.Add(path));

            if (ofd.FileNames.Length == 1) {
                cbMultiplayerSaveGame.SelectedIndex = cbMultiplayerSaveGame.Items.Count - 1;
            }
        }

        this.LastMultiplayerSaveGameIndex = cbMultiplayerSaveGame.SelectedIndex;
        this.OnConfigSaveRequired();
    }
    #endregion Multiplayer

    private void btnPlay_Click(object? sender, EventArgs e) {
        if (cbSourcePorts.SelectedItem is not LoadedPort port) {
            return;
        }

        var args = this.GenerateArguments(port, tvIWADs.SelectedNode?.Tag is LoadedInternal liwad && liwad.FileExists ? liwad : null);

        if (args is null) {
            return;
        }

        this.Launch(args);
        this.SaveConfig(forceSave: false);
    }
}
