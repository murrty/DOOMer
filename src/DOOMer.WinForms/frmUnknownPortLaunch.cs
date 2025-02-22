namespace DOOMer.WinForms;

using System;
using System.IO;
using System.Windows.Forms;
using DOOMer.Core;

public partial class frmUnknownPortLaunch : Form {
    private readonly Timer countdown = new() { Interval = 1_000, };
    int count = 5;
    public bool SavePort => chkSavePortForLater.Enabled && chkSavePortForLater.Checked;
    public frmUnknownPortLaunch(string filePath, InternalWad port, DoomArguments args, bool allowSavePort) {
        this.InitializeComponent();
        this.countdown.Tick += this.CountdownTick;
        this.Shown += this.FormShown;
        chkSavePortForLater.Enabled = allowSavePort;
        txtName.Text = $"{port.Name} (file name: '{Path.GetFileNameWithoutExtension(filePath)}')";
        txtPath.Text = filePath;
        txtArguments.Text = args.GetArgumentString();
    }
    private void CountdownTick(object? sender, EventArgs e) {
        if (--count == 0) {
            countdown.Stop();
            btnLaunch.Enabled = true;
            btnLaunch.Text = "Launch";
            lbDescription.Text = "For safety, the file will not automatically launch.\r\nYou can launch the program.";
            return;
        }

        lbDescription.Text = $"For safety, the file will not automatically launch.\r\nEnabling launch in {count} seconds";
        btnLaunch.Text = $"( {count} )";
    }
    private void FormShown(object? sender, EventArgs e) {
        countdown.Start();
    }
    private void btnLaunch_Click(object? sender, EventArgs e) {
        this.DialogResult = DialogResult.OK;
    }
    private void btnCancel_Click(object? sender, EventArgs e) {
        this.DialogResult = DialogResult.Cancel;
    }
}
