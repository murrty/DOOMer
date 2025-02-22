namespace DOOMer.WinForms;

partial class frmPackageExplorer {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        this.pnDialogButtons = new System.Windows.Forms.Panel();
        this.chkFullRowSelect = new System.Windows.Forms.CheckBox();
        this.btnExtractAll = new System.Windows.Forms.Button();
        this.btnExtractSelectedEntries = new System.Windows.Forms.Button();
        this.btnOk = new System.Windows.Forms.Button();
        this.pnArgumentDisplay = new System.Windows.Forms.Panel();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.tcMain = new System.Windows.Forms.TabControl();
        this.tpEntries = new System.Windows.Forms.TabPage();
        this.lvEntries = new System.Windows.Forms.ListView();
        this.chEntryName = new System.Windows.Forms.ColumnHeader();
        this.chEntrySize = new System.Windows.Forms.ColumnHeader();
        this.chOffset = new System.Windows.Forms.ColumnHeader();
        this.chCRC = new System.Windows.Forms.ColumnHeader();
        this.tpFileInfo = new System.Windows.Forms.TabPage();
        this.txtFileInformation = new System.Windows.Forms.TextBox();
        this.txtFile = new System.Windows.Forms.TextBox();
        this.lbInternalWad = new System.Windows.Forms.Label();
        this.lbFileName = new System.Windows.Forms.Label();
        this.ttHints = new System.Windows.Forms.ToolTip(this.components);
        this.pnDialogButtons.SuspendLayout();
        this.pnArgumentDisplay.SuspendLayout();
        this.tcMain.SuspendLayout();
        this.tpEntries.SuspendLayout();
        this.tpFileInfo.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnDialogButtons
        // 
        this.pnDialogButtons.BackColor = System.Drawing.SystemColors.Menu;
        this.pnDialogButtons.Controls.Add(this.chkFullRowSelect);
        this.pnDialogButtons.Controls.Add(this.btnExtractAll);
        this.pnDialogButtons.Controls.Add(this.btnExtractSelectedEntries);
        this.pnDialogButtons.Controls.Add(this.btnOk);
        this.pnDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnDialogButtons.Location = new System.Drawing.Point(0, 325);
        this.pnDialogButtons.Name = "pnDialogButtons";
        this.pnDialogButtons.Size = new System.Drawing.Size(504, 40);
        this.pnDialogButtons.TabIndex = 2;
        // 
        // chkFullRowSelect
        // 
        this.chkFullRowSelect.AutoSize = true;
        this.chkFullRowSelect.Location = new System.Drawing.Point(207, 10);
        this.chkFullRowSelect.Name = "chkFullRowSelect";
        this.chkFullRowSelect.Size = new System.Drawing.Size(100, 19);
        this.chkFullRowSelect.TabIndex = 3;
        this.chkFullRowSelect.Text = "Full row select";
        this.chkFullRowSelect.UseVisualStyleBackColor = true;
        this.chkFullRowSelect.CheckedChanged += this.chkFullRowSelect_CheckedChanged;
        // 
        // btnExtractAll
        // 
        this.btnExtractAll.Location = new System.Drawing.Point(116, 7);
        this.btnExtractAll.Name = "btnExtractAll";
        this.btnExtractAll.Size = new System.Drawing.Size(85, 23);
        this.btnExtractAll.TabIndex = 2;
        this.btnExtractAll.Text = "Extract all...";
        this.ttHints.SetToolTip(this.btnExtractAll, "Extracts all entries");
        this.btnExtractAll.UseVisualStyleBackColor = true;
        this.btnExtractAll.Click += this.btnExtractAll_Click;
        // 
        // btnExtractSelectedEntries
        // 
        this.btnExtractSelectedEntries.Location = new System.Drawing.Point(12, 7);
        this.btnExtractSelectedEntries.Name = "btnExtractSelectedEntries";
        this.btnExtractSelectedEntries.Size = new System.Drawing.Size(98, 23);
        this.btnExtractSelectedEntries.TabIndex = 1;
        this.btnExtractSelectedEntries.Text = "Extract entries...";
        this.ttHints.SetToolTip(this.btnExtractSelectedEntries, "Extracts the selected entries");
        this.btnExtractSelectedEntries.UseVisualStyleBackColor = true;
        this.btnExtractSelectedEntries.Click += this.btnExtractSelectedEntries_Click;
        // 
        // btnOk
        // 
        this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnOk.Location = new System.Drawing.Point(417, 7);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 4;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += this.btnOk_Click;
        // 
        // pnArgumentDisplay
        // 
        this.pnArgumentDisplay.BackColor = System.Drawing.SystemColors.Window;
        this.pnArgumentDisplay.Controls.Add(this.btnBrowse);
        this.pnArgumentDisplay.Controls.Add(this.tcMain);
        this.pnArgumentDisplay.Controls.Add(this.txtFile);
        this.pnArgumentDisplay.Controls.Add(this.lbInternalWad);
        this.pnArgumentDisplay.Controls.Add(this.lbFileName);
        this.pnArgumentDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnArgumentDisplay.Location = new System.Drawing.Point(0, 0);
        this.pnArgumentDisplay.MinimumSize = new System.Drawing.Size(0, 123);
        this.pnArgumentDisplay.Name = "pnArgumentDisplay";
        this.pnArgumentDisplay.Size = new System.Drawing.Size(504, 325);
        this.pnArgumentDisplay.TabIndex = 1;
        // 
        // btnBrowse
        // 
        this.btnBrowse.Location = new System.Drawing.Point(470, 18);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(31, 25);
        this.btnBrowse.TabIndex = 3;
        this.btnBrowse.Text = "...";
        this.btnBrowse.UseVisualStyleBackColor = true;
        this.btnBrowse.Click += this.btnBrowse_Click;
        // 
        // tcMain
        // 
        this.tcMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.tcMain.Controls.Add(this.tpEntries);
        this.tcMain.Controls.Add(this.tpFileInfo);
        this.tcMain.Location = new System.Drawing.Point(3, 48);
        this.tcMain.Name = "tcMain";
        this.tcMain.SelectedIndex = 0;
        this.tcMain.Size = new System.Drawing.Size(501, 274);
        this.tcMain.TabIndex = 4;
        // 
        // tpEntries
        // 
        this.tpEntries.Controls.Add(this.lvEntries);
        this.tpEntries.Location = new System.Drawing.Point(4, 22);
        this.tpEntries.Name = "tpEntries";
        this.tpEntries.Padding = new System.Windows.Forms.Padding(3);
        this.tpEntries.Size = new System.Drawing.Size(493, 248);
        this.tpEntries.TabIndex = 0;
        this.tpEntries.Text = "Entries (0)";
        this.tpEntries.UseVisualStyleBackColor = true;
        // 
        // lvEntries
        // 
        this.lvEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.chEntryName, this.chEntrySize, this.chOffset, this.chCRC });
        this.lvEntries.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lvEntries.FullRowSelect = true;
        this.lvEntries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.lvEntries.Location = new System.Drawing.Point(3, 3);
        this.lvEntries.Name = "lvEntries";
        this.lvEntries.ShowGroups = false;
        this.lvEntries.Size = new System.Drawing.Size(487, 242);
        this.lvEntries.TabIndex = 1;
        this.lvEntries.UseCompatibleStateImageBehavior = false;
        this.lvEntries.View = System.Windows.Forms.View.Details;
        // 
        // chEntryName
        // 
        this.chEntryName.Text = "Name";
        // 
        // chEntrySize
        // 
        this.chEntrySize.Text = "Size";
        // 
        // chOffset
        // 
        this.chOffset.Text = "Offset";
        // 
        // chCRC
        // 
        this.chCRC.Text = "CRC32";
        // 
        // tpFileInfo
        // 
        this.tpFileInfo.Controls.Add(this.txtFileInformation);
        this.tpFileInfo.Location = new System.Drawing.Point(4, 22);
        this.tpFileInfo.Name = "tpFileInfo";
        this.tpFileInfo.Padding = new System.Windows.Forms.Padding(3);
        this.tpFileInfo.Size = new System.Drawing.Size(493, 248);
        this.tpFileInfo.TabIndex = 1;
        this.tpFileInfo.Text = "File information";
        this.tpFileInfo.UseVisualStyleBackColor = true;
        // 
        // txtFileInformation
        // 
        this.txtFileInformation.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtFileInformation.Location = new System.Drawing.Point(3, 3);
        this.txtFileInformation.Multiline = true;
        this.txtFileInformation.Name = "txtFileInformation";
        this.txtFileInformation.ReadOnly = true;
        this.txtFileInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtFileInformation.Size = new System.Drawing.Size(487, 242);
        this.txtFileInformation.TabIndex = 1;
        // 
        // txtFile
        // 
        this.txtFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtFile.Location = new System.Drawing.Point(3, 19);
        this.txtFile.Name = "txtFile";
        this.txtFile.ReadOnly = true;
        this.txtFile.Size = new System.Drawing.Size(465, 23);
        this.txtFile.TabIndex = 2;
        // 
        // lbInternalWad
        // 
        this.lbInternalWad.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.lbInternalWad.AutoEllipsis = true;
        this.lbInternalWad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
        this.lbInternalWad.ForeColor = System.Drawing.SystemColors.ControlLightLight;
        this.lbInternalWad.Location = new System.Drawing.Point(59, 3);
        this.lbInternalWad.Name = "lbInternalWad";
        this.lbInternalWad.Size = new System.Drawing.Size(442, 15);
        this.lbInternalWad.TabIndex = 8;
        this.lbInternalWad.Text = "no internal wad found";
        // 
        // lbFileName
        // 
        this.lbFileName.AutoSize = true;
        this.lbFileName.Location = new System.Drawing.Point(3, 3);
        this.lbFileName.Name = "lbFileName";
        this.lbFileName.Size = new System.Drawing.Size(58, 15);
        this.lbFileName.TabIndex = 1;
        this.lbFileName.Text = "File name";
        // 
        // frmPackageExplorer
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Window;
        this.ClientSize = new System.Drawing.Size(504, 365);
        this.Controls.Add(this.pnArgumentDisplay);
        this.Controls.Add(this.pnDialogButtons);
        this.Icon = Properties.Resources.ProgramIcon;
        this.MinimumSize = new System.Drawing.Size(520, 400);
        this.Name = "frmPackageExplorer";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "DOOM package explorer";
        this.ResizeEnd += this.frmEntryExplorer_ResizeEnd;
        this.pnDialogButtons.ResumeLayout(false);
        this.pnDialogButtons.PerformLayout();
        this.pnArgumentDisplay.ResumeLayout(false);
        this.pnArgumentDisplay.PerformLayout();
        this.tcMain.ResumeLayout(false);
        this.tpEntries.ResumeLayout(false);
        this.tpFileInfo.ResumeLayout(false);
        this.tpFileInfo.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnDialogButtons;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Panel pnArgumentDisplay;
    private System.Windows.Forms.Label lbFileName;
    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.ToolTip ttHints;
    private System.Windows.Forms.Button btnExtractAll;
    private System.Windows.Forms.Button btnExtractSelectedEntries;
    private System.Windows.Forms.ListView lvEntries;
    private System.Windows.Forms.ColumnHeader chEntryName;
    private System.Windows.Forms.Label lbInternalWad;
    private System.Windows.Forms.ColumnHeader chEntrySize;
    private System.Windows.Forms.ColumnHeader chOffset;
    private System.Windows.Forms.CheckBox chkFullRowSelect;
    private System.Windows.Forms.TabControl tcMain;
    private System.Windows.Forms.TabPage tpEntries;
    private System.Windows.Forms.TabPage tpFileInfo;
    private System.Windows.Forms.TextBox txtFileInformation;
    private System.Windows.Forms.ColumnHeader chCRC;
    private System.Windows.Forms.Button btnBrowse;
}