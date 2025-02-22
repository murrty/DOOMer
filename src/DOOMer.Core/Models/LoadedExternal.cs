namespace DOOMer.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HashSlingingSlasher;

[DebuggerDisplay("External {IsFile ? \"File\" : IsDirectory ? \"Directory\" : IsGroup ? \"Group\" : \"Unknown External\"} \\{ Name = {Name}, Type = {ExternalType}, Path = {Path} \\}")]
public sealed class LoadedExternal : LoadedInfo, IJsonOnSerialized {
    [JsonPropertyName("dependants")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<LoadedExternal> dependantlist { get; set; } = [];

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("external_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ExternalType ExternalType { get; set; }

    [JsonIgnore]
    public override string? Path { get { return base.Path; } set { base.Path = value; } }

    [JsonIgnore]
    public LoadedExternal? Parent { get; private set; }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(this.File))]
    [MemberNotNullWhen(true, nameof(Path))]
    public bool IsFile { get; private set; }
    [JsonIgnore]
    public FileInfo? File { get; private set; }
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(this.File))]
    public bool FileExists => this.IsFile && this.File.PathExists();

    [JsonIgnore]
    public InternalWad? InternalWad { get; private set; }
    [JsonIgnore]
    public Hash MD5 { get; private set; }
    [JsonIgnore]
    public Hash SHA1 { get; private set; }
    [JsonIgnore]
    public Hash CRC32 { get; private set; }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Path))]
    [MemberNotNullWhen(true, nameof(this.Directory))]
    public bool IsDirectory { get; private set; }
    [JsonIgnore]
    public DirectoryInfo? Directory { get; private set; }
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(this.Directory))]
    public bool DirectoryExists => this.IsDirectory && this.Directory.PathExists();
    [JsonPropertyName("directory_as_file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DirectoryAsFile { get; set; }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(this.Dependants), nameof(this.dependantlist))]
    public bool IsGroup { get; private set; }

    [JsonIgnore]
    public IReadOnlyList<LoadedExternal> Dependants => this.dependantlist;

    [JsonIgnore]
    public bool IsDehack { get; private set; }
    [JsonIgnore]
    public bool IsDehackBoom { get; private set; }
    [JsonIgnore]
    public MapCollection? Maps { get; private set; }

    [JsonIgnore]
    private string DependantsToolTip {
        get {
            return this.dependantlist.Count > 0 ? $"\r\n{this.dependantlist.Count} dependants" : string.Empty;
        }
    }

    [JsonIgnore]
    private string DefaultFileToolTip {
        get {
            return this.IsFile ? $$"""
                {{base.DefaultToolTip}}{{this.DependantsToolTip}}
                {{(this.IsDehack ? "Dehack PWAD" : (this.IsDehackBoom ? "Dehack BOOM Extension PWAD" : "Patch WAD (PWAD)"))}}

                {{(this.File.Exists ? "Select this file to load the wad information." : "The file does not exist, select this file to try loading again.")}}
                """ : "Cannot use the default file tool tip on non-file external items.";
        }
    }
    [JsonIgnore]
    private string DefaultDirectoryToolTip {
        get {
            return this.IsDirectory ? $$"""
                {{base.DefaultToolTip}}{{this.DependantsToolTip}}

                {{(this.Directory.Exists ? "Directory launched along the file argument." : "The directory does not exist, select this directory to try loading again.")}}
                """ : "Cannot use the default directory tool tip on non-directory external items.";
        }
    }
    [JsonIgnore]
    private string DefaultGroupToolTip {
        get {
            return this.IsGroup ? $$"""
                {{this.Name}}
                Group of {{this.dependantlist.Count}} external files.
                """ : "Cannot use the default group tool tip on non-group items.";
        }
    }

    [JsonConstructor]
    LoadedExternal() : base(name: string.Empty) { }
    public LoadedExternal(string name, string? path) : base(name: name, path: path) {
        this.LoadFromPath();
    }
    public LoadedExternal(FileInfo file) : this(name: System.IO.Path.GetFileNameWithoutExtension(file.FullName), file: file) { }
    public LoadedExternal(string name, FileInfo file) : base(name: name, path: file.FullName) {
        this.LoadFile(file);
    }
    public LoadedExternal(DirectoryInfo directory, bool asGroup, bool relativePath) : this(name: System.IO.Path.GetFileNameWithoutExtension(directory.FullName), directory: directory, asGroup: asGroup, relativePath: relativePath) { }
    public LoadedExternal(string name, DirectoryInfo directory, bool asGroup, bool relativePath) : base(name: name) {
        if (asGroup) {
            this.LoadGroup(EnumerateFiles(directory), relativePath);
            return;
        }
        this.Path = directory.FullName;
        this.LoadDirectory(directory);
    }

    [MemberNotNull(nameof(this.Maps))]
    public int UpdateMaps() {
        if (this.Maps is not null) {
            return 0;
        }

        if (this.IsFile) {
            this.Maps = base.GetMaps();
            return this.Maps.Count;
        }

        this.Maps = [];
        return 0;
    }
    [MemberNotNull(nameof(this.Maps))]
    public int UpdateAllMaps() {
        int maps = 0;

        if (this.IsFile && this.Maps is null) {
            maps += Math.Max(0, this.UpdateMaps());
        }
        else {
            this.Maps ??= [];
        }

        this.dependantlist?.WhereFor(
            x => x.IsFile && !x.IsDehack && !x.IsDehackBoom && x.FileExists,
            x => maps += Math.Max(0, x.UpdateMaps()));

        return maps;
    }
    public bool LoadWadInfo() {
        return this.LoadWadInfo(false);
    }
    public bool LoadWadInfo(bool forceUpdate) {
        if (this.IsDirectory) {
            if (!this.DirectoryExists && !forceUpdate) {
                return false;
            }
            this.ToolTipText = this.DefaultDirectoryToolTip;
            return true;
        }

        if (this.IsGroup) {
            if (!forceUpdate) {
                return false;
            }

            if (forceUpdate) {
                this.UpdateDependants();
            }

            this.ToolTipText = this.DefaultGroupToolTip;
            return true;
        }

        if (!this.IsFile || (this.InternalWad?.WadType is not null && !forceUpdate)) {
            return false;
        }

        if (!this.FileExists) {
            return false;
        }

        bool fileExists = this.File.Exists;
        if (!fileExists) {
            this.File.Refresh();
            if (this.File.Exists == fileExists) {
                return false;
            }
            if (!this.File.Exists || this.File.Length < 6) {
                this.ToolTipText = this.DefaultFileToolTip;
                return true;
            }
        }

        Task.WaitAll(Task.Run(() => this.MD5 = Hash.Calculate(this.File, Hash.CalculateMD5)),
            Task.Run(() => this.SHA1 = Hash.Calculate(this.File, Hash.CalculateSHA1)),
            Task.Run(() => this.CRC32 = Hash.Calculate(this.File, Hash.CalculateCRC32)));

        this.InternalWad = InternalWad.FindAnyInternalWad(this) ?? InternalWad.InvalidPWAD;
        this.ToolTipText = $$"""
            {{this.Name}}
            {{this.File.FullName}}{{(this.IsPathRelative ? "\r\nRelative path" : string.Empty)}}{{(this.dependantlist.Count > 0 ? $"{this.dependantlist.Count} dependants" : string.Empty)}}
            {{(this.IsDehack ? "Dehack PWAD" : (this.IsDehackBoom ? "Dehack BOOM Extension PWAD" : "Patch WAD (PWAD)"))}}

            {{this.InternalWad.GenerateWadComparisonString(this.MD5, this.SHA1, this.CRC32, this.File.Length, null)}}
            """;
        return true;
    }
    public bool SetInternalWadName() {
        if (!this.IsFile) {
            return false;
        }

        this.LoadWadInfo();
        if (this.InternalWad is null || this.InternalWad.WadType == WadType.Unknown) {
            return false;
        }

        if (this.InternalWad?.Name.IsNullEmptyWhitespace() == false) {
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

    public static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo directory) {
        return directory.EnumerateFiles("*", SearchOption.AllDirectories)
            .Where(x => InternalWad.IsSupportedFile(x.Extension));
    }
    public bool HasNameAndPathChanged(string name, string? path, bool isPathRelative) {
        if (this.IsGroup) {
            if (base.IsNameEqual(name)) {
                return false;
            }
            this.Name = name;
            this.LoadWadInfo(forceUpdate: true);
            return true;
        }

        if (!base.UpdateNameOrPath(name: name, path: path, isPathRelative: isPathRelative)) {
            return false;
        }

        if (this.IsFile) {
            this.LoadFile(new FileInfo(this.Path));
            return true;
        }

        if (this.IsDirectory) {
            this.LoadDirectory(new DirectoryInfo(this.Path));
            return true;
        }

        this.LoadWadInfo(forceUpdate: true);
        return true;
    }

    public bool AddDependant(LoadedExternal external) {
        if (this.dependantlist.Contains(external)) {
            return false;
        }

        external.UpdateDependants();
        external.Parent = this;
        this.dependantlist.Add(external);
        return true;
    }
    public void UpdateDependants() {
        for (int i = 0; i < this.dependantlist.Count; i++) {
            var dependant = this.dependantlist[i];
            dependant.UpdateDependants();
            dependant.Parent = this;
        }
    }
    public void RemoveDependant() {
        this.Parent?.RemoveDependant(this);
    }
    public void RemoveDependant(LoadedExternal external) {
        this.dependantlist.Remove(external);
    }
    public void MoveDependantUp() {
        this.Parent?.MoveDependantUp(this);
    }
    public void MoveDependantDown() {
        this.Parent?.MoveDependantDown(this);
    }
    public void MoveDependantUp(LoadedExternal external) {
        if (this.dependantlist?.Count is not > 0) {
            return;
        }

        int index = this.dependantlist.IndexOf(external);
        if (index < 1) {
            return;
        }

        int newIndex = index - 1;
        LoadedExternal previousItem = this.dependantlist[newIndex];
        this.dependantlist[newIndex] = external;
        this.dependantlist[index] = previousItem;
    }
    public void MoveDependantDown(LoadedExternal external) {
        if (this.dependantlist?.Count is not > 0) {
            return;
        }

        int index = this.dependantlist.IndexOf(external);
        if (index < 0 || index == this.dependantlist.Count - 1) {
            return;
        }

        int newIndex = index + 1;
        LoadedExternal previousItem = this.dependantlist[newIndex];
        this.dependantlist[newIndex] = external;
        this.dependantlist[index] = previousItem;
    }

    private void LoadFromPath() {
        if (!this.Path.IsNullEmptyWhitespace()) {
            FileInfo file = new(this.Path);
            if (file.Exists && file.Length > 0) {
                this.LoadFile(file);
                return;
            }

            DirectoryInfo directory = new(this.Path);
            if (directory.Exists) {
                this.LoadDirectory(directory);
                return;
            }
        }

        if (this.dependantlist?.Count > 0) {
            this.LoadGroup([], false);
            return;
        }

        this.ToolTipText = "File or directory does not exist.";
    }
    [MemberNotNull(nameof(this.File))]
    private void LoadFile(FileInfo file) {
        this.IsFile = true;
        this.InternalWad = null;
        this.ExternalType = ExternalType.File;
        this.File = file;
        this.Maps = null;
        this.dependantlist ??= [];

        this.IsDehack = InternalWad.IsDehack(this.File.Extension);
        this.IsDehackBoom = InternalWad.IsDehackBoomEx(this.File.Extension);

        this.UpdateDependants();
        this.ToolTipText = this.DefaultFileToolTip;
    }
    [MemberNotNull(nameof(this.Directory))]
    private void LoadDirectory(DirectoryInfo directory) {
        this.IsDirectory = true;
        this.ExternalType = ExternalType.Directory;
        this.Directory = directory;
        this.Maps = null;
        this.dependantlist ??= [];

        this.UpdateDependants();
        this.ToolTipText = this.DefaultDirectoryToolTip;
    }
    [MemberNotNull(nameof(this.dependantlist))]
    private void LoadGroup(IEnumerable<FileInfo> dependants, bool relativePath) {
        this.IsGroup = true;
        this.ExternalType = ExternalType.Group;
        this.Maps = null;
        this.dependantlist ??= [];

        foreach (FileInfo file in dependants) {
            //if (InternalWad.GetWadType(file) == WadType.Unknown) {
            //    continue;
            //}

            LoadedExternal external = new(name: System.IO.Path.GetFileNameWithoutExtension(file.FullName), file: file) {
                Enabled = true,
            };

            if (relativePath && file.FullName.IsPathRelativeCompatible()) {
                external.IsPathRelative = true;
            }

            external.UpdateDependants();
            external.SetInternalWadName();
            this.dependantlist.Add(external);
        }

        this.UpdateDependants();
        this.LoadWadInfo(true);
    }

    protected override void OnSerializing() {
        if (this.dependantlist.Count < 1) {
            this.dependantlist = null!;
        }
        if (this.DirectoryAsFile && !this.IsDirectory) {
            this.DirectoryAsFile = default;
        }
        if (this.IsGroup && this.IsPathRelative) {
            this.IsPathRelative = false;
        }
    }
    void IJsonOnSerialized.OnSerialized() {
        this.dependantlist ??= [];
    }
    protected override void OnDeserialized() {
        this.dependantlist ??= [];
        switch (this.ExternalType) {
            case ExternalType.File:
                if (!this.Path.IsNullEmptyWhitespace()) {
                    this.LoadFile(new FileInfo(this.Path));
                    return;
                }
                break;
            case ExternalType.Directory:
                if (!this.Path.IsNullEmptyWhitespace()) {
                    this.LoadDirectory(new DirectoryInfo(this.Path));
                    return;
                }
                break;
            case ExternalType.Group:
                this.LoadGroup([], false);
                break;
            default:
                this.LoadFromPath();
                break;
        }
    }

    public bool SamePath(LoadedExternal other) {
        if (other is null) {
            return false;
        }
        return this.Path?.Equals(other.Path) == true;
    }
    public override string ToString() {
        return this.Name;
    }
}
