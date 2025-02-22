namespace DOOMer.WinForms;

partial class frmUnknownPortLaunch {
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
        this.pnDialogButtons = new System.Windows.Forms.Panel();
        this.chkSavePortForLater = new System.Windows.Forms.CheckBox();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnLaunch = new System.Windows.Forms.Button();
        this.lbHeader = new System.Windows.Forms.Label();
        this.lbDescription = new System.Windows.Forms.Label();
        this.pnInfo = new Controls.PanelEx();
        this.lbArguments = new System.Windows.Forms.Label();
        this.txtArguments = new System.Windows.Forms.TextBox();
        this.lbFilePath = new System.Windows.Forms.Label();
        this.txtPath = new Controls.TextBoxEx();
        this.lbName = new System.Windows.Forms.Label();
        this.txtName = new Controls.TextBoxEx();
        this.pnDialogButtons.SuspendLayout();
        this.pnInfo.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnDialogButtons
        // 
        this.pnDialogButtons.BackColor = System.Drawing.SystemColors.Menu;
        this.pnDialogButtons.Controls.Add(this.chkSavePortForLater);
        this.pnDialogButtons.Controls.Add(this.btnCancel);
        this.pnDialogButtons.Controls.Add(this.btnLaunch);
        this.pnDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnDialogButtons.Location = new System.Drawing.Point(0, 325);
        this.pnDialogButtons.Name = "pnDialogButtons";
        this.pnDialogButtons.Size = new System.Drawing.Size(384, 40);
        this.pnDialogButtons.TabIndex = 3;
        // 
        // chkSavePortForLater
        // 
        this.chkSavePortForLater.AutoSize = true;
        this.chkSavePortForLater.Location = new System.Drawing.Point(12, 10);
        this.chkSavePortForLater.Name = "chkSavePortForLater";
        this.chkSavePortForLater.Size = new System.Drawing.Size(127, 19);
        this.chkSavePortForLater.TabIndex = 7;
        this.chkSavePortForLater.Text = "Save as known port";
        this.chkSavePortForLater.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnCancel.Location = new System.Drawing.Point(297, 7);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 6;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += this.btnCancel_Click;
        // 
        // btnLaunch
        // 
        this.btnLaunch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnLaunch.Enabled = false;
        this.btnLaunch.Location = new System.Drawing.Point(200, 7);
        this.btnLaunch.Name = "btnLaunch";
        this.btnLaunch.Size = new System.Drawing.Size(91, 23);
        this.btnLaunch.TabIndex = 5;
        this.btnLaunch.Text = "( 5 )";
        this.btnLaunch.UseVisualStyleBackColor = true;
        this.btnLaunch.Click += this.btnLaunch_Click;
        // 
        // lbHeader
        // 
        this.lbHeader.AutoSize = true;
        this.lbHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.lbHeader.Location = new System.Drawing.Point(12, 4);
        this.lbHeader.Name = "lbHeader";
        this.lbHeader.Size = new System.Drawing.Size(316, 21);
        this.lbHeader.TabIndex = 4;
        this.lbHeader.Text = "An external known source port was detected";
        // 
        // lbDescription
        // 
        this.lbDescription.AutoSize = true;
        this.lbDescription.Location = new System.Drawing.Point(12, 33);
        this.lbDescription.Name = "lbDescription";
        this.lbDescription.Size = new System.Drawing.Size(259, 30);
        this.lbDescription.TabIndex = 5;
        this.lbDescription.Text = "For safety, the file will not automatically launch.\r\nYou may launch it in 5 seconds";
        this.lbDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // pnInfo
        // 
        this.pnInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.pnInfo.BackColor = System.Drawing.SystemColors.Menu;
        this.pnInfo.Controls.Add(this.lbArguments);
        this.pnInfo.Controls.Add(this.txtArguments);
        this.pnInfo.Controls.Add(this.lbFilePath);
        this.pnInfo.Controls.Add(this.txtPath);
        this.pnInfo.Controls.Add(this.lbName);
        this.pnInfo.Controls.Add(this.txtName);
        this.pnInfo.Location = new System.Drawing.Point(14, 78);
        this.pnInfo.Name = "pnInfo";
        this.pnInfo.Size = new System.Drawing.Size(356, 226);
        this.pnInfo.TabIndex = 6;
        // 
        // lbArguments
        // 
        this.lbArguments.AutoSize = true;
        this.lbArguments.Location = new System.Drawing.Point(3, 77);
        this.lbArguments.Name = "lbArguments";
        this.lbArguments.Size = new System.Drawing.Size(66, 15);
        this.lbArguments.TabIndex = 11;
        this.lbArguments.Text = "Arguments";
        // 
        // txtArguments
        // 
        this.txtArguments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtArguments.Location = new System.Drawing.Point(3, 94);
        this.txtArguments.Multiline = true;
        this.txtArguments.Name = "txtArguments";
        this.txtArguments.Size = new System.Drawing.Size(350, 129);
        this.txtArguments.TabIndex = 10;
        // 
        // lbFilePath
        // 
        this.lbFilePath.AutoSize = true;
        this.lbFilePath.Location = new System.Drawing.Point(3, 40);
        this.lbFilePath.Name = "lbFilePath";
        this.lbFilePath.Size = new System.Drawing.Size(52, 15);
        this.lbFilePath.TabIndex = 8;
        this.lbFilePath.Text = "File path";
        // 
        // txtPath
        // 
        this.txtPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtPath.Location = new System.Drawing.Point(3, 56);
        this.txtPath.Name = "txtPath";
        this.txtPath.Size = new System.Drawing.Size(350, 23);
        this.txtPath.TabIndex = 9;
        // 
        // lbName
        // 
        this.lbName.AutoSize = true;
        this.lbName.Location = new System.Drawing.Point(3, 3);
        this.lbName.Name = "lbName";
        this.lbName.Size = new System.Drawing.Size(39, 15);
        this.lbName.TabIndex = 6;
        this.lbName.Text = "Name";
        // 
        // txtName
        // 
        this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtName.Location = new System.Drawing.Point(3, 19);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(350, 23);
        this.txtName.TabIndex = 7;
        // 
        // frmUnknownPortLaunch
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 365);
        this.Controls.Add(this.pnInfo);
        this.Controls.Add(this.lbDescription);
        this.Controls.Add(this.lbHeader);
        this.Controls.Add(this.pnDialogButtons);
        this.MinimumSize = new System.Drawing.Size(400, 400);
        this.Name = "frmUnknownPortLaunch";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Non-known DOOM source port launch";
        this.pnDialogButtons.ResumeLayout(false);
        this.pnDialogButtons.PerformLayout();
        this.pnInfo.ResumeLayout(false);
        this.pnInfo.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel pnDialogButtons;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnLaunch;
    private System.Windows.Forms.Label lbHeader;
    private System.Windows.Forms.Label lbDescription;
    private DOOMer.Controls.PanelEx pnInfo;
    private System.Windows.Forms.Label lbFilePath;
    private Controls.TextBoxEx txtPath;
    private System.Windows.Forms.Label lbName;
    private Controls.TextBoxEx txtName;
    private System.Windows.Forms.Label lbArguments;
    private System.Windows.Forms.TextBox txtArguments;
    private System.Windows.Forms.CheckBox chkSavePortForLater;
}