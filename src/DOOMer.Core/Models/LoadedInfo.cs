namespace DOOMer.Core;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using pdj.tiny7z.Archive;

public abstract class LoadedInfo : IJsonOnSerializing, IJsonOnDeserialized, IDisposable, IAsyncDisposable {
    protected const string PathJsonKey = "path";

    static char pthsp => System.IO.Path.DirectorySeparatorChar;
    static char pthsp2 => System.IO.Path.AltDirectorySeparatorChar;

    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set { field = (value?.Trim()).UnlessNullEmptyWhitespace(string.Empty); } }

    [JsonIgnore]
    public virtual string? Path { get; set { field = (value?.Trim()).UnlessNullEmptyWhitespace(null!); } }

    [JsonPropertyName(PathJsonKey)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SerializingPath { get; set; }

    [JsonIgnore]
    public bool IsPathRelative { get; set; }

    [JsonIgnore]
    public object? TreeNode { get; set; }

    [JsonIgnore]
    public string ToolTipText { get; protected set; }

    [JsonIgnore]
    public bool IsDisposed { get; private set; }

    [JsonIgnore]
    protected string DefaultToolTip {
        get {
            return $$"""
                {{this.Name}}
                {{this.Path}}{{(this.IsPathRelative ? "\r\nRelative enabled path" : string.Empty)}}
                """;
        }
    }

    protected LoadedInfo(string name) {
        this.Name = name;
        this.ToolTipText = string.Empty;
    }
    protected LoadedInfo(string name, string? path) : this(name: name) => this.Path = this.SerializingPath = path;

    protected MapCollection GetMaps() {
        if (this.Path.IsNullEmptyWhitespace()) {
            return [];
        }

        FileInfo file = new(this.Path);
        if (!file.Exists || file.Length < 6) {
            return [];
        }

        using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        Span<byte> header = stackalloc byte[6];
        fileStream.Read(header);
        fileStream.Position = 0;
        WadType wadType = InternalWad.GetWadType(header, file);
        MapCollection maps = [];

        switch (wadType) {
            case WadType.IWAD:
            case WadType.PWAD:
            case WadType.IWADIncorrectlyMarked:
            case WadType.PWADIncorrectlyMarked: {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes);
                fileStream.Close();

                maps.AddMapsFromWadEntries(InternalWad.EnumerateWadFileEntries(bytes: fileBytes,
                    file: file,
                    scanEntriesCrc32: false,
                    wadType: wadType));
            } break;

            case WadType.PK3:
            case WadType.IPK3:
            case WadType.PK3IncorrectlyMarked:
            case WadType.IPK3IncorrectlyMarked: {
                using ZipArchive pk3archive = new(fileStream);
                maps.AddMapsFromPK3Archive(pk3archive);
            } break;

            case WadType.PK7:
            case WadType.IPK7:
            case WadType.PK7IncorrectlyMarked:
            case WadType.IPK7IncorrectlyMarked: {
                using var pk7archive = new SevenZipArchive(fileStream, FileAccess.Read);
                maps.AddMapsFromPK7Archive(pk7archive);
            } break;
        }

        return maps;
    }

    public bool IsNameEqual(string? name) {
        return this.Name?.Equals(name, StringComparison.Ordinal) == true;
    }
    public bool IsPathEqual(string? path) {
        return this.Path?.Equals(path, StringComparison.Ordinal) == true;
    }
    protected bool UpdateNameOrPath(string name, string? path, bool isPathRelative) {
        bool namesMatch = this.IsNameEqual(name);
        if (!namesMatch) {
            this.Name = name;
        }

        bool pathsMatch = this.IsPathEqual(path);
        if (!pathsMatch) {
            this.Path = path;
        }

        bool pathRelativityMatch = this.IsPathRelative == isPathRelative;
        if (!pathRelativityMatch) {
            this.IsPathRelative = isPathRelative;
        }

        return !(namesMatch && pathsMatch && pathRelativityMatch);
    }
    protected bool IsNameOrPathUpdatedOld(string name, string? path, bool isPathRelative) {
        if (this.IsNameEqual(name)
        && ((this.Path is null && path is null) || this.IsPathEqual(path))
        && this.IsPathRelative == isPathRelative) {
            return false;
        }

        this.Name = name;
        this.Path = path;
        this.IsPathRelative = isPathRelative;
        return true;
    }

    protected virtual void OnSerializing() { }
    protected virtual void OnDeserialized() { }

    void IJsonOnSerializing.OnSerializing() {
        this.SerializingPath = (this.IsPathRelative && this.Path.IsPathRelativeCompatible()) ?
            $".{pthsp}{this.Path[(Environment.CurrentDirectory.Length + 1)..]}"
            : this.Path.UnlessNullEmptyWhitespace(null!);

        this.OnSerializing();
    }
    void IJsonOnDeserialized.OnDeserialized() {
        if (this.SerializingPath.IsNullEmptyWhitespace()) {
            this.Path = null;
            this.SerializingPath = null;
            this.OnDeserialized();
            return;
        }

        static bool isRelative([NotNullWhen(true)] string? str) {
            if (str?.Length is not > 2 || str[0] != '.') {
                return false;
            }
            char ch = str[1];
            return ch == pthsp || ch == pthsp2;
        }

        if (this.IsPathRelative = isRelative(this.SerializingPath)) {
            this.Path = System.IO.Path.Join(
                path1: Environment.CurrentDirectory,
                path2: this.SerializingPath[1..]
                    .TrimStartWhitespace(pthsp, pthsp2))
                    .Replace(pthsp2, pthsp);
        }
        else {
            this.Path = this.SerializingPath
                .Replace(pthsp2, pthsp);
        }

        this.OnDeserialized();
    }

    protected virtual void Dispose(bool disposing) { }
    protected virtual ValueTask DisposeAsync(bool disposing) {
        return ValueTask.CompletedTask;
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        this.IsDisposed = true;
        GC.SuppressFinalize(this);
    }
    public async ValueTask DisposeAsync() {
        await this.DisposeAsync(disposing: true)
            .ConfigureAwait(false);
        this.IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}
