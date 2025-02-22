namespace DOOMer.Core;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HashSlingingSlasher;

[DebuggerDisplay("Internal WAD (IWAD) \\{ Name = {Name}, Path = {Path} \\}")]
public sealed class LoadedInternal : LoadedInfo {
    [JsonIgnore]
    public FileInfo File { get; private set; }

    [AllowNull]
    [JsonIgnore]
    public override string Path {
        get {
            return base.Path ??= this.File.FullName;
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
    public InternalWad? InternalWad { get; private set; }
    [JsonIgnore]
    public Hash MD5 { get; private set; }
    [JsonIgnore]
    public Hash SHA1 { get; private set; }
    [JsonIgnore]
    public Hash CRC32 { get; private set; }

    [JsonIgnore]
    public MapCollection? Maps { get; private set; }

    [JsonIgnore]
    public object? TreeNodeConfig { get; set; }

    [JsonIgnore]
    private string DefaultInternalToolTip {
        get {
            return $$"""
                {{base.DefaultToolTip}}

                {{(this.File.Exists ? "Select this file to load the wad information." : "The file does not exist, select this file to try loading again.")}}
                """;
        }
    }

    [JsonConstructor]
    LoadedInternal() : base(string.Empty, string.Empty) {
        this.File = null!;
    }
    public LoadedInternal(FileInfo file) : this(name: System.IO.Path.GetFileNameWithoutExtension(file.FullName), file: file) { }
    public LoadedInternal(string name, FileInfo file) : base(name: name, path: file.FullName) {
        this.LoadFile(file);
    }

    [MemberNotNull(nameof(this.Maps))]
    public int UpdateMaps() {
        if (this.Maps is not null) {
            return this.Maps.Count;
        }
        return this.ForceUpdateMaps();
    }
    [MemberNotNull(nameof(this.Maps))]

    public int ForceUpdateMaps() {
        this.Maps = base.GetMaps();
        return this.Maps.Count;
    }

    public bool LoadWadInfo() {
        return this.LoadWadInfo(forceUpdate: false);
    }
    public bool LoadWadInfo(bool forceUpdate) {
        if (this.InternalWad?.WadType is not null && !forceUpdate) {
            return false;
        }

        bool exists = this.File.Exists;
        if (!exists) {
            this.File.Refresh();
            if (this.File.Exists == exists) {
                return false;
            }

            if (!this.File.Exists || this.File.Length < 6) {
                this.ToolTipText = this.DefaultInternalToolTip;
                return true;
            }
        }

        Task.WaitAll(Task.Run(() => this.MD5 = Hash.Calculate(this.File, Hash.CalculateMD5)),
            Task.Run(() => this.SHA1 = Hash.Calculate(this.File, Hash.CalculateSHA1)),
            Task.Run(() => this.CRC32 = Hash.Calculate(this.File, Hash.CalculateCRC32)));

        this.InternalWad = InternalWad.FindAnyInternalWad(this) ?? InternalWad.InvalidIWAD;
        this.ToolTipText = $$"""
            {{this.Name}}
            {{this.Path}}{{(this.IsPathRelative ? "\r\nRelative path" : string.Empty)}}

            {{this.InternalWad.GenerateWadComparisonString(this.MD5, this.SHA1, this.CRC32, this.File.Length, null)}}
            """;
        return true;
    }

    public bool UpdateNameAndPath(string name, string path, bool isPathRelative) {
        if (this.Path.IsNullEmptyWhitespace() || path.IsNullEmptyWhitespace()) {
            return false;
        }

        if (this.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        && this.Path.Equals(path, StringComparison.OrdinalIgnoreCase)
        && this.IsPathRelative == isPathRelative) {
            return false;
        }

        this.Name = name;
        this.Path = path;
        this.IsPathRelative = isPathRelative;
        this.LoadFile(new FileInfo(path));
        this.LoadWadInfo(forceUpdate: true);
        return true;
    }

    public bool SetInternalWadName() {
        this.LoadWadInfo();
        if (this.InternalWad is null || this.InternalWad.WadType == WadType.Unknown) {
            return false;
        }

        if (!this.InternalWad.Name.IsNullEmptyWhitespace()) {
            if (this.InternalWad.NameSubtext.IsNullEmptyWhitespace()) {
                this.Name = $"{this.InternalWad.Name} v{this.InternalWad.Version}";
                return true;
            }
            this.Name = $"{this.InternalWad.Name} v{this.InternalWad.Version} ({this.InternalWad.NameSubtext})";
            return true;
        }

        this.Name = System.IO.Path.GetFileNameWithoutExtension(this.File.FullName);
        return true;
    }

    [MemberNotNull(nameof(this.File))]
    private void LoadFile(FileInfo file) {
        this.File = file;
        this.ToolTipText = this.DefaultInternalToolTip;
    }

    protected override void OnDeserialized() {
        if (this.Path.IsNullEmptyWhitespace()) {
            return;
        }
        FileInfo file = new(this.Path);
        this.LoadFile(file);
    }

    public override string ToString() {
        return this.Name;
    }
}
