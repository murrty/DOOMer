namespace DOOMer.WinForms;

using System;
using System.IO;
using System.Windows.Forms;
using DOOMer.Core;

public partial class frmArguments : Form {
    private readonly DoomArguments? arguments;
    public frmArguments(DoomArguments arguments) {
        this.InitializeComponent();
        this.arguments = arguments;
        this.Shown += (_,_) => btnOk.Focus();

        txtCommandLineArguments.Text = arguments.GetArgumentString();

        if (arguments.Port is null) {
            txtPortFileName.Text = "No port was defined.";
            txtPortWorkingDirectory.Text = string.Empty;
            lbLoadedPort.Text = "No port defined";
            chkFullArguments.Enabled = false;
            return;
        }

        txtPortFileName.Text = arguments.Port.File.Name;
        txtPortWorkingDirectory.Text = arguments.Port.WorkingDirectory;
        lbLoadedPort.Text = arguments.Port.Name;
        ttHints.SetToolTip(lbLoadedPort, arguments.Port.File.Exists ? arguments.Port.ToolTipText : ("Port file does not eixst, this is only for display.\r\n" + arguments.Port.ToolTipText));
    }
    private void chkFullArguments_CheckedChanged(object? sender, EventArgs e) {
        if (this.arguments?.Port is null) {
            return;
        }

        if (chkFullArguments.Checked) {
            txtCommandLineArguments.Text = $"\"{this.arguments.Port.File.FullName}\" {arguments.GetArgumentString()}";
            return;
        }

        txtCommandLineArguments.Text = arguments.GetArgumentString();
    }
    private void btnOk_Click(object sender, EventArgs e) {
        this.DialogResult = DialogResult.OK;
    }
}
