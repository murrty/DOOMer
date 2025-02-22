namespace DOOMer.Core;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;

[DebuggerDisplay("Source Port \\{ Name = {Name}, Path = {Path} \\}")]
public sealed class LoadedPort : LoadedInfo {
    [JsonIgnore]
    public FileInfo File { get; private set; }

    [AllowNull]
    [JsonIgnore]
    public override string Path {
        get {
            return base.Path ??= this.File?.FullName!;
        }
        set {
            base.Path = value;
        }
    }

    [JsonIgnore]
    public bool FileExists {
        get {
            if (this.File is null) {
                return false;
            }
            this.File.Refresh();
            return this.File.Exists;
        }
    }

    [JsonIgnore]
    public string FileName { get; private set; }

    [JsonIgnore]
    public string WorkingDirectory { get; private set; }

    [JsonIgnore]
    private string DefaultPortToolTip {
        get {
            return $$"""
                Name: {{this.Name}}
                File: {{this.File.Name}}
                Working directory: {{this.WorkingDirectory}}{{(this.IsPathRelative ? "\r\nRelative path" : string.Empty)}}
                """;
        }
    }

    [JsonConstructor]
    LoadedPort() : base(string.Empty, string.Empty) {
        this.File = null!;
        this.FileName = string.Empty;
        this.WorkingDirectory = string.Empty;
        this.ToolTipText = "No tip text set.";
    }
    public LoadedPort(FileInfo file) : this(name: System.IO.Path.GetFileNameWithoutExtension(file.FullName), file: file) { }
    public LoadedPort(string name, FileInfo file) : base(name: name, path: file.FullName) {
        this.LoadPort(file);
    }

    public bool LoadPortInfo() {
        return this.LoadPortInfo(forceUpdate: false);
    }
    public bool LoadPortInfo(bool forceUpdate) {
        bool fileExists = this.File.Exists;
        if (!fileExists) {
            this.File.Refresh();
            if (this.File.Exists == fileExists && !forceUpdate) {
                return false;
            }
            if (!this.File.Exists || this.File.Length < 6) {
                this.ToolTipText = this.DefaultPortToolTip;
                return true;
            }
        }

        this.ToolTipText = $$"""
            Name: {{this.Name}}
            File: {{this.File.Name}}
            Working directory: {{this.WorkingDirectory}}{{(this.IsPathRelative ? "\r\nRelative path" : string.Empty)}}
            """;
        return true;
    }

    public bool UpdateNameAndPath(string name, string path, bool isPathRelative) {
        if (this.Path.IsNullEmptyWhitespace() || path.IsNullEmptyWhitespace()) {
            return false;
        }

        if (!base.UpdateNameOrPath(name: name, path: path, isPathRelative: isPathRelative)) {
            return false;
        }

        this.LoadPort(new FileInfo(path));
        this.LoadPortInfo(forceUpdate: true);
        return true;
    }

    [MemberNotNull(nameof(this.File))]
    [MemberNotNull(nameof(this.FileName))]
    [MemberNotNull(nameof(this.WorkingDirectory))]
    private void LoadPort(FileInfo file) {
        this.File = file;
        this.FileName = file.Name;
        this.WorkingDirectory = System.IO.Path.GetDirectoryName(file.FullName) ?? throw new Exception($"Invalid path name for '{file.FullName}'.");
        this.LoadPortInfo();
    }

    protected override void OnDeserialized() {
        if (this.Path.IsNullEmptyWhitespace()) {
            return;
        }
        FileInfo file = new(this.Path);
        this.LoadPort(file);
        this.ToolTipText = this.DefaultPortToolTip;
    }

    public override string ToString() {
        return this.Name;
    }
}
