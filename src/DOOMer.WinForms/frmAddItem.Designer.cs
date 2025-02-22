namespace DOOMer.WinForms;

partial class frmAddItem {
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddItem));
        this.lbMultiplayerHostnameIP = new System.Windows.Forms.Label();
        this.txtName = new Controls.TextBoxEx();
        this.label1 = new System.Windows.Forms.Label();
        this.txtPath = new Controls.TextBoxEx();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.pnDialogButtons = new System.Windows.Forms.Panel();
        this.chkDirectoryAsFile = new System.Windows.Forms.CheckBox();
        this.chkAddAsGroup = new System.Windows.Forms.CheckBox();
        this.btnOK = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.chkRelativePathing = new System.Windows.Forms.CheckBox();
        this.ttHints = new System.Windows.Forms.ToolTip(this.components);
        this.lbParent = new System.Windows.Forms.Label();
        this.pnDialogButtons.SuspendLayout();
        this.SuspendLayout();
        // 
        // lbMultiplayerHostnameIP
        // 
        this.lbMultiplayerHostnameIP.AutoSize = true;
        this.lbMultiplayerHostnameIP.Location = new System.Drawing.Point(12, 9);
        this.lbMultiplayerHostnameIP.Name = "lbMultiplayerHostnameIP";
        this.lbMultiplayerHostnameIP.Size = new System.Drawing.Size(39, 15);
        this.lbMultiplayerHostnameIP.TabIndex = 1;
        this.lbMultiplayerHostnameIP.Text = "Name";
        // 
        // txtName
        // 
        this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtName.Location = new System.Drawing.Point(12, 25);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(390, 23);
        this.txtName.TabIndex = 2;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 54);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(52, 15);
        this.label1.TabIndex = 3;
        this.label1.Text = "File path";
        // 
        // txtPath
        // 
        this.txtPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtPath.Location = new System.Drawing.Point(36, 70);
        this.txtPath.Name = "txtPath";
        this.txtPath.Size = new System.Drawing.Size(319, 23);
        this.txtPath.TabIndex = 5;
        // 
        // btnBrowse
        // 
        this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnBrowse.Location = new System.Drawing.Point(361, 68);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(38, 24);
        this.btnBrowse.TabIndex = 6;
        this.btnBrowse.Text = "...";
        this.btnBrowse.UseVisualStyleBackColor = true;
        this.btnBrowse.Click += this.btnBrowse_Click;
        // 
        // pnDialogButtons
        // 
        this.pnDialogButtons.BackColor = System.Drawing.SystemColors.Menu;
        this.pnDialogButtons.Controls.Add(this.chkDirectoryAsFile);
        this.pnDialogButtons.Controls.Add(this.chkAddAsGroup);
        this.pnDialogButtons.Controls.Add(this.btnOK);
        this.pnDialogButtons.Controls.Add(this.btnCancel);
        this.pnDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnDialogButtons.Location = new System.Drawing.Point(0, 105);
        this.pnDialogButtons.Name = "pnDialogButtons";
        this.pnDialogButtons.Size = new System.Drawing.Size(414, 40);
        this.pnDialogButtons.TabIndex = 7;
        // 
        // chkDirectoryAsFile
        // 
        this.chkDirectoryAsFile.AutoSize = true;
        this.chkDirectoryAsFile.Enabled = false;
        this.chkDirectoryAsFile.Location = new System.Drawing.Point(114, 11);
        this.chkDirectoryAsFile.Name = "chkDirectoryAsFile";
        this.chkDirectoryAsFile.Size = new System.Drawing.Size(106, 19);
        this.chkDirectoryAsFile.TabIndex = 11;
        this.chkDirectoryAsFile.Text = "Directory as file";
        this.ttHints.SetToolTip(this.chkDirectoryAsFile, "If enabled, the directory will be added as-is to the arguments.\r\nIf disabled, every file in the diectory will be enumerated and added inidividually per launch.");
        this.chkDirectoryAsFile.UseVisualStyleBackColor = true;
        this.chkDirectoryAsFile.Visible = false;
        // 
        // chkAddAsGroup
        // 
        this.chkAddAsGroup.AutoSize = true;
        this.chkAddAsGroup.Enabled = false;
        this.chkAddAsGroup.Location = new System.Drawing.Point(12, 11);
        this.chkAddAsGroup.Name = "chkAddAsGroup";
        this.chkAddAsGroup.Size = new System.Drawing.Size(96, 19);
        this.chkAddAsGroup.TabIndex = 8;
        this.chkAddAsGroup.Text = "Add as group";
        this.ttHints.SetToolTip(this.chkAddAsGroup, "Whether the directory will be added as a group.\r\nA group contains any wad files in any location, but launched together with the group.\r\nA directory will only load wads associated with that directory.");
        this.chkAddAsGroup.UseVisualStyleBackColor = true;
        this.chkAddAsGroup.Visible = false;
        // 
        // btnOK
        // 
        this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnOK.Location = new System.Drawing.Point(246, 7);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(75, 23);
        this.btnOK.TabIndex = 9;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += this.btnOK_Click;
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnCancel.Location = new System.Drawing.Point(327, 7);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 10;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += this.btnCancel_Click;
        // 
        // chkRelativePathing
        // 
        this.chkRelativePathing.AutoSize = true;
        this.chkRelativePathing.Location = new System.Drawing.Point(16, 76);
        this.chkRelativePathing.Name = "chkRelativePathing";
        this.chkRelativePathing.Size = new System.Drawing.Size(14, 13);
        this.chkRelativePathing.TabIndex = 4;
        this.ttHints.SetToolTip(this.chkRelativePathing, resources.GetString("chkRelativePathing.ToolTip"));
        this.chkRelativePathing.UseVisualStyleBackColor = true;
        // 
        // lbParent
        // 
        this.lbParent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.lbParent.Location = new System.Drawing.Point(70, 9);
        this.lbParent.Name = "lbParent";
        this.lbParent.Size = new System.Drawing.Size(332, 15);
        this.lbParent.TabIndex = 8;
        this.lbParent.Text = "Parent: none";
        // 
        // frmAddItem
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Window;
        this.ClientSize = new System.Drawing.Size(414, 145);
        this.Controls.Add(this.lbParent);
        this.Controls.Add(this.chkRelativePathing);
        this.Controls.Add(this.pnDialogButtons);
        this.Controls.Add(this.btnBrowse);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtPath);
        this.Controls.Add(this.lbMultiplayerHostnameIP);
        this.Controls.Add(this.txtName);
        this.Icon = Properties.Resources.ProgramIcon;
        this.MaximumSize = new System.Drawing.Size(999999, 180);
        this.MinimumSize = new System.Drawing.Size(430, 180);
        this.Name = "frmAddItem";
        this.Text = "Add file";
        this.pnDialogButtons.ResumeLayout(false);
        this.pnDialogButtons.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lbMultiplayerHostnameIP;
    private DOOMer.Controls.TextBoxEx txtName;
    private System.Windows.Forms.Label label1;
    private DOOMer.Controls.TextBoxEx txtPath;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Panel pnDialogButtons;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.CheckBox chkAddAsGroup;
    private System.Windows.Forms.CheckBox chkRelativePathing;
    private System.Windows.Forms.ToolTip ttHints;
    private System.Windows.Forms.CheckBox chkDirectoryAsFile;
    private System.Windows.Forms.Label lbParent;
}