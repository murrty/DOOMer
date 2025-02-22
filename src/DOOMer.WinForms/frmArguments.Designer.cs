namespace DOOMer.WinForms;

partial class frmArguments {
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
        this.chkFullArguments = new System.Windows.Forms.CheckBox();
        this.btnOk = new System.Windows.Forms.Button();
        this.pnArgumentDisplay = new System.Windows.Forms.Panel();
        this.lbLoadedPort = new System.Windows.Forms.Label();
        this.lbCommandLineArguments = new System.Windows.Forms.Label();
        this.txtCommandLineArguments = new System.Windows.Forms.TextBox();
        this.lbPortWorkingDirectory = new System.Windows.Forms.Label();
        this.txtPortWorkingDirectory = new System.Windows.Forms.TextBox();
        this.lbPortFileName = new System.Windows.Forms.Label();
        this.txtPortFileName = new System.Windows.Forms.TextBox();
        this.ttHints = new System.Windows.Forms.ToolTip(this.components);
        this.pnDialogButtons.SuspendLayout();
        this.pnArgumentDisplay.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnDialogButtons
        // 
        this.pnDialogButtons.BackColor = System.Drawing.SystemColors.Menu;
        this.pnDialogButtons.Controls.Add(this.chkFullArguments);
        this.pnDialogButtons.Controls.Add(this.btnOk);
        this.pnDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnDialogButtons.Location = new System.Drawing.Point(0, 195);
        this.pnDialogButtons.Name = "pnDialogButtons";
        this.pnDialogButtons.Size = new System.Drawing.Size(364, 40);
        this.pnDialogButtons.TabIndex = 2;
        // 
        // chkFullArguments
        // 
        this.chkFullArguments.AutoSize = true;
        this.chkFullArguments.Location = new System.Drawing.Point(12, 11);
        this.chkFullArguments.Name = "chkFullArguments";
        this.chkFullArguments.Size = new System.Drawing.Size(134, 19);
        this.chkFullArguments.TabIndex = 2;
        this.chkFullArguments.Text = "Show full arguments";
        this.chkFullArguments.UseVisualStyleBackColor = true;
        this.chkFullArguments.CheckedChanged += this.chkFullArguments_CheckedChanged;
        // 
        // btnOk
        // 
        this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnOk.Location = new System.Drawing.Point(277, 7);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 1;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += this.btnOk_Click;
        // 
        // pnArgumentDisplay
        // 
        this.pnArgumentDisplay.BackColor = System.Drawing.SystemColors.Window;
        this.pnArgumentDisplay.Controls.Add(this.lbLoadedPort);
        this.pnArgumentDisplay.Controls.Add(this.lbCommandLineArguments);
        this.pnArgumentDisplay.Controls.Add(this.txtCommandLineArguments);
        this.pnArgumentDisplay.Controls.Add(this.lbPortWorkingDirectory);
        this.pnArgumentDisplay.Controls.Add(this.txtPortWorkingDirectory);
        this.pnArgumentDisplay.Controls.Add(this.lbPortFileName);
        this.pnArgumentDisplay.Controls.Add(this.txtPortFileName);
        this.pnArgumentDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnArgumentDisplay.Location = new System.Drawing.Point(0, 0);
        this.pnArgumentDisplay.MinimumSize = new System.Drawing.Size(0, 123);
        this.pnArgumentDisplay.Name = "pnArgumentDisplay";
        this.pnArgumentDisplay.Size = new System.Drawing.Size(364, 195);
        this.pnArgumentDisplay.TabIndex = 1;
        // 
        // lbLoadedPort
        // 
        this.lbLoadedPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.lbLoadedPort.AutoEllipsis = true;
        this.lbLoadedPort.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
        this.lbLoadedPort.ForeColor = System.Drawing.SystemColors.ControlLightLight;
        this.lbLoadedPort.Location = new System.Drawing.Point(59, 3);
        this.lbLoadedPort.Name = "lbLoadedPort";
        this.lbLoadedPort.Size = new System.Drawing.Size(302, 15);
        this.lbLoadedPort.TabIndex = 7;
        this.lbLoadedPort.Text = "none";
        // 
        // lbCommandLineArguments
        // 
        this.lbCommandLineArguments.AutoSize = true;
        this.lbCommandLineArguments.Location = new System.Drawing.Point(3, 79);
        this.lbCommandLineArguments.Name = "lbCommandLineArguments";
        this.lbCommandLineArguments.Size = new System.Drawing.Size(146, 15);
        this.lbCommandLineArguments.TabIndex = 5;
        this.lbCommandLineArguments.Text = "Command line arguments";
        // 
        // txtCommandLineArguments
        // 
        this.txtCommandLineArguments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtCommandLineArguments.Location = new System.Drawing.Point(3, 95);
        this.txtCommandLineArguments.Multiline = true;
        this.txtCommandLineArguments.Name = "txtCommandLineArguments";
        this.txtCommandLineArguments.ReadOnly = true;
        this.txtCommandLineArguments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtCommandLineArguments.Size = new System.Drawing.Size(358, 94);
        this.txtCommandLineArguments.TabIndex = 6;
        // 
        // lbPortWorkingDirectory
        // 
        this.lbPortWorkingDirectory.AutoSize = true;
        this.lbPortWorkingDirectory.Location = new System.Drawing.Point(3, 41);
        this.lbPortWorkingDirectory.Name = "lbPortWorkingDirectory";
        this.lbPortWorkingDirectory.Size = new System.Drawing.Size(102, 15);
        this.lbPortWorkingDirectory.TabIndex = 3;
        this.lbPortWorkingDirectory.Text = "Working directory";
        // 
        // txtPortWorkingDirectory
        // 
        this.txtPortWorkingDirectory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtPortWorkingDirectory.Location = new System.Drawing.Point(3, 57);
        this.txtPortWorkingDirectory.Name = "txtPortWorkingDirectory";
        this.txtPortWorkingDirectory.ReadOnly = true;
        this.txtPortWorkingDirectory.Size = new System.Drawing.Size(358, 23);
        this.txtPortWorkingDirectory.TabIndex = 4;
        // 
        // lbPortFileName
        // 
        this.lbPortFileName.AutoSize = true;
        this.lbPortFileName.Location = new System.Drawing.Point(3, 3);
        this.lbPortFileName.Name = "lbPortFileName";
        this.lbPortFileName.Size = new System.Drawing.Size(58, 15);
        this.lbPortFileName.TabIndex = 1;
        this.lbPortFileName.Text = "File name";
        // 
        // txtPortFileName
        // 
        this.txtPortFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.txtPortFileName.Location = new System.Drawing.Point(3, 19);
        this.txtPortFileName.Name = "txtPortFileName";
        this.txtPortFileName.ReadOnly = true;
        this.txtPortFileName.Size = new System.Drawing.Size(358, 23);
        this.txtPortFileName.TabIndex = 2;
        // 
        // frmArguments
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Window;
        this.ClientSize = new System.Drawing.Size(364, 235);
        this.Controls.Add(this.pnArgumentDisplay);
        this.Controls.Add(this.pnDialogButtons);
        this.Icon = Properties.Resources.ProgramIcon;
        this.MinimumSize = new System.Drawing.Size(380, 270);
        this.Name = "frmArguments";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Argumnts";
        this.pnDialogButtons.ResumeLayout(false);
        this.pnDialogButtons.PerformLayout();
        this.pnArgumentDisplay.ResumeLayout(false);
        this.pnArgumentDisplay.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnDialogButtons;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Panel pnArgumentDisplay;
    private System.Windows.Forms.Label lbPortFileName;
    private System.Windows.Forms.TextBox txtPortFileName;
    private System.Windows.Forms.Label lbCommandLineArguments;
    private System.Windows.Forms.TextBox txtCommandLineArguments;
    private System.Windows.Forms.Label lbPortWorkingDirectory;
    private System.Windows.Forms.TextBox txtPortWorkingDirectory;
    private System.Windows.Forms.Label lbLoadedPort;
    private System.Windows.Forms.ToolTip ttHints;
    private System.Windows.Forms.CheckBox chkFullArguments;
}