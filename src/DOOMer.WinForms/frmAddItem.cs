namespace DOOMer.WinForms;

using System;
using System.IO;
using System.Windows.Forms;
using DOOMer.Core;

public partial class frmAddItem : Form {
    public AddExternalType Type { get; }
    public InternalWad? FoundInternalWad { get; private set; }

    public string NewName { get; private set; }
    public string NewPath { get; private set; }
    public ExternalType? SelectedExternalType { get; private set; }

    public bool IsPathRelative { get; private set; }
    public bool LoadDirectoryAsFile { get; private set; }
    public bool AddAsGroup { get; private set; }

    public bool IsEditing { get; init; }

    public frmAddItem(string name, string path, AddExternalType externalType, bool isPathRelative, bool allowRelative) {
        this.InitializeComponent();
        txtName.KeyDown += this.Return_KeyDown;
        txtPath.KeyDown += this.Return_KeyDown;

        this.StartPosition = FormStartPosition.CenterParent;
        chkAddAsGroup.Enabled = chkAddAsGroup.Visible = chkAddAsGroup.Checked =
        chkDirectoryAsFile.Enabled = chkDirectoryAsFile.Visible = chkDirectoryAsFile.Checked = false;

        this.Type = externalType;
        this.IsPathRelative = chkRelativePathing.Checked = isPathRelative;
        this.NewName = txtName.Text = name.IsNullEmptyWhitespace() ? string.Empty : name.Trim();
        this.NewPath = txtPath.Text = path.IsNullEmptyWhitespace() ? string.Empty : path.Trim();

        switch (externalType) {
            case AddExternalType.IWad: {
                this.Text = "IWAD file...";
            } break;
            case AddExternalType.SourcePort: {
                this.Text = "Source port file...";
            } break;
            case AddExternalType.ExternalFile: {
                this.Text = "External file...";
            } break;
            case AddExternalType.ExternalDirectory: {
                chkDirectoryAsFile.Enabled = chkDirectoryAsFile.Visible = chkDirectoryAsFile.Checked = true;
                chkDirectoryAsFile.Location = chkAddAsGroup.Location;
                this.LoadDirectoryAsFile = chkDirectoryAsFile.Checked;
                this.Text = "External directory...";
            } break;
            case AddExternalType.ExternalGroup: {
                this.Text = "External group...";
            } break;
            case AddExternalType.ExternalGroupOrDirectory: {
                chkDirectoryAsFile.Enabled = chkDirectoryAsFile.Visible = chkDirectoryAsFile.Checked = true;
                chkAddAsGroup.Enabled = chkAddAsGroup.Visible = chkAddAsGroup.Checked = true;
                this.LoadDirectoryAsFile = chkDirectoryAsFile.Checked;
                this.Text = "External file group or directory...";
            } break;
        }

        if (!allowRelative) {
            chkRelativePathing.Enabled = chkRelativePathing.Checked = false;
        }
    }
    public frmAddItem(string name, string path, LoadedExternal? parent, AddExternalType externalType, bool isPathRelative, bool allowRelative) : this(name, path, externalType, isPathRelative, allowRelative) {
        if (parent is null) {
            return;
        }

        lbParent.Text = $"Parent: {parent.Name}";
    }

    private void btnBrowse_Click(object? sender, EventArgs e) {
        switch (this.Type) {
            case AddExternalType.IWad: {
                using var ofd = FindResourceFile.GetIWadDialog(multiSelect: false);
                if (!ofd.ShowDialog(DialogResult.OK)) {
                    return;
                }

                FindResourceFile.UpdateLastDirectory(ofd);
                FileInfo file = new(ofd.FileName);

                if (!file.Exists || file.Length < 5) {
                    return;
                }

                if (txtName.Text.IsNullEmptyWhitespace()) {
                    string? name = (this.FoundInternalWad = InternalWad.FindAnyInternalWad(file))?.GetFullString();
                    txtName.Text = name.IsNullEmptyWhitespace() ? Path.GetFileNameWithoutExtension(file.FullName) : name;
                }

                txtPath.Text = file.FullName;
            } break;

            case AddExternalType.SourcePort: {
                using var ofd = FindResourceFile.GetPortDialog(multiSelect: false);
                if (!ofd.ShowDialog(DialogResult.OK)) {
                    return;
                }

                FindResourceFile.UpdateLastDirectory(ofd);
                FileInfo file = new(ofd.FileName);

                if (!file.Exists || file.Length < 5) {
                    return;
                }

                if (txtName.Text.IsNullEmptyWhitespace()) {
                    txtName.Text = Path.GetFileNameWithoutExtension(file.FullName);
                }

                txtPath.Text = file.FullName;
            } break;

            case AddExternalType.ExternalFile: {
                using var ofd = FindResourceFile.GetExternalFileDialog(multiSelect: false);
                if (!ofd.ShowDialog(DialogResult.OK)) {
                    return;
                }

                FindResourceFile.UpdateLastDirectory(ofd);
                FileInfo file = new(ofd.FileName);

                if (!file.Exists || file.Length < 5) {
                    return;
                }

                if (txtName.Text.IsNullEmptyWhitespace()) {
                    string? name = (this.FoundInternalWad = InternalWad.FindAnyInternalWad(file))?.GetFullString();
                    txtName.Text = name.IsNullEmptyWhitespace() ? Path.GetFileNameWithoutExtension(file.FullName) : name;
                }

                txtPath.Text = file.FullName;
            } break;

            case AddExternalType.ExternalDirectory: {
                using var fbd = FindResourceFile.GetExternalDirectoryDialog(AddExternalType.ExternalDirectory, false);
                if (!fbd.ShowDialog(DialogResult.OK)) {
                    return;
                }

                FindResourceFile.UpdateLastDirectory(fbd);
                DirectoryInfo directory = new(fbd.SelectedPath);

                if (!directory.Exists) {
                    return;
                }

                if (txtName.Text.IsNullEmptyWhitespace()) {
                    txtName.Text = Path.GetFileNameWithoutExtension(directory.FullName);
                }

                txtPath.Text = directory.FullName;
            } break;

            case AddExternalType.ExternalGroupOrDirectory: {
                using var fbd = FindResourceFile.GetExternalDirectoryDialog(AddExternalType.ExternalGroupOrDirectory, false);
                if (!fbd.ShowDialog(DialogResult.OK)) {
                    return;
                }

                FindResourceFile.UpdateLastDirectory(fbd);
                DirectoryInfo directory = new(fbd.SelectedPath);

                if (!directory.Exists) {
                    return;
                }

                if (txtName.Text.IsNullEmptyWhitespace()) {
                    txtName.Text = Path.GetFileNameWithoutExtension(directory.FullName);
                }

                txtPath.Text = directory.FullName;
            } break;
        }
    }
    private void btnOK_Click(object? sender, EventArgs e) {
        if (txtName.Text.IsNullEmptyWhitespace()) {
            txtName.Focus();
            return;
        }

        this.SelectedExternalType = null;
        string newName = txtName.Text.Trim();
        string newPath;

        switch (this.Type) {
            case AddExternalType.IWad:
            case AddExternalType.SourcePort:
            case AddExternalType.ExternalFile:
            case AddExternalType.ExternalDirectory:
                if (txtPath.Text.IsNullEmptyWhitespace()) {
                    txtPath.Focus();
                    return;
                }

                if (this.IsEditing && newName.Equals(this.NewName, StringComparison.OrdinalIgnoreCase) && txtPath.Text.Equals(this.NewPath, StringComparison.OrdinalIgnoreCase)) {
                    if (this.IsPathRelative && chkRelativePathing.Checked && (this.Type != AddExternalType.ExternalDirectory || this.LoadDirectoryAsFile == chkDirectoryAsFile.Checked)) {
                        goto default;
                    }

                    this.IsPathRelative = chkRelativePathing.Checked;
                    this.LoadDirectoryAsFile = chkDirectoryAsFile.Checked;
                    this.DialogResult = DialogResult.OK;
                    break;
                }

                if (!File.Exists(newPath = txtPath.Text.Trim())) {
                    txtPath.Focus();
                    return;
                }

                this.NewName = newName;
                this.NewPath = newPath;
                this.IsPathRelative = newPath.IsPathRelativeCompatible() && chkRelativePathing.Checked;
                this.LoadDirectoryAsFile = chkDirectoryAsFile.Checked;
                this.DialogResult = DialogResult.OK;
                break;

            case AddExternalType.ExternalGroup:
                if (this.IsEditing && newName.Equals(this.NewName, StringComparison.OrdinalIgnoreCase)) {
                    goto default;
                }

                this.NewName = newName;
                this.DialogResult = DialogResult.OK;
                break;

            case AddExternalType.ExternalGroupOrDirectory:
                // ExternalGroupOrDirectory always has 'IsEditing' set to false.

                if (txtPath.Text.IsNullEmptyWhitespace()) {
                    txtPath.Focus();
                    return;
                }

                if (!Directory.Exists(newPath = txtPath.Text.Trim())) {
                    txtPath.Focus();
                    return;
                }

                this.NewName = newName;
                this.NewPath = newPath;
                this.SelectedExternalType = chkAddAsGroup.Checked ? ExternalType.Group : ExternalType.Directory;
                this.IsPathRelative = newPath.IsPathRelativeCompatible() && chkRelativePathing.Checked;
                this.LoadDirectoryAsFile = chkDirectoryAsFile.Checked;
                this.AddAsGroup = chkAddAsGroup.Checked;
                this.DialogResult = DialogResult.OK;
                break;

            default:
                this.DialogResult = DialogResult.Cancel;
                break;
        }
    }
    private void btnCancel_Click(object? sender, EventArgs e) {
        this.DialogResult = DialogResult.Cancel;
    }
    private void Return_KeyDown(object? sender, KeyEventArgs e) {
        if (e.KeyCode != Keys.Return) {
            return;
        }

        this.btnOK_Click(sender, e);
    }
}
