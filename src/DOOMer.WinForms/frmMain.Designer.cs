namespace DOOMer.WinForms;

using System.Drawing;
using System.Windows.Forms;
using DOOMer.Controls;

partial class frmMain {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
        this.pnFiles = new Panel();
        this.scFiles = new SplitContainer();
        this.pnExternalFiles = new Panel();
        this.btnExternalFileMoveUp = new Button();
        this.chkAddAsDependant = new CheckBox();
        this.btnExternalFileMoveDown = new Button();
        this.lbExternalFiles = new Label();
        this.btnExternalFileEdit = new Button();
        this.tvExternalFiles = new ExplorerTreeView();
        this.btnExternalFileAddDirectory = new Button();
        this.btnExternalFileAddFile = new Button();
        this.btnExternalFileToggle = new Button();
        this.btnExternalFileRemoveSelected = new Button();
        this.pnInternalWADs = new Panel();
        this.lbSkillLevel = new Label();
        this.cbSkillLevel = new ComboBox();
        this.tvIWADs = new ExplorerTreeView();
        this.cbMaps = new ComboBoxEx();
        this.lbMaps = new Label();
        this.cbSourcePorts = new ComboBoxEx();
        this.lbSelectedIWAD = new Label();
        this.lbIWADs = new Label();
        this.lbSourcePort = new Label();
        this.pnOptions = new Panel();
        this.btnMore = new Button();
        this.btnImportZdl = new Button();
        this.cbCommandLineArguments = new ComboBoxEx();
        this.btnMultiplayer = new ButtonEx();
        this.btnPlay = new ButtonEx();
        this.lbExtraCommandLineArguments = new Label();
        this.pnMultiplayer = new Panel();
        this.cbMultiplayerPlayers = new ComboBox();
        this.btnMultiplayerConfig = new Button();
        this.cbMultiplayerSaveGame = new ComboBoxEx();
        this.lbMultiplayerSaveGame = new Label();
        this.lbMultiplayerDMFLAGS2 = new Label();
        this.txtMultiplayerDMFLAGS2 = new TextBoxEx();
        this.lbMultiplayerDMFLAGS = new Label();
        this.txtMultiplayerDMFLAGS = new TextBoxEx();
        this.lbMultiplayerTimeLimit = new Label();
        this.txtMultiplayerTimeLimit = new TextBoxEx();
        this.cbMultiplayerExtratic = new ComboBox();
        this.lbMultiplayerExtratic = new Label();
        this.cbMultiplayerDup = new ComboBox();
        this.lbMultiplayerDup = new Label();
        this.lbMultiplayerFragLimit = new Label();
        this.txtMultiplayerFragLimit = new TextBoxEx();
        this.lbMultiplayerPort = new Label();
        this.txtMultiplayerPort = new TextBoxEx();
        this.lbMultiplayerHostnameIP = new Label();
        this.txtMultiplayerHostnameIp = new TextBoxEx();
        this.cbMultiplayerNetMode = new ComboBox();
        this.lbMultiplayerNetMode = new Label();
        this.lbMultiplayerPlayers = new Label();
        this.cbMultiplayerGameMode = new ComboBox();
        this.lbMultiplayerGameMode = new Label();
        this.tcMain = new TabControl();
        this.tpEnabledFiles = new TabPage();
        this.tpConfig = new TabPage();
        this.pnConfigFiles = new Panel();
        this.scConfig = new SplitContainer();
        this.pnConfigSourcePorts = new Panel();
        this.lbConfigurationSourcePorts = new Label();
        this.btnConfigMoveSourcePortUp = new Button();
        this.tvConfigSourcePorts = new ExplorerTreeView();
        this.btnConfigMoveSourcePortDown = new Button();
        this.btnConfigAddSourcePort = new Button();
        this.btnConfigEditSourcePort = new Button();
        this.btnConfigRemoveSourcePort = new Button();
        this.btnConfigMoveIWADUp = new Button();
        this.btnConfigMoveIWADDown = new Button();
        this.btnConfigEditIWAD = new Button();
        this.btnConfigRemoveIWAD = new Button();
        this.btnConfigAddIWAD = new Button();
        this.lbConfigIWADs = new Label();
        this.tvConfigIWADs = new ExplorerTreeView();
        this.ttHints = new ToolTip(this.components);
        this.lbModified = new Label();
        this.tpAbout = new TabPage();
        this.lbAbout = new Label();
        this.lbTiny7z = new Label();
        this.pnFiles.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.scFiles).BeginInit();
        this.scFiles.Panel1.SuspendLayout();
        this.scFiles.Panel2.SuspendLayout();
        this.scFiles.SuspendLayout();
        this.pnExternalFiles.SuspendLayout();
        this.pnInternalWADs.SuspendLayout();
        this.pnOptions.SuspendLayout();
        this.pnMultiplayer.SuspendLayout();
        this.tcMain.SuspendLayout();
        this.tpEnabledFiles.SuspendLayout();
        this.tpConfig.SuspendLayout();
        this.pnConfigFiles.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.scConfig).BeginInit();
        this.scConfig.Panel1.SuspendLayout();
        this.scConfig.Panel2.SuspendLayout();
        this.scConfig.SuspendLayout();
        this.pnConfigSourcePorts.SuspendLayout();
        this.tpAbout.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnFiles
        // 
        this.pnFiles.BackColor = SystemColors.Window;
        this.pnFiles.Controls.Add(this.scFiles);
        this.pnFiles.Dock = DockStyle.Fill;
        this.pnFiles.Location = new Point(0, 0);
        this.pnFiles.Name = "pnFiles";
        this.pnFiles.Size = new Size(640, 201);
        this.pnFiles.TabIndex = 1;
        // 
        // scFiles
        // 
        this.scFiles.Dock = DockStyle.Fill;
        this.scFiles.Location = new Point(0, 0);
        this.scFiles.Margin = new Padding(0);
        this.scFiles.Name = "scFiles";
        // 
        // scFiles.Panel1
        // 
        this.scFiles.Panel1.Controls.Add(this.pnExternalFiles);
        // 
        // scFiles.Panel2
        // 
        this.scFiles.Panel2.Controls.Add(this.pnInternalWADs);
        this.scFiles.Size = new Size(640, 201);
        this.scFiles.SplitterDistance = 320;
        this.scFiles.TabIndex = 0;
        // 
        // pnExternalFiles
        // 
        this.pnExternalFiles.Controls.Add(this.btnExternalFileMoveUp);
        this.pnExternalFiles.Controls.Add(this.chkAddAsDependant);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileMoveDown);
        this.pnExternalFiles.Controls.Add(this.lbExternalFiles);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileEdit);
        this.pnExternalFiles.Controls.Add(this.tvExternalFiles);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileAddDirectory);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileAddFile);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileToggle);
        this.pnExternalFiles.Controls.Add(this.btnExternalFileRemoveSelected);
        this.pnExternalFiles.Dock = DockStyle.Fill;
        this.pnExternalFiles.Location = new Point(0, 0);
        this.pnExternalFiles.Margin = new Padding(0);
        this.pnExternalFiles.Name = "pnExternalFiles";
        this.pnExternalFiles.Size = new Size(320, 201);
        this.pnExternalFiles.TabIndex = 12;
        // 
        // btnExternalFileMoveUp
        // 
        this.btnExternalFileMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnExternalFileMoveUp.Font = new Font("Wingdings", 9F);
        this.btnExternalFileMoveUp.Location = new Point(271, 176);
        this.btnExternalFileMoveUp.Name = "btnExternalFileMoveUp";
        this.btnExternalFileMoveUp.Size = new Size(23, 23);
        this.btnExternalFileMoveUp.TabIndex = 9;
        this.btnExternalFileMoveUp.Text = "á";
        this.btnExternalFileMoveUp.UseVisualStyleBackColor = true;
        this.btnExternalFileMoveUp.Click += this.btnExternalFileMoveUp_Click;
        // 
        // chkAddAsDependant
        // 
        this.chkAddAsDependant.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.chkAddAsDependant.Appearance = Appearance.Button;
        this.chkAddAsDependant.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        this.chkAddAsDependant.Location = new Point(118, 176);
        this.chkAddAsDependant.Name = "chkAddAsDependant";
        this.chkAddAsDependant.Size = new Size(31, 23);
        this.chkAddAsDependant.TabIndex = 11;
        this.chkAddAsDependant.Text = "++";
        this.chkAddAsDependant.TextAlign = ContentAlignment.MiddleCenter;
        this.ttHints.SetToolTip(this.chkAddAsDependant, "When checked, any files/directories added to the selected node will be added as a dependant.\r\nYou can also add dependants by holding a (\"CONTROL\") key while adding files.");
        this.chkAddAsDependant.UseVisualStyleBackColor = true;
        // 
        // btnExternalFileMoveDown
        // 
        this.btnExternalFileMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnExternalFileMoveDown.Font = new Font("Wingdings", 9F);
        this.btnExternalFileMoveDown.Location = new Point(294, 176);
        this.btnExternalFileMoveDown.Name = "btnExternalFileMoveDown";
        this.btnExternalFileMoveDown.Size = new Size(23, 23);
        this.btnExternalFileMoveDown.TabIndex = 10;
        this.btnExternalFileMoveDown.Text = "â";
        this.btnExternalFileMoveDown.UseVisualStyleBackColor = true;
        this.btnExternalFileMoveDown.Click += this.btnExternalFileMoveDown_Click;
        // 
        // lbExternalFiles
        // 
        this.lbExternalFiles.AutoSize = true;
        this.lbExternalFiles.Location = new Point(3, 0);
        this.lbExternalFiles.Name = "lbExternalFiles";
        this.lbExternalFiles.Size = new Size(234, 15);
        this.lbExternalFiles.TabIndex = 1;
        this.lbExternalFiles.Text = "External files, groups, and directories (0 / 0)";
        // 
        // btnExternalFileEdit
        // 
        this.btnExternalFileEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnExternalFileEdit.Font = new Font("Wingdings", 9F);
        this.btnExternalFileEdit.Location = new Point(72, 176);
        this.btnExternalFileEdit.Name = "btnExternalFileEdit";
        this.btnExternalFileEdit.Size = new Size(23, 23);
        this.btnExternalFileEdit.TabIndex = 6;
        this.btnExternalFileEdit.Text = "!";
        this.ttHints.SetToolTip(this.btnExternalFileEdit, "Edit the selectd port");
        this.btnExternalFileEdit.UseVisualStyleBackColor = true;
        this.btnExternalFileEdit.Click += this.btnExternalFileEdit_Click;
        // 
        // tvExternalFiles
        // 
        this.tvExternalFiles.AllowDrop = true;
        this.tvExternalFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.tvExternalFiles.CheckBoxes = true;
        this.tvExternalFiles.FileDragging = true;
        this.tvExternalFiles.FullRowSelect = true;
        this.tvExternalFiles.HideSelection = false;
        this.tvExternalFiles.HotTracking = true;
        this.tvExternalFiles.ImageIndex = 0;
        this.tvExternalFiles.Indent = 17;
        this.tvExternalFiles.Location = new Point(3, 16);
        this.tvExternalFiles.MouseDragNodes = true;
        this.tvExternalFiles.Name = "tvExternalFiles";
        this.tvExternalFiles.OnlyAllowDraggedNodesInSameCollection = true;
        this.tvExternalFiles.SelectedImageIndex = 0;
        this.tvExternalFiles.ShowLines = false;
        this.tvExternalFiles.ShowNodeToolTips = true;
        this.tvExternalFiles.Size = new Size(314, 158);
        this.tvExternalFiles.TabIndex = 2;
        this.tvExternalFiles.AfterCheck += this.tvExternalFiles_AfterCheck;
        this.tvExternalFiles.AfterSelect += this.tvExternalFiles_AfterSelect;
        this.tvExternalFiles.NodeMouseClick += this.tvRightClick_NodeMouseClick;
        this.tvExternalFiles.NodeMouseDoubleClick += this.tvExternalFiles_NodeMouseDoubleClick;
        // 
        // btnExternalFileAddDirectory
        // 
        this.btnExternalFileAddDirectory.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnExternalFileAddDirectory.Font = new Font("Wingdings", 9F, FontStyle.Regular, GraphicsUnit.Point, 2);
        this.btnExternalFileAddDirectory.Location = new Point(3, 176);
        this.btnExternalFileAddDirectory.Name = "btnExternalFileAddDirectory";
        this.btnExternalFileAddDirectory.Size = new Size(23, 23);
        this.btnExternalFileAddDirectory.TabIndex = 3;
        this.btnExternalFileAddDirectory.Text = "1";
        this.ttHints.SetToolTip(this.btnExternalFileAddDirectory, "Add a directory with patch wads (pwads) and dehacks");
        this.btnExternalFileAddDirectory.UseVisualStyleBackColor = true;
        this.btnExternalFileAddDirectory.Click += this.btnExternalFileAddDirectory_Click;
        // 
        // btnExternalFileAddFile
        // 
        this.btnExternalFileAddFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnExternalFileAddFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        this.btnExternalFileAddFile.Location = new Point(26, 176);
        this.btnExternalFileAddFile.Name = "btnExternalFileAddFile";
        this.btnExternalFileAddFile.Size = new Size(23, 23);
        this.btnExternalFileAddFile.TabIndex = 4;
        this.btnExternalFileAddFile.Text = "+";
        this.ttHints.SetToolTip(this.btnExternalFileAddFile, "Add an external patch wad (pwad) or dehack");
        this.btnExternalFileAddFile.UseVisualStyleBackColor = true;
        this.btnExternalFileAddFile.Click += this.btnExternalFileAddFile_Click;
        // 
        // btnExternalFileToggle
        // 
        this.btnExternalFileToggle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnExternalFileToggle.Font = new Font("Wingdings", 9F, FontStyle.Regular, GraphicsUnit.Point, 2);
        this.btnExternalFileToggle.Location = new Point(95, 176);
        this.btnExternalFileToggle.Name = "btnExternalFileToggle";
        this.btnExternalFileToggle.Size = new Size(23, 23);
        this.btnExternalFileToggle.TabIndex = 7;
        this.btnExternalFileToggle.Text = "O";
        this.ttHints.SetToolTip(this.btnExternalFileToggle, "Toggle the selected external file(s)");
        this.btnExternalFileToggle.UseVisualStyleBackColor = true;
        this.btnExternalFileToggle.Click += this.btnExternalFileToggle_Click;
        // 
        // btnExternalFileRemoveSelected
        // 
        this.btnExternalFileRemoveSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnExternalFileRemoveSelected.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        this.btnExternalFileRemoveSelected.Location = new Point(49, 176);
        this.btnExternalFileRemoveSelected.Name = "btnExternalFileRemoveSelected";
        this.btnExternalFileRemoveSelected.Size = new Size(23, 23);
        this.btnExternalFileRemoveSelected.TabIndex = 5;
        this.btnExternalFileRemoveSelected.Text = "X";
        this.ttHints.SetToolTip(this.btnExternalFileRemoveSelected, "Remove the selected external file(s)");
        this.btnExternalFileRemoveSelected.UseVisualStyleBackColor = true;
        this.btnExternalFileRemoveSelected.Click += this.btnExternalFileRemoveSelected_Click;
        // 
        // pnInternalWADs
        // 
        this.pnInternalWADs.Controls.Add(this.lbSkillLevel);
        this.pnInternalWADs.Controls.Add(this.cbSkillLevel);
        this.pnInternalWADs.Controls.Add(this.tvIWADs);
        this.pnInternalWADs.Controls.Add(this.cbMaps);
        this.pnInternalWADs.Controls.Add(this.lbMaps);
        this.pnInternalWADs.Controls.Add(this.cbSourcePorts);
        this.pnInternalWADs.Controls.Add(this.lbSelectedIWAD);
        this.pnInternalWADs.Controls.Add(this.lbIWADs);
        this.pnInternalWADs.Controls.Add(this.lbSourcePort);
        this.pnInternalWADs.Dock = DockStyle.Fill;
        this.pnInternalWADs.Location = new Point(0, 0);
        this.pnInternalWADs.Margin = new Padding(0);
        this.pnInternalWADs.Name = "pnInternalWADs";
        this.pnInternalWADs.Size = new Size(316, 201);
        this.pnInternalWADs.TabIndex = 10;
        // 
        // lbSkillLevel
        // 
        this.lbSkillLevel.AutoSize = true;
        this.lbSkillLevel.Location = new Point(157, 159);
        this.lbSkillLevel.Name = "lbSkillLevel";
        this.lbSkillLevel.Size = new Size(58, 15);
        this.lbSkillLevel.TabIndex = 7;
        this.lbSkillLevel.Text = "Skill Level";
        // 
        // cbSkillLevel
        // 
        this.cbSkillLevel.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbSkillLevel.FormattingEnabled = true;
        this.cbSkillLevel.Items.AddRange(new object[] { "(Default)", "Very Easy", "Easy", "Normal", "Hard", "Very Hard" });
        this.cbSkillLevel.Location = new Point(157, 173);
        this.cbSkillLevel.Name = "cbSkillLevel";
        this.cbSkillLevel.Size = new Size(96, 23);
        this.cbSkillLevel.TabIndex = 9;
        this.ttHints.SetToolTip(this.cbSkillLevel, "The skill level that will be applied.");
        // 
        // tvIWADs
        // 
        this.tvIWADs.AllowDrop = true;
        this.tvIWADs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.tvIWADs.FileDragging = true;
        this.tvIWADs.FullRowSelect = true;
        this.tvIWADs.HideSelection = false;
        this.tvIWADs.HotTracking = true;
        this.tvIWADs.ImageIndex = 0;
        this.tvIWADs.Location = new Point(3, 51);
        this.tvIWADs.MouseDragNodes = true;
        this.tvIWADs.Name = "tvIWADs";
        this.tvIWADs.OnlyAllowDraggedNodesInSameCollection = true;
        this.tvIWADs.SelectedImageIndex = 0;
        this.tvIWADs.ShowLines = false;
        this.tvIWADs.ShowNodeToolTips = true;
        this.tvIWADs.ShowPlusMinus = false;
        this.tvIWADs.Size = new Size(314, 110);
        this.tvIWADs.TabIndex = 5;
        this.tvIWADs.BeforeSelect += this.tvIWADs_BeforeSelect;
        this.tvIWADs.AfterSelect += this.tvIWADs_AfterSelect;
        this.tvIWADs.NodeMouseClick += this.tvRightClick_NodeMouseClick;
        this.tvIWADs.NodeMouseDoubleClick += this.tvIWADs_NodeMouseDoubleClick;
        // 
        // cbMaps
        // 
        this.cbMaps.CueText = "(None)";
        this.cbMaps.FormattingEnabled = true;
        this.cbMaps.Items.AddRange(new object[] { "(Default)" });
        this.cbMaps.Location = new Point(59, 173);
        this.cbMaps.Name = "cbMaps";
        this.cbMaps.Size = new Size(96, 23);
        this.cbMaps.TabIndex = 8;
        this.ttHints.SetToolTip(this.cbMaps, "The map that will be loaded, if it exists.");
        // 
        // lbMaps
        // 
        this.lbMaps.AutoSize = true;
        this.lbMaps.Location = new Point(59, 159);
        this.lbMaps.Name = "lbMaps";
        this.lbMaps.Size = new Size(31, 15);
        this.lbMaps.TabIndex = 6;
        this.lbMaps.Text = "Map";
        // 
        // cbSourcePorts
        // 
        this.cbSourcePorts.AllowDrop = true;
        this.cbSourcePorts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.cbSourcePorts.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbSourcePorts.FileDragging = true;
        this.cbSourcePorts.FormattingEnabled = true;
        this.cbSourcePorts.Items.AddRange(new object[] { "(Browse)" });
        this.cbSourcePorts.Location = new Point(3, 14);
        this.cbSourcePorts.Name = "cbSourcePorts";
        this.cbSourcePorts.Size = new Size(310, 23);
        this.cbSourcePorts.TabIndex = 2;
        this.cbSourcePorts.SelectedIndexChanged += this.cbSourcePorts_SelectedIndexChanged;
        // 
        // lbSelectedIWAD
        // 
        this.lbSelectedIWAD.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.lbSelectedIWAD.AutoEllipsis = true;
        this.lbSelectedIWAD.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
        this.lbSelectedIWAD.ForeColor = SystemColors.ControlLightLight;
        this.lbSelectedIWAD.Location = new Point(40, 36);
        this.lbSelectedIWAD.Name = "lbSelectedIWAD";
        this.lbSelectedIWAD.Size = new Size(273, 15);
        this.lbSelectedIWAD.TabIndex = 4;
        this.lbSelectedIWAD.Text = "No IWAD selected";
        this.ttHints.SetToolTip(this.lbSelectedIWAD, "Loaded IWAD information will appear here when you select a known WAD.");
        // 
        // lbIWADs
        // 
        this.lbIWADs.AutoSize = true;
        this.lbIWADs.Location = new Point(3, 36);
        this.lbIWADs.Name = "lbIWADs";
        this.lbIWADs.Size = new Size(37, 15);
        this.lbIWADs.TabIndex = 3;
        this.lbIWADs.Text = "IWAD";
        // 
        // lbSourcePort
        // 
        this.lbSourcePort.AutoSize = true;
        this.lbSourcePort.Location = new Point(3, 0);
        this.lbSourcePort.Name = "lbSourcePort";
        this.lbSourcePort.Size = new Size(68, 15);
        this.lbSourcePort.TabIndex = 1;
        this.lbSourcePort.Text = "Source port";
        // 
        // pnOptions
        // 
        this.pnOptions.BackColor = SystemColors.Window;
        this.pnOptions.Controls.Add(this.btnMore);
        this.pnOptions.Controls.Add(this.btnImportZdl);
        this.pnOptions.Controls.Add(this.cbCommandLineArguments);
        this.pnOptions.Controls.Add(this.btnMultiplayer);
        this.pnOptions.Controls.Add(this.btnPlay);
        this.pnOptions.Controls.Add(this.lbExtraCommandLineArguments);
        this.pnOptions.Dock = DockStyle.Bottom;
        this.pnOptions.Location = new Point(0, 227);
        this.pnOptions.Margin = new Padding(0);
        this.pnOptions.MaximumSize = new Size(0, 63);
        this.pnOptions.MinimumSize = new Size(0, 63);
        this.pnOptions.Name = "pnOptions";
        this.pnOptions.Size = new Size(648, 63);
        this.pnOptions.TabIndex = 2;
        // 
        // btnMore
        // 
        this.btnMore.Location = new Point(91, 36);
        this.btnMore.Name = "btnMore";
        this.btnMore.Size = new Size(92, 25);
        this.btnMore.TabIndex = 4;
        this.btnMore.Text = "More options";
        this.ttHints.SetToolTip(this.btnMore, "Display more options");
        this.btnMore.UseVisualStyleBackColor = true;
        // 
        // btnImportZdl
        // 
        this.btnImportZdl.Location = new Point(3, 36);
        this.btnImportZdl.Name = "btnImportZdl";
        this.btnImportZdl.Size = new Size(86, 25);
        this.btnImportZdl.TabIndex = 3;
        this.btnImportZdl.Text = "Import ZDL";
        this.ttHints.SetToolTip(this.btnImportZdl, "Import a previous (q)ZDL (most likely \"qzdl.ini\") configuration file.");
        this.btnImportZdl.UseVisualStyleBackColor = true;
        this.btnImportZdl.Click += this.btnImportZdl_Click;
        // 
        // cbCommandLineArguments
        // 
        this.cbCommandLineArguments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.cbCommandLineArguments.CueText = "(None)";
        this.cbCommandLineArguments.FormattingEnabled = true;
        this.cbCommandLineArguments.Items.AddRange(new object[] { "(None)" });
        this.cbCommandLineArguments.Location = new Point(3, 13);
        this.cbCommandLineArguments.Name = "cbCommandLineArguments";
        this.cbCommandLineArguments.Size = new Size(642, 23);
        this.cbCommandLineArguments.TabIndex = 2;
        this.ttHints.SetToolTip(this.cbCommandLineArguments, "Any additional command line arguments to pass to the source port.");
        // 
        // btnMultiplayer
        // 
        this.btnMultiplayer.AllowDrop = true;
        this.btnMultiplayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnMultiplayer.FileDragging = true;
        this.btnMultiplayer.Location = new Point(401, 36);
        this.btnMultiplayer.Name = "btnMultiplayer";
        this.btnMultiplayer.Size = new Size(121, 25);
        this.btnMultiplayer.TabIndex = 5;
        this.btnMultiplayer.Text = "Multiplayer";
        this.ttHints.SetToolTip(this.btnMultiplayer, resources.GetString("btnMultiplayer.ToolTip"));
        this.btnMultiplayer.UseVisualStyleBackColor = true;
        this.btnMultiplayer.Click += this.btnMultiplayer_Click;
        // 
        // btnPlay
        // 
        this.btnPlay.AllowDrop = true;
        this.btnPlay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnPlay.Enabled = false;
        this.btnPlay.FileDragging = true;
        this.btnPlay.Location = new Point(524, 36);
        this.btnPlay.Name = "btnPlay";
        this.btnPlay.Size = new Size(121, 25);
        this.btnPlay.TabIndex = 6;
        this.btnPlay.Text = "Launch";
        this.ttHints.SetToolTip(this.btnPlay, resources.GetString("btnPlay.ToolTip"));
        this.btnPlay.UseVisualStyleBackColor = true;
        this.btnPlay.Click += this.btnPlay_Click;
        // 
        // lbExtraCommandLineArguments
        // 
        this.lbExtraCommandLineArguments.AutoSize = true;
        this.lbExtraCommandLineArguments.Location = new Point(3, -2);
        this.lbExtraCommandLineArguments.Name = "lbExtraCommandLineArguments";
        this.lbExtraCommandLineArguments.Size = new Size(173, 15);
        this.lbExtraCommandLineArguments.TabIndex = 1;
        this.lbExtraCommandLineArguments.Text = "Extra command line arguments";
        // 
        // pnMultiplayer
        // 
        this.pnMultiplayer.BackColor = SystemColors.Menu;
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerPlayers);
        this.pnMultiplayer.Controls.Add(this.btnMultiplayerConfig);
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerSaveGame);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerSaveGame);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerDMFLAGS2);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerDMFLAGS2);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerDMFLAGS);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerDMFLAGS);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerTimeLimit);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerTimeLimit);
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerExtratic);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerExtratic);
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerDup);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerDup);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerFragLimit);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerFragLimit);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerPort);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerPort);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerHostnameIP);
        this.pnMultiplayer.Controls.Add(this.txtMultiplayerHostnameIp);
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerNetMode);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerNetMode);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerPlayers);
        this.pnMultiplayer.Controls.Add(this.cbMultiplayerGameMode);
        this.pnMultiplayer.Controls.Add(this.lbMultiplayerGameMode);
        this.pnMultiplayer.Dock = DockStyle.Bottom;
        this.pnMultiplayer.Location = new Point(0, 290);
        this.pnMultiplayer.Margin = new Padding(0);
        this.pnMultiplayer.Name = "pnMultiplayer";
        this.pnMultiplayer.Size = new Size(648, 117);
        this.pnMultiplayer.TabIndex = 3;
        this.pnMultiplayer.Visible = false;
        // 
        // cbMultiplayerPlayers
        // 
        this.cbMultiplayerPlayers.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMultiplayerPlayers.FormattingEnabled = true;
        this.cbMultiplayerPlayers.Items.AddRange(new object[] { "Joining", "1", "2", "3", "4", "5", "6", "7", "8", "More..." });
        this.cbMultiplayerPlayers.Location = new Point(3, 53);
        this.cbMultiplayerPlayers.Name = "cbMultiplayerPlayers";
        this.cbMultiplayerPlayers.Size = new Size(121, 23);
        this.cbMultiplayerPlayers.TabIndex = 11;
        this.cbMultiplayerPlayers.SelectedIndexChanged += this.cbMultiplayerPlayers_SelectedIndexChanged;
        this.cbMultiplayerPlayers.KeyDown += this.cbMultiplayerPlayers_KeyDown;
        // 
        // btnMultiplayerConfig
        // 
        this.btnMultiplayerConfig.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnMultiplayerConfig.Font = new Font("Wingdings", 9F, FontStyle.Regular, GraphicsUnit.Point, 2);
        this.btnMultiplayerConfig.Location = new Point(620, 89);
        this.btnMultiplayerConfig.Name = "btnMultiplayerConfig";
        this.btnMultiplayerConfig.Size = new Size(25, 25);
        this.btnMultiplayerConfig.TabIndex = 24;
        this.btnMultiplayerConfig.Text = "â";
        this.btnMultiplayerConfig.TextAlign = ContentAlignment.BottomCenter;
        this.btnMultiplayerConfig.UseVisualStyleBackColor = true;
        // 
        // cbMultiplayerSaveGame
        // 
        this.cbMultiplayerSaveGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.cbMultiplayerSaveGame.CueText = "The path to the save game to load";
        this.cbMultiplayerSaveGame.FormattingEnabled = true;
        this.cbMultiplayerSaveGame.Items.AddRange(new object[] { "(None)", "(Browse)" });
        this.cbMultiplayerSaveGame.Location = new Point(309, 91);
        this.cbMultiplayerSaveGame.Name = "cbMultiplayerSaveGame";
        this.cbMultiplayerSaveGame.Size = new Size(309, 23);
        this.cbMultiplayerSaveGame.TabIndex = 23;
        this.cbMultiplayerSaveGame.SelectedIndexChanged += this.cbMultiplayerSaveGame_SelectedIndexChanged;
        // 
        // lbMultiplayerSaveGame
        // 
        this.lbMultiplayerSaveGame.AutoSize = true;
        this.lbMultiplayerSaveGame.Location = new Point(309, 76);
        this.lbMultiplayerSaveGame.Name = "lbMultiplayerSaveGame";
        this.lbMultiplayerSaveGame.Size = new Size(64, 15);
        this.lbMultiplayerSaveGame.TabIndex = 19;
        this.lbMultiplayerSaveGame.Text = "Save game";
        // 
        // lbMultiplayerDMFLAGS2
        // 
        this.lbMultiplayerDMFLAGS2.AutoSize = true;
        this.lbMultiplayerDMFLAGS2.Location = new Point(509, 38);
        this.lbMultiplayerDMFLAGS2.Name = "lbMultiplayerDMFLAGS2";
        this.lbMultiplayerDMFLAGS2.Size = new Size(66, 15);
        this.lbMultiplayerDMFLAGS2.TabIndex = 10;
        this.lbMultiplayerDMFLAGS2.Text = "DMFLAGS2";
        // 
        // txtMultiplayerDMFLAGS2
        // 
        this.txtMultiplayerDMFLAGS2.CueText = "Default: 0";
        this.txtMultiplayerDMFLAGS2.Location = new Point(509, 54);
        this.txtMultiplayerDMFLAGS2.MaxLength = 10;
        this.txtMultiplayerDMFLAGS2.Name = "txtMultiplayerDMFLAGS2";
        this.txtMultiplayerDMFLAGS2.Size = new Size(135, 23);
        this.txtMultiplayerDMFLAGS2.TabIndex = 15;
        this.txtMultiplayerDMFLAGS2.KeyDown += this.txtNumericOnly_KeyDown;
        // 
        // lbMultiplayerDMFLAGS
        // 
        this.lbMultiplayerDMFLAGS.AutoSize = true;
        this.lbMultiplayerDMFLAGS.Location = new Point(372, 38);
        this.lbMultiplayerDMFLAGS.Name = "lbMultiplayerDMFLAGS";
        this.lbMultiplayerDMFLAGS.Size = new Size(60, 15);
        this.lbMultiplayerDMFLAGS.TabIndex = 9;
        this.lbMultiplayerDMFLAGS.Text = "DMFLAGS";
        // 
        // txtMultiplayerDMFLAGS
        // 
        this.txtMultiplayerDMFLAGS.CueText = "Default: 0";
        this.txtMultiplayerDMFLAGS.Location = new Point(372, 54);
        this.txtMultiplayerDMFLAGS.MaxLength = 10;
        this.txtMultiplayerDMFLAGS.Name = "txtMultiplayerDMFLAGS";
        this.txtMultiplayerDMFLAGS.Size = new Size(135, 23);
        this.txtMultiplayerDMFLAGS.TabIndex = 14;
        this.txtMultiplayerDMFLAGS.KeyDown += this.txtNumericOnly_KeyDown;
        // 
        // lbMultiplayerTimeLimit
        // 
        this.lbMultiplayerTimeLimit.AutoSize = true;
        this.lbMultiplayerTimeLimit.Location = new Point(249, 38);
        this.lbMultiplayerTimeLimit.Name = "lbMultiplayerTimeLimit";
        this.lbMultiplayerTimeLimit.Size = new Size(60, 15);
        this.lbMultiplayerTimeLimit.TabIndex = 8;
        this.lbMultiplayerTimeLimit.Text = "Time limit";
        // 
        // txtMultiplayerTimeLimit
        // 
        this.txtMultiplayerTimeLimit.CueText = "Default: 0";
        this.txtMultiplayerTimeLimit.Location = new Point(249, 54);
        this.txtMultiplayerTimeLimit.MaxLength = 10;
        this.txtMultiplayerTimeLimit.Name = "txtMultiplayerTimeLimit";
        this.txtMultiplayerTimeLimit.Size = new Size(121, 23);
        this.txtMultiplayerTimeLimit.TabIndex = 13;
        this.txtMultiplayerTimeLimit.KeyDown += this.txtNumericOnly_KeyDown;
        // 
        // cbMultiplayerExtratic
        // 
        this.cbMultiplayerExtratic.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMultiplayerExtratic.FormattingEnabled = true;
        this.cbMultiplayerExtratic.Items.AddRange(new object[] { "Off (Default)", "On" });
        this.cbMultiplayerExtratic.Location = new Point(215, 91);
        this.cbMultiplayerExtratic.Name = "cbMultiplayerExtratic";
        this.cbMultiplayerExtratic.Size = new Size(93, 23);
        this.cbMultiplayerExtratic.TabIndex = 22;
        // 
        // lbMultiplayerExtratic
        // 
        this.lbMultiplayerExtratic.AutoSize = true;
        this.lbMultiplayerExtratic.Location = new Point(215, 76);
        this.lbMultiplayerExtratic.Name = "lbMultiplayerExtratic";
        this.lbMultiplayerExtratic.Size = new Size(46, 15);
        this.lbMultiplayerExtratic.TabIndex = 18;
        this.lbMultiplayerExtratic.Text = "Extratic";
        // 
        // cbMultiplayerDup
        // 
        this.cbMultiplayerDup.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMultiplayerDup.FormattingEnabled = true;
        this.cbMultiplayerDup.Items.AddRange(new object[] { "(Default)", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
        this.cbMultiplayerDup.Location = new Point(125, 91);
        this.cbMultiplayerDup.Name = "cbMultiplayerDup";
        this.cbMultiplayerDup.Size = new Size(89, 23);
        this.cbMultiplayerDup.TabIndex = 21;
        // 
        // lbMultiplayerDup
        // 
        this.lbMultiplayerDup.AutoSize = true;
        this.lbMultiplayerDup.Location = new Point(125, 76);
        this.lbMultiplayerDup.Name = "lbMultiplayerDup";
        this.lbMultiplayerDup.Size = new Size(29, 15);
        this.lbMultiplayerDup.TabIndex = 17;
        this.lbMultiplayerDup.Text = "Dup";
        // 
        // lbMultiplayerFragLimit
        // 
        this.lbMultiplayerFragLimit.AutoSize = true;
        this.lbMultiplayerFragLimit.Location = new Point(126, 38);
        this.lbMultiplayerFragLimit.Name = "lbMultiplayerFragLimit";
        this.lbMultiplayerFragLimit.Size = new Size(57, 15);
        this.lbMultiplayerFragLimit.TabIndex = 7;
        this.lbMultiplayerFragLimit.Text = "Frag limit";
        // 
        // txtMultiplayerFragLimit
        // 
        this.txtMultiplayerFragLimit.CueText = "Default: 0";
        this.txtMultiplayerFragLimit.Location = new Point(126, 54);
        this.txtMultiplayerFragLimit.MaxLength = 10;
        this.txtMultiplayerFragLimit.Name = "txtMultiplayerFragLimit";
        this.txtMultiplayerFragLimit.Size = new Size(122, 23);
        this.txtMultiplayerFragLimit.TabIndex = 12;
        this.txtMultiplayerFragLimit.KeyDown += this.txtNumericOnly_KeyDown;
        // 
        // lbMultiplayerPort
        // 
        this.lbMultiplayerPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lbMultiplayerPort.AutoSize = true;
        this.lbMultiplayerPort.Location = new Point(547, 0);
        this.lbMultiplayerPort.Name = "lbMultiplayerPort";
        this.lbMultiplayerPort.Size = new Size(29, 15);
        this.lbMultiplayerPort.TabIndex = 2;
        this.lbMultiplayerPort.Text = "Port";
        // 
        // txtMultiplayerPort
        // 
        this.txtMultiplayerPort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.txtMultiplayerPort.CueText = "5029";
        this.txtMultiplayerPort.Location = new Point(547, 16);
        this.txtMultiplayerPort.MaxLength = 5;
        this.txtMultiplayerPort.Name = "txtMultiplayerPort";
        this.txtMultiplayerPort.Size = new Size(98, 23);
        this.txtMultiplayerPort.TabIndex = 5;
        this.txtMultiplayerPort.KeyDown += this.txtNumericOnly_KeyDown;
        // 
        // lbMultiplayerHostnameIP
        // 
        this.lbMultiplayerHostnameIP.AutoSize = true;
        this.lbMultiplayerHostnameIP.Location = new Point(126, 0);
        this.lbMultiplayerHostnameIP.Name = "lbMultiplayerHostnameIP";
        this.lbMultiplayerHostnameIP.Size = new Size(83, 15);
        this.lbMultiplayerHostnameIP.TabIndex = 1;
        this.lbMultiplayerHostnameIP.Text = "Hostname / IP";
        // 
        // txtMultiplayerHostnameIp
        // 
        this.txtMultiplayerHostnameIp.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.txtMultiplayerHostnameIp.CueText = "hostname.com / 127.0.0.1";
        this.txtMultiplayerHostnameIp.Location = new Point(126, 16);
        this.txtMultiplayerHostnameIp.Name = "txtMultiplayerHostnameIp";
        this.txtMultiplayerHostnameIp.Size = new Size(420, 23);
        this.txtMultiplayerHostnameIp.TabIndex = 4;
        // 
        // cbMultiplayerNetMode
        // 
        this.cbMultiplayerNetMode.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMultiplayerNetMode.FormattingEnabled = true;
        this.cbMultiplayerNetMode.Items.AddRange(new object[] { "(Default)", "Classic P2P (0)", "Client/Server (1)" });
        this.cbMultiplayerNetMode.Location = new Point(3, 91);
        this.cbMultiplayerNetMode.Name = "cbMultiplayerNetMode";
        this.cbMultiplayerNetMode.Size = new Size(121, 23);
        this.cbMultiplayerNetMode.TabIndex = 20;
        this.cbMultiplayerNetMode.SelectedIndexChanged += this.cbMultiplayerNetMode_SelectedIndexChanged;
        // 
        // lbMultiplayerNetMode
        // 
        this.lbMultiplayerNetMode.AutoSize = true;
        this.lbMultiplayerNetMode.Location = new Point(3, 76);
        this.lbMultiplayerNetMode.Name = "lbMultiplayerNetMode";
        this.lbMultiplayerNetMode.Size = new Size(60, 15);
        this.lbMultiplayerNetMode.TabIndex = 16;
        this.lbMultiplayerNetMode.Text = "Net mode";
        // 
        // lbMultiplayerPlayers
        // 
        this.lbMultiplayerPlayers.AutoSize = true;
        this.lbMultiplayerPlayers.Location = new Point(3, 38);
        this.lbMultiplayerPlayers.Name = "lbMultiplayerPlayers";
        this.lbMultiplayerPlayers.Size = new Size(44, 15);
        this.lbMultiplayerPlayers.TabIndex = 6;
        this.lbMultiplayerPlayers.Text = "Players";
        // 
        // cbMultiplayerGameMode
        // 
        this.cbMultiplayerGameMode.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMultiplayerGameMode.FormattingEnabled = true;
        this.cbMultiplayerGameMode.Items.AddRange(new object[] { "Singleplayer", "Co-Op", "Multiplayer", "AltMultiplayer" });
        this.cbMultiplayerGameMode.Location = new Point(3, 15);
        this.cbMultiplayerGameMode.Name = "cbMultiplayerGameMode";
        this.cbMultiplayerGameMode.Size = new Size(121, 23);
        this.cbMultiplayerGameMode.TabIndex = 3;
        this.cbMultiplayerGameMode.SelectedIndexChanged += this.cbMultiplayerGameMode_SelectedIndexChanged;
        // 
        // lbMultiplayerGameMode
        // 
        this.lbMultiplayerGameMode.AutoSize = true;
        this.lbMultiplayerGameMode.Location = new Point(3, 0);
        this.lbMultiplayerGameMode.Name = "lbMultiplayerGameMode";
        this.lbMultiplayerGameMode.Size = new Size(72, 15);
        this.lbMultiplayerGameMode.TabIndex = 0;
        this.lbMultiplayerGameMode.Text = "Game mode";
        // 
        // tcMain
        // 
        this.tcMain.Controls.Add(this.tpEnabledFiles);
        this.tcMain.Controls.Add(this.tpConfig);
        this.tcMain.Controls.Add(this.tpAbout);
        this.tcMain.Dock = DockStyle.Fill;
        this.tcMain.Location = new Point(0, 0);
        this.tcMain.Margin = new Padding(0);
        this.tcMain.Name = "tcMain";
        this.tcMain.SelectedIndex = 0;
        this.tcMain.Size = new Size(648, 227);
        this.tcMain.TabIndex = 1;
        // 
        // tpEnabledFiles
        // 
        this.tpEnabledFiles.BackColor = SystemColors.Window;
        this.tpEnabledFiles.Controls.Add(this.pnFiles);
        this.tpEnabledFiles.Location = new Point(4, 22);
        this.tpEnabledFiles.Name = "tpEnabledFiles";
        this.tpEnabledFiles.Size = new Size(640, 201);
        this.tpEnabledFiles.TabIndex = 0;
        this.tpEnabledFiles.Text = "Enabled files & selected IWAD";
        // 
        // tpConfig
        // 
        this.tpConfig.BackColor = SystemColors.Window;
        this.tpConfig.Controls.Add(this.pnConfigFiles);
        this.tpConfig.Location = new Point(4, 22);
        this.tpConfig.Margin = new Padding(0);
        this.tpConfig.Name = "tpConfig";
        this.tpConfig.Size = new Size(640, 201);
        this.tpConfig.TabIndex = 1;
        this.tpConfig.Text = "Configuration";
        // 
        // pnConfigFiles
        // 
        this.pnConfigFiles.Controls.Add(this.scConfig);
        this.pnConfigFiles.Dock = DockStyle.Fill;
        this.pnConfigFiles.Location = new Point(0, 0);
        this.pnConfigFiles.Name = "pnConfigFiles";
        this.pnConfigFiles.Size = new Size(640, 201);
        this.pnConfigFiles.TabIndex = 4;
        // 
        // scConfig
        // 
        this.scConfig.Dock = DockStyle.Fill;
        this.scConfig.Location = new Point(0, 0);
        this.scConfig.Margin = new Padding(0);
        this.scConfig.Name = "scConfig";
        // 
        // scConfig.Panel1
        // 
        this.scConfig.Panel1.Controls.Add(this.pnConfigSourcePorts);
        // 
        // scConfig.Panel2
        // 
        this.scConfig.Panel2.Controls.Add(this.btnConfigMoveIWADUp);
        this.scConfig.Panel2.Controls.Add(this.btnConfigMoveIWADDown);
        this.scConfig.Panel2.Controls.Add(this.btnConfigEditIWAD);
        this.scConfig.Panel2.Controls.Add(this.btnConfigRemoveIWAD);
        this.scConfig.Panel2.Controls.Add(this.btnConfigAddIWAD);
        this.scConfig.Panel2.Controls.Add(this.lbConfigIWADs);
        this.scConfig.Panel2.Controls.Add(this.tvConfigIWADs);
        this.scConfig.Size = new Size(640, 201);
        this.scConfig.SplitterDistance = 320;
        this.scConfig.TabIndex = 1;
        // 
        // pnConfigSourcePorts
        // 
        this.pnConfigSourcePorts.Controls.Add(this.lbConfigurationSourcePorts);
        this.pnConfigSourcePorts.Controls.Add(this.btnConfigMoveSourcePortUp);
        this.pnConfigSourcePorts.Controls.Add(this.tvConfigSourcePorts);
        this.pnConfigSourcePorts.Controls.Add(this.btnConfigMoveSourcePortDown);
        this.pnConfigSourcePorts.Controls.Add(this.btnConfigAddSourcePort);
        this.pnConfigSourcePorts.Controls.Add(this.btnConfigEditSourcePort);
        this.pnConfigSourcePorts.Controls.Add(this.btnConfigRemoveSourcePort);
        this.pnConfigSourcePorts.Dock = DockStyle.Fill;
        this.pnConfigSourcePorts.Location = new Point(0, 0);
        this.pnConfigSourcePorts.Name = "pnConfigSourcePorts";
        this.pnConfigSourcePorts.Size = new Size(320, 201);
        this.pnConfigSourcePorts.TabIndex = 8;
        // 
        // lbConfigurationSourcePorts
        // 
        this.lbConfigurationSourcePorts.AutoSize = true;
        this.lbConfigurationSourcePorts.Location = new Point(3, 0);
        this.lbConfigurationSourcePorts.Name = "lbConfigurationSourcePorts";
        this.lbConfigurationSourcePorts.Size = new Size(73, 15);
        this.lbConfigurationSourcePorts.TabIndex = 1;
        this.lbConfigurationSourcePorts.Text = "Source ports";
        // 
        // btnConfigMoveSourcePortUp
        // 
        this.btnConfigMoveSourcePortUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnConfigMoveSourcePortUp.Font = new Font("Wingdings", 9F);
        this.btnConfigMoveSourcePortUp.Location = new Point(271, 176);
        this.btnConfigMoveSourcePortUp.Name = "btnConfigMoveSourcePortUp";
        this.btnConfigMoveSourcePortUp.Size = new Size(23, 23);
        this.btnConfigMoveSourcePortUp.TabIndex = 6;
        this.btnConfigMoveSourcePortUp.Text = "á";
        this.btnConfigMoveSourcePortUp.UseVisualStyleBackColor = true;
        this.btnConfigMoveSourcePortUp.Click += this.btnConfigMoveSourcePortUp_Click;
        // 
        // tvConfigSourcePorts
        // 
        this.tvConfigSourcePorts.AllowDrop = true;
        this.tvConfigSourcePorts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.tvConfigSourcePorts.FileDragging = true;
        this.tvConfigSourcePorts.FullRowSelect = true;
        this.tvConfigSourcePorts.HideSelection = false;
        this.tvConfigSourcePorts.HotTracking = true;
        this.tvConfigSourcePorts.ImageIndex = 0;
        this.tvConfigSourcePorts.Location = new Point(3, 16);
        this.tvConfigSourcePorts.MouseDragNodes = true;
        this.tvConfigSourcePorts.Name = "tvConfigSourcePorts";
        this.tvConfigSourcePorts.OnlyAllowDraggedNodesInSameCollection = true;
        this.tvConfigSourcePorts.SelectedImageIndex = 0;
        this.tvConfigSourcePorts.ShowLines = false;
        this.tvConfigSourcePorts.ShowNodeToolTips = true;
        this.tvConfigSourcePorts.ShowPlusMinus = false;
        this.tvConfigSourcePorts.ShowRootLines = false;
        this.tvConfigSourcePorts.Size = new Size(314, 158);
        this.tvConfigSourcePorts.TabIndex = 2;
        this.tvConfigSourcePorts.BeforeSelect += this.tvConfigSourcePorts_BeforeSelect;
        this.tvConfigSourcePorts.AfterSelect += this.tvConfigSourcePorts_AfterSelect;
        this.tvConfigSourcePorts.NodeMouseClick += this.tvRightClick_NodeMouseClick;
        // 
        // btnConfigMoveSourcePortDown
        // 
        this.btnConfigMoveSourcePortDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnConfigMoveSourcePortDown.Font = new Font("Wingdings", 9F);
        this.btnConfigMoveSourcePortDown.Location = new Point(294, 176);
        this.btnConfigMoveSourcePortDown.Name = "btnConfigMoveSourcePortDown";
        this.btnConfigMoveSourcePortDown.Size = new Size(23, 23);
        this.btnConfigMoveSourcePortDown.TabIndex = 7;
        this.btnConfigMoveSourcePortDown.Text = "â";
        this.btnConfigMoveSourcePortDown.UseVisualStyleBackColor = true;
        this.btnConfigMoveSourcePortDown.Click += this.btnConfigMoveSourcePortDown_Click;
        // 
        // btnConfigAddSourcePort
        // 
        this.btnConfigAddSourcePort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigAddSourcePort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.btnConfigAddSourcePort.Location = new Point(3, 176);
        this.btnConfigAddSourcePort.Name = "btnConfigAddSourcePort";
        this.btnConfigAddSourcePort.Size = new Size(23, 23);
        this.btnConfigAddSourcePort.TabIndex = 3;
        this.btnConfigAddSourcePort.Text = "+";
        this.ttHints.SetToolTip(this.btnConfigAddSourcePort, "Add a port executable");
        this.btnConfigAddSourcePort.UseVisualStyleBackColor = true;
        this.btnConfigAddSourcePort.Click += this.btnConfigAddSourcePort_Click;
        // 
        // btnConfigEditSourcePort
        // 
        this.btnConfigEditSourcePort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigEditSourcePort.Font = new Font("Wingdings", 9F);
        this.btnConfigEditSourcePort.Location = new Point(48, 176);
        this.btnConfigEditSourcePort.Name = "btnConfigEditSourcePort";
        this.btnConfigEditSourcePort.Size = new Size(23, 23);
        this.btnConfigEditSourcePort.TabIndex = 5;
        this.btnConfigEditSourcePort.Text = "!";
        this.ttHints.SetToolTip(this.btnConfigEditSourcePort, "Edit the selectd port");
        this.btnConfigEditSourcePort.UseVisualStyleBackColor = true;
        this.btnConfigEditSourcePort.Click += this.btnConfigEditSourcePort_Click;
        // 
        // btnConfigRemoveSourcePort
        // 
        this.btnConfigRemoveSourcePort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigRemoveSourcePort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.btnConfigRemoveSourcePort.Location = new Point(25, 176);
        this.btnConfigRemoveSourcePort.Name = "btnConfigRemoveSourcePort";
        this.btnConfigRemoveSourcePort.Size = new Size(23, 23);
        this.btnConfigRemoveSourcePort.TabIndex = 4;
        this.btnConfigRemoveSourcePort.Text = "X";
        this.ttHints.SetToolTip(this.btnConfigRemoveSourcePort, "Remove the selected external file(s)");
        this.btnConfigRemoveSourcePort.UseVisualStyleBackColor = true;
        this.btnConfigRemoveSourcePort.Click += this.btnConfigRemoveSourcePort_Click;
        // 
        // btnConfigMoveIWADUp
        // 
        this.btnConfigMoveIWADUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnConfigMoveIWADUp.Font = new Font("Wingdings", 9F);
        this.btnConfigMoveIWADUp.Location = new Point(268, 177);
        this.btnConfigMoveIWADUp.Name = "btnConfigMoveIWADUp";
        this.btnConfigMoveIWADUp.Size = new Size(23, 23);
        this.btnConfigMoveIWADUp.TabIndex = 6;
        this.btnConfigMoveIWADUp.Text = "á";
        this.btnConfigMoveIWADUp.UseVisualStyleBackColor = true;
        this.btnConfigMoveIWADUp.Click += this.btnConfigMoveIWADUp_Click;
        // 
        // btnConfigMoveIWADDown
        // 
        this.btnConfigMoveIWADDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnConfigMoveIWADDown.Font = new Font("Wingdings", 9F);
        this.btnConfigMoveIWADDown.Location = new Point(291, 177);
        this.btnConfigMoveIWADDown.Name = "btnConfigMoveIWADDown";
        this.btnConfigMoveIWADDown.Size = new Size(23, 23);
        this.btnConfigMoveIWADDown.TabIndex = 7;
        this.btnConfigMoveIWADDown.Text = "â";
        this.btnConfigMoveIWADDown.UseVisualStyleBackColor = true;
        this.btnConfigMoveIWADDown.Click += this.btnConfigMoveIWADDown_Click;
        // 
        // btnConfigEditIWAD
        // 
        this.btnConfigEditIWAD.AccessibleDescription = "";
        this.btnConfigEditIWAD.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigEditIWAD.Font = new Font("Wingdings", 9F);
        this.btnConfigEditIWAD.Location = new Point(48, 177);
        this.btnConfigEditIWAD.Name = "btnConfigEditIWAD";
        this.btnConfigEditIWAD.Size = new Size(23, 23);
        this.btnConfigEditIWAD.TabIndex = 5;
        this.btnConfigEditIWAD.Text = "!";
        this.ttHints.SetToolTip(this.btnConfigEditIWAD, "Edit the selected internal wad (iwad)");
        this.btnConfigEditIWAD.UseVisualStyleBackColor = true;
        this.btnConfigEditIWAD.Click += this.btnConfigEditIWAD_Click;
        // 
        // btnConfigRemoveIWAD
        // 
        this.btnConfigRemoveIWAD.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigRemoveIWAD.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.btnConfigRemoveIWAD.Location = new Point(25, 177);
        this.btnConfigRemoveIWAD.Name = "btnConfigRemoveIWAD";
        this.btnConfigRemoveIWAD.Size = new Size(23, 23);
        this.btnConfigRemoveIWAD.TabIndex = 4;
        this.btnConfigRemoveIWAD.Text = "X";
        this.ttHints.SetToolTip(this.btnConfigRemoveIWAD, "Remove the selected external file(s)");
        this.btnConfigRemoveIWAD.UseVisualStyleBackColor = true;
        this.btnConfigRemoveIWAD.Click += this.btnConfigRemoveIWAD_Click;
        // 
        // btnConfigAddIWAD
        // 
        this.btnConfigAddIWAD.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.btnConfigAddIWAD.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.btnConfigAddIWAD.Location = new Point(2, 177);
        this.btnConfigAddIWAD.Name = "btnConfigAddIWAD";
        this.btnConfigAddIWAD.Size = new Size(23, 23);
        this.btnConfigAddIWAD.TabIndex = 3;
        this.btnConfigAddIWAD.Text = "+";
        this.ttHints.SetToolTip(this.btnConfigAddIWAD, "Add an external patch wad (pwad) or dehack");
        this.btnConfigAddIWAD.UseVisualStyleBackColor = true;
        this.btnConfigAddIWAD.Click += this.btnConfigAddIWAD_Click;
        // 
        // lbConfigIWADs
        // 
        this.lbConfigIWADs.AutoSize = true;
        this.lbConfigIWADs.Location = new Point(3, 0);
        this.lbConfigIWADs.Name = "lbConfigIWADs";
        this.lbConfigIWADs.Size = new Size(42, 15);
        this.lbConfigIWADs.TabIndex = 1;
        this.lbConfigIWADs.Text = "IWADs";
        // 
        // tvConfigIWADs
        // 
        this.tvConfigIWADs.AllowDrop = true;
        this.tvConfigIWADs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.tvConfigIWADs.FileDragging = true;
        this.tvConfigIWADs.FullRowSelect = true;
        this.tvConfigIWADs.HideSelection = false;
        this.tvConfigIWADs.HotTracking = true;
        this.tvConfigIWADs.ImageIndex = 0;
        this.tvConfigIWADs.Location = new Point(3, 16);
        this.tvConfigIWADs.MouseDragNodes = true;
        this.tvConfigIWADs.Name = "tvConfigIWADs";
        this.tvConfigIWADs.OnlyAllowDraggedNodesInSameCollection = true;
        this.tvConfigIWADs.SelectedImageIndex = 0;
        this.tvConfigIWADs.ShowLines = false;
        this.tvConfigIWADs.ShowNodeToolTips = true;
        this.tvConfigIWADs.ShowPlusMinus = false;
        this.tvConfigIWADs.ShowRootLines = false;
        this.tvConfigIWADs.Size = new Size(314, 158);
        this.tvConfigIWADs.TabIndex = 2;
        this.tvConfigIWADs.BeforeSelect += this.tvConfigIWADs_BeforeSelect;
        this.tvConfigIWADs.AfterSelect += this.tvConfigIWADs_AfterSelect;
        this.tvConfigIWADs.NodeMouseClick += this.tvRightClick_NodeMouseClick;
        // 
        // ttHints
        // 
        this.ttHints.AutoPopDelay = 60000;
        this.ttHints.InitialDelay = 500;
        this.ttHints.ReshowDelay = 100;
        // 
        // lbModified
        // 
        this.lbModified.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lbModified.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
        this.lbModified.ForeColor = SystemColors.ControlLightLight;
        this.lbModified.Location = new Point(440, 2);
        this.lbModified.Name = "lbModified";
        this.lbModified.Size = new Size(204, 16);
        this.lbModified.TabIndex = 2;
        this.lbModified.Text = "Configuration modified";
        this.lbModified.TextAlign = ContentAlignment.MiddleRight;
        this.ttHints.SetToolTip(this.lbModified, "The configuration will be saved when the next auto-save tick occurs,\r\nif not force-saved before it occurs.");
        this.lbModified.Visible = false;
        // 
        // tpAbout
        // 
        this.tpAbout.Controls.Add(this.lbTiny7z);
        this.tpAbout.Controls.Add(this.lbAbout);
        this.tpAbout.Location = new Point(4, 22);
        this.tpAbout.Name = "tpAbout";
        this.tpAbout.Padding = new Padding(3);
        this.tpAbout.Size = new Size(640, 201);
        this.tpAbout.TabIndex = 2;
        this.tpAbout.Text = "About";
        this.tpAbout.UseVisualStyleBackColor = true;
        // 
        // lbAbout
        // 
        this.lbAbout.AutoSize = true;
        this.lbAbout.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        this.lbAbout.Location = new Point(11, 12);
        this.lbAbout.Name = "lbAbout";
        this.lbAbout.Size = new Size(325, 84);
        this.lbAbout.TabIndex = 0;
        this.lbAbout.Text = "DOOMer (GPL v3), Copyright (c) 2025 murrty\r\nhttps://github.com/murrty/DOOMer\r\n\r\nLibraries used (hover for licenses)";
        // 
        // lbTiny7z
        // 
        this.lbTiny7z.AutoSize = true;
        this.lbTiny7z.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        this.lbTiny7z.Location = new Point(33, 100);
        this.lbTiny7z.Name = "lbTiny7z";
        this.lbTiny7z.Size = new Size(344, 21);
        this.lbTiny7z.TabIndex = 1;
        this.lbTiny7z.Text = "tiny7z (MIT), Copyright (c) 2018 princess_daphie";
        this.ttHints.SetToolTip(this.lbTiny7z, resources.GetString("lbTiny7z.ToolTip"));
        // 
        // frmMain
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(648, 407);
        this.Controls.Add(this.lbModified);
        this.Controls.Add(this.tcMain);
        this.Controls.Add(this.pnOptions);
        this.Controls.Add(this.pnMultiplayer);
        this.Icon = Properties.Resources.ProgramIcon;
        this.MinimumSize = new Size(664, 442);
        this.Name = "frmMain";
        this.Text = "DOOMer for GZDoom (and others???)";
        this.pnFiles.ResumeLayout(false);
        this.scFiles.Panel1.ResumeLayout(false);
        this.scFiles.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.scFiles).EndInit();
        this.scFiles.ResumeLayout(false);
        this.pnExternalFiles.ResumeLayout(false);
        this.pnExternalFiles.PerformLayout();
        this.pnInternalWADs.ResumeLayout(false);
        this.pnInternalWADs.PerformLayout();
        this.pnOptions.ResumeLayout(false);
        this.pnOptions.PerformLayout();
        this.pnMultiplayer.ResumeLayout(false);
        this.pnMultiplayer.PerformLayout();
        this.tcMain.ResumeLayout(false);
        this.tpEnabledFiles.ResumeLayout(false);
        this.tpConfig.ResumeLayout(false);
        this.pnConfigFiles.ResumeLayout(false);
        this.scConfig.Panel1.ResumeLayout(false);
        this.scConfig.Panel2.ResumeLayout(false);
        this.scConfig.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.scConfig).EndInit();
        this.scConfig.ResumeLayout(false);
        this.pnConfigSourcePorts.ResumeLayout(false);
        this.pnConfigSourcePorts.PerformLayout();
        this.tpAbout.ResumeLayout(false);
        this.tpAbout.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private Panel pnFiles;
    private SplitContainer scFiles;
    private Panel pnOptions;
    private ExplorerTreeView tvExternalFiles;
    private Label lbSourcePort;
    private ComboBoxEx cbSourcePorts;
    private Label lbIWADs;
    private Button btnExternalFileMoveUp;
    private Button btnExternalFileMoveDown;
    private Button btnExternalFileRemoveSelected;
    private Button btnExternalFileAddFile;
    private Button btnExternalFileAddDirectory;
    private Button btnExternalFileToggle;
    private ComboBox cbSkillLevel;
    private ComboBoxEx cbMaps;
    private Label lbMaps;
    private Label lbSkillLevel;
    private Label lbExtraCommandLineArguments;
    private ButtonEx btnPlay;
    private ButtonEx btnMultiplayer;
    private Panel pnMultiplayer;
    private TabControl tcMain;
    private TabPage tpEnabledFiles;
    private TabPage tpConfig;
    private ComboBoxEx cbCommandLineArguments;
    private Button btnImportZdl;
    private Label lbSelectedIWAD;
    private ToolTip ttHints;
    private Label lbMultiplayerPlayers;
    private ComboBox cbMultiplayerGameMode;
    private Label lbMultiplayerGameMode;
    private ComboBox cbMultiplayerNetMode;
    private Label lbMultiplayerNetMode;
    private Label lbMultiplayerPort;
    private TextBoxEx txtMultiplayerPort;
    private Label lbMultiplayerHostnameIP;
    private TextBoxEx txtMultiplayerHostnameIp;
    private Label lbMultiplayerFragLimit;
    private TextBoxEx txtMultiplayerFragLimit;
    private ComboBox cbMultiplayerExtratic;
    private Label lbMultiplayerExtratic;
    private ComboBox cbMultiplayerDup;
    private Label lbMultiplayerDup;
    private Label lbMultiplayerDMFLAGS2;
    private TextBoxEx txtMultiplayerDMFLAGS2;
    private Label lbMultiplayerDMFLAGS;
    private TextBoxEx txtMultiplayerDMFLAGS;
    private Label lbMultiplayerTimeLimit;
    private TextBoxEx txtMultiplayerTimeLimit;
    private ComboBoxEx cbMultiplayerSaveGame;
    private Label lbMultiplayerSaveGame;
    private Button btnMore;
    private Panel pnConfigFiles;
    private SplitContainer scConfig;
    private Button btnConfigMoveSourcePortUp;
    private Button btnConfigMoveSourcePortDown;
    private Button btnConfigEditSourcePort;
    private Button btnConfigRemoveSourcePort;
    private Button btnConfigAddSourcePort;
    private Label lbConfigurationSourcePorts;
    private ExplorerTreeView tvConfigSourcePorts;
    private Button btnConfigMoveIWADUp;
    private Button btnConfigMoveIWADDown;
    private Button btnConfigEditIWAD;
    private Button btnConfigRemoveIWAD;
    private Button btnConfigAddIWAD;
    private Label lbConfigIWADs;
    private ExplorerTreeView tvConfigIWADs;
    private ExplorerTreeView tvIWADs;
    private Button btnMultiplayerConfig;
    private ComboBox cbMultiplayerPlayers;
    private Label lbModified;
    private Button btnExternalFileEdit;
    private CheckBox chkAddAsDependant;
    private Label lbExternalFiles;
    private Panel pnExternalFiles;
    private Panel pnInternalWADs;
    private Panel pnConfigSourcePorts;
    private TabPage tpAbout;
    private Label lbAbout;
    private Label lbTiny7z;
}
