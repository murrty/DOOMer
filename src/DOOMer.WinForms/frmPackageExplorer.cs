namespace DOOMer.WinForms;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using DOOMer.Core;
using HashSlingingSlasher;
using pdj.tiny7z.Archive;

public partial class frmPackageExplorer : Form {
    private FileInfo File { get; set; }
    private Stream FileStream { get; set; }
    private ZipArchive? PK3Archive { get; set; }
    private SevenZipArchive? PK7Archive { get; set; }

    private sealed class Entry {
        public string Name { get; set; }
        public WadEntry? WadEntry { get; set; }
        public ZipArchiveEntry? PK3Entry { get; set; }
        public SevenZipArchiveFile? PK7Entry { get; set; }

        public Stream FileStream { get; }

        Entry(string name, Stream fileStream) {
            this.Name = name;
            this.FileStream = fileStream;
        }
        public Entry(string name, Stream fileStream, WadEntry wad) : this(name: name, fileStream: fileStream) {
            this.Name = name;
            this.WadEntry = wad;
        }
        public Entry(string name, Stream fileStream, ZipArchiveEntry pk3Entry) : this(name: name, fileStream: fileStream) {
            this.Name = name;
            this.PK3Entry = pk3Entry;
        }
        public Entry(string name, Stream fileStream, SevenZipArchiveFile pk7Entry) : this(name: name, fileStream: fileStream) {
            this.Name = name;
            this.PK7Entry = pk7Entry;
        }

        public void ExtractWadToDirectory(string directoryPath) {
            this.ExtractWad(directoryPath: directoryPath, fullPath: Path.Combine(directoryPath, this.Name));
        }
        public void ExtractWadToFile(string filePath) {
            string? directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath.IsNullEmptyWhitespace()) {
                return;
            }
            this.ExtractWad(directoryPath: directoryPath, fullPath: filePath);
        }

        public void ExtractPK3ToDirectory(string directoryPath) {
            this.ExtractPK3(directoryPath: directoryPath, fullPath: Path.Combine(directoryPath, this.Name));
        }
        public void ExtractPK3ToFile(string filePath) {
            string? directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath.IsNullEmptyWhitespace()) {
                return;
            }
            this.ExtractPK3(directoryPath: directoryPath, fullPath: filePath);
        }

        public void ExtractPK7ToDirectory(IExtractor extractor, string directoryPath) {
            this.ExtractPK7(extractor: extractor, directoryPath: directoryPath, fullPath: Path.Combine(directoryPath, this.Name));
        }
        public void ExtractPK7ToFile(IExtractor extractor, string filePath) {
            string? directoryPath = Path.GetDirectoryName(filePath);
            if (directoryPath.IsNullEmptyWhitespace()) {
                return;
            }
            this.ExtractPK7(extractor: extractor, directoryPath: directoryPath, fullPath: filePath);
        }

        public void ExtractWad(string directoryPath, string fullPath) {
            if (!Directory.CreateDirectory(directoryPath).Exists) {
                return;
            }

            using var fileStream = System.IO.File.Create(fullPath);
            if (this.WadEntry?.Length is not > 0) {
                return;
            }

            this.FileStream.Position = this.WadEntry.Offset;
            byte[] bytes = new byte[this.WadEntry.Length];
            this.FileStream.Read(bytes);
            fileStream.Write(bytes);
        }
        public void ExtractPK3(string directoryPath, string fullPath) {
            if (!Directory.CreateDirectory(directoryPath).Exists) {
                return;
            }

            if (this.PK3Entry?.Length > 0) {
                this.PK3Entry.ExtractToFile(fullPath, true);
                return;
            }

            System.IO.File.Create(fullPath).Dispose();
        }
        public void ExtractPK7(IExtractor extractor, string directoryPath, string fullPath) {
            if (!Directory.CreateDirectory(directoryPath).Exists) {
                return;
            }

            if (this.PK7Entry?.Size > 0 && this.PK7Entry.UnPackIndex.HasValue) {
                var fileStream = System.IO.File.Create(fullPath);
                extractor.ExtractFile(this.PK7Entry.UnPackIndex.Value, fileStream);
                return;
            }

            System.IO.File.Create(fullPath).Dispose();
        }
    }

    #region Menus
    readonly ContextMenu cmEntries = new() { Name = nameof(cmEntries), };
    readonly MenuItem mExtractSelectedEntries = new("Extract selected entries...") { Name = nameof(mExtractSelectedEntries), };

    private void InitializeMenus() {
        mExtractSelectedEntries.Click += this.btnExtractSelectedEntries_Click;
        cmEntries.MenuItems.Add(mExtractSelectedEntries);
        lvEntries.MouseUp += (_, e) => {
            if (e.Button == MouseButtons.Right) {
                cmEntries.Show(lvEntries, new System.Drawing.Point(e.X, e.Y));
            }
        };
    }
    #endregion Menus

    public frmPackageExplorer(FileInfo file) {
        this.InitializeComponent();
        this.InitializeMenus();

        this.FormClosing += this.frmPackageExplorer_FormClosing;
        this.Shown += (_, _) => txtFile.Focus();

        chkFullRowSelect.Checked = false;
        lvEntries.FullRowSelect = false;

        this.UpdatePackage(file);
    }

    [MemberNotNull(nameof(File))]
    [MemberNotNull(nameof(FileStream))]
    private void UpdatePackage(FileInfo file) {
        this.FileStream?.Dispose();
        this.PK3Archive?.Dispose();
        this.PK7Archive?.Dispose();
        this.File ??= file;

        if (!file.Exists || file.Length < 6) {
            this.FileStream ??= Stream.Null;
            return;
        }

        txtFile.Text = file.FullName;
        lvEntries.Items.Clear();

        this.FileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        Span<byte> header = stackalloc byte[6];
        this.FileStream.Read(header);
        this.FileStream.Position = 0;
        WadType wadType = InternalWad.GetWadType(header, file);
        long totalEntriesSize = 0;

        try {
            switch (wadType) {
                case WadType.IWAD:
                case WadType.PWAD:
                case WadType.IWADIncorrectlyMarked:
                case WadType.PWADIncorrectlyMarked:
                    lvEntries.Enabled = true;
                    btnExtractAll.Enabled = true;
                    mExtractSelectedEntries.Enabled = true;
                    btnExtractSelectedEntries.Enabled = true;
                    lvEntries.Columns[2].Text = "Offset";

                    byte[] bytes = new byte[this.FileStream.Length];
                    this.FileStream.Read(bytes, 0, bytes.Length);

                    var wads = InternalWad.EnumerateWadFileEntries(bytes: bytes, file: file, scanEntriesCrc32: true, wadType: wadType);
                    if (wads?.Length is not > 0) {
                        break;
                    }

                    for (int i = 0, noNameCount = 0; i < wads.Length; i++) {
                        WadEntry wad = wads[i];
                        Entry entry = new(name: wad.Name.IsNullEmptyWhitespace() ? $"NO_NAME{noNameCount++:000.#}" : wad.Name.Trim(),
                            fileStream: this.FileStream,
                            wad: wad);

                        totalEntriesSize += wad.Length;
                        ListViewItem lvi = new(wad.Name) {
                            Tag = entry,
                            ToolTipText = $$"""
                                {{wad.Name}}
                                Offset: {{wad.Offset}}
                                Length: {{wad.Length}}
                                """,
                        };
                        lvi.SubItems.Add(wad.Length.ToString("N0"));
                        lvi.SubItems.Add("0x" + wad.Offset.ToString("X"));
                        lvi.SubItems.Add("0x" + wad.CRC32.ToString("X"));
                        lvEntries.Items.Add(lvi);
                    }
                    break;

                case WadType.PK3:
                case WadType.IPK3:
                case WadType.PK3IncorrectlyMarked:
                case WadType.IPK3IncorrectlyMarked:
                    lvEntries.Enabled = true;
                    btnExtractAll.Enabled = true;
                    mExtractSelectedEntries.Enabled = true;
                    btnExtractSelectedEntries.Enabled = true;
                    lvEntries.Columns[2].Text = "Attributes";

                    var pk3archive = new ZipArchive(this.FileStream);
                    if (pk3archive?.Entries.Count is not > 0) {
                        pk3archive?.Dispose();
                        break;
                    }

                    this.PK3Archive = pk3archive;

                    for (int i = 0, noNameCount = 0; i < pk3archive.Entries.Count; i++) {
                        ZipArchiveEntry zipEntry = pk3archive.Entries[i];

                        Entry entry = new(name: zipEntry.Name.IsNullEmptyWhitespace() ? $"NO_NAME{noNameCount++:000.#}" : zipEntry.Name,
                            fileStream: this.FileStream,
                            pk3Entry: zipEntry);

                        totalEntriesSize += zipEntry.Length;
                        ListViewItem lvi = new(entry.Name) {
                            Tag = entry,
                            ToolTipText = $$"""
                                {{entry.Name}}
                                Full path: {{zipEntry.FullName}}
                                Length: {{zipEntry.Length}}
                                """,
                        };
                        lvi.SubItems.Add(zipEntry.Length.ToString("N0"));
                        lvi.SubItems.Add("0x" + zipEntry.ExternalAttributes.ToString("X"));
                        lvi.SubItems.Add("0x" + zipEntry.Crc32.ToString("X"));
                        lvEntries.Items.Add(lvi);
                    }
                    break;

                case WadType.PK7:
                case WadType.IPK7:
                case WadType.PK7IncorrectlyMarked:
                case WadType.IPK7IncorrectlyMarked:
                    lvEntries.Enabled = true;
                    btnExtractAll.Enabled = true;
                    mExtractSelectedEntries.Enabled = true;
                    btnExtractSelectedEntries.Enabled = true;
                    lvEntries.Columns[2].Text = "Attributes";

                    var pk7archive = new SevenZipArchive(this.FileStream, FileAccess.Read);
                    if (!pk7archive.IsValid) {
                        break;
                    }

                    this.PK7Archive = pk7archive;
                    var extractor = pk7archive.Extractor();

                    if (extractor.Files?.Count is not > 0) {
                        this.PK7Archive = null;
                        break;
                    }

                    for (int i = 0, noNameCount = 0; i < extractor.Files.Count; i++) {
                        if (extractor.Files[i] is not SevenZipArchiveFile zipEntry || zipEntry.IsDirectory) {
                            continue;
                        }

                        long size = (long)(zipEntry.Size ?? 0ul);

                        Entry entry = new(name: zipEntry.EntryName.IsNullEmptyWhitespace() ? $"NO_NAME{noNameCount++:000.#}" : zipEntry.EntryName,
                            fileStream: this.FileStream,
                            pk7Entry: zipEntry);

                        ListViewItem lvi = new(zipEntry.EntryName) {
                            Tag = entry,
                            ToolTipText = $$"""
                                File '{{zipEntry.EntryName}}'
                                Full path: {{zipEntry.Name}}
                                Length: {{size}}
                                """,
                        };
                        lvi.SubItems.Add(size.ToString("N0"));
                        lvi.SubItems.Add("0x" + (zipEntry.Attributes ?? 0).ToString("X"));
                        lvi.SubItems.Add("0x" + (zipEntry.CRC ?? 0).ToString("X"));
                        lvEntries.Items.Add(lvi);
                    }
                    break;

                default:
                    lbInternalWad.Text = "No internal wad found";
                    lvEntries.Enabled = true;
                    btnExtractAll.Enabled = true;
                    mExtractSelectedEntries.Enabled = true;
                    btnExtractSelectedEntries.Enabled = true;
                    wadType = WadType.Unknown;
                    break;
            }

            Hash md5 = default, sha1 = default, crc32 = default;
            Task.WaitAll(
                Task.Run(() => md5 = Hash.Calculate(file, Hash.CalculateMD5)),
                Task.Run(() => sha1 = Hash.Calculate(file, Hash.CalculateSHA1)),
                Task.Run(() => crc32 = Hash.Calculate(file, Hash.CalculateCRC32)));

            InternalWad? internalWad;
            if (wadType != WadType.Unknown && (internalWad = InternalWad.FindAnyInternalWad(md5, sha1, crc32, file.Length)) is not null && internalWad.WadType != WadType.Unknown) {
                lbInternalWad.Text = internalWad.GetFullString();
                string informationText = internalWad.GenerateWadComparisonString(md5, sha1, crc32, file.Length, lvEntries.Items.Count > 0 ? lvEntries.Items.Count : null);
                ttHints.SetToolTip(lbInternalWad, informationText);
                txtFileInformation.Text = informationText;
            }
            else {
                txtFileInformation.Text = $$"""
                    {{file.Name}}
                    {{file.FullName}}
                    MD5 [{{md5}}]
                    SHA1 [{{sha1}}]
                    CRC32 [{{crc32}}]
                    Size [{{file.Length}}]
                    Entries [{{lvEntries.Items.Count}}]
                    """;
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.ToString(), "DOOMer");
        }

        tpEntries.Text = $"Entries ({lvEntries.Items.Count:N0})";
        this.ResizeColumn();
    }
    private void SelectNewPackage() {
        using var ofd = FindResourceFile.GetExternalFileDialog(multiSelect: false);
        if (!ofd.ShowDialog(DialogResult.OK)) {
            return;
        }

        FindResourceFile.UpdateLastDirectory(ofd);
        FileInfo file = new(ofd.FileName);
        if (!file.Exists || file.Length < 5) {
            return;
        }

        this.UpdatePackage(file);
    }
    private void DisposeStreams() {
        this.PK3Archive?.Dispose();
        this.PK7Archive?.Dispose();
        this.FileStream?.Dispose();
    }
    private void ResizeColumn() {
        //lvEntries.Columns[0].Width = lvEntries.Width - lvEntries.Margin.Left - lvEntries.Margin.Right - 16;
        lvEntries.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
    }

    private void frmPackageExplorer_FormClosing(object? sender, FormClosingEventArgs e) {
        this.DisposeStreams();
    }

    private void btnBrowse_Click(object sender, EventArgs e) {
        this.SelectNewPackage();
    }
    private void btnOk_Click(object? sender, EventArgs e) {
        this.DialogResult = DialogResult.OK;
    }

    static string? GetSaveEntryFilePath(string entryName) {
        using SaveFileDialog sfd = new() {
            FileName = entryName,
            Filter = "All files (*.*)|*.*",
            Title = "Save the entry as...",
        };

        return !sfd.ShowDialog(DialogResult.OK) || sfd.FileName.IsNullEmptyWhitespace() ?
            null : sfd.FileName;
    }
    static string? GetSaveEntryDirectoryPath(FileInfo file) {
        using FolderBrowserDialog fbd = new() {
            Description = "Select a folder to extract entries to...",
            UseDescriptionForTitle = true,
            InitialDirectory = Path.GetDirectoryName(file.FullName) ?? "",
        };

        return fbd.ShowDialog() != DialogResult.OK || fbd.SelectedPath.IsNullEmptyWhitespace() ?
            null : Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(file.FullName));
    }

    private void btnExtractSelectedEntries_Click(object? sender, EventArgs e) {
        if (lvEntries.SelectedItems.Count < 1) {
            return;
        }

        string? outputPath;
        if (lvEntries.SelectedItems.Count == 1) {
            if (lvEntries.SelectedItems[0].Tag is not Entry entry) {
                return;
            }

            outputPath = GetSaveEntryFilePath(entry.Name);
            if (outputPath.IsNullEmptyWhitespace()) {
                return;
            }

            if (this.PK7Archive is not null) {
                entry.ExtractPK7ToFile(this.PK7Archive.Extractor(), filePath: outputPath);
                return;
            }

            entry.ExtractWadToFile(filePath: outputPath);
            return;
        }

        outputPath = GetSaveEntryDirectoryPath(this.File);
        if (outputPath.IsNullEmptyWhitespace()) {
            return;
        }

        if (this.PK7Archive is not null) {
            var extractor = this.PK7Archive.Extractor();
            for (int i = 0; i < lvEntries.SelectedItems.Count; i++) {
                if (lvEntries.SelectedItems[i].Tag is not Entry entry) {
                    continue;
                }
                entry.ExtractPK7ToDirectory(extractor: extractor, directoryPath: outputPath);
            }
            return;
        }

        for (int i = 0; i < lvEntries.SelectedItems.Count; i++) {
            if (lvEntries.SelectedItems[i].Tag is not Entry entry) {
                continue;
            }
            entry.ExtractWadToDirectory(directoryPath: outputPath);
        }
    }
    private void btnExtractAll_Click(object? sender, EventArgs e) {
        string? outputPath = GetSaveEntryDirectoryPath(this.File);
        if (outputPath.IsNullEmptyWhitespace()) {
            return;
        }

        if (this.PK7Archive is not null) {
            var extractor = this.PK7Archive.Extractor();
            for (int i = 0; i < lvEntries.Items.Count; i++) {
                if (lvEntries.Items[i].Tag is not Entry entry) {
                    continue;
                }
                entry.ExtractPK7ToDirectory(extractor: extractor, directoryPath: outputPath);
            }
            return;
        }

        for (int i = 0; i < lvEntries.Items.Count; i++) {
            if (lvEntries.Items[i].Tag is not Entry entry) {
                continue;
            }
            entry.ExtractWadToDirectory(directoryPath: outputPath);
        }
    }

    private void frmEntryExplorer_ResizeEnd(object? sender, EventArgs e) {
        this.ResizeColumn();
    }

    private void chkFullRowSelect_CheckedChanged(object sender, EventArgs e) {
        lvEntries.FullRowSelect = chkFullRowSelect.Checked;
    }
}
