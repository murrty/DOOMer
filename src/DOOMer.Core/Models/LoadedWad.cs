namespace DOOMer.Core;

using HashSlingingSlasher;
using pdj.tiny7z.Archive;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[DebuggerDisplay("WAD \\{ Name = {Name}, Path = {Path} \\}")]
public abstract class LoadedWad : LoadedInfo {
    [JsonIgnore]
    public abstract bool FileExists { get; }

    [JsonIgnore]
    public InternalWad? InternalWad { get; protected set; }
    [JsonIgnore]
    public Hash MD5 { get; protected set; }
    [JsonIgnore]
    public Hash SHA1 { get; protected set; }
    [JsonIgnore]
    public Hash CRC32 { get; protected set; }
    [JsonIgnore]
    public MapCollection? Maps { get; protected set; }

    protected LoadedWad(string name, string? path) : base(name: name, path: path) { }

    /// <summary>
    ///     Updates the loaded wads maps.
    /// </summary>
    /// <param name="file">
    ///     The file to load the maps from.
    /// </param>
    /// <param name="forceUpdate">
    ///     Whether to force-update the maps, replacing the collection.
    /// </param>
    /// <returns>
    ///     The map count of maps added.
    /// </returns>
    [MemberNotNull(nameof(Maps))]
    protected int UpdateMaps(FileInfo file, bool forceUpdate) {
        if (this.Maps is null) {
            this.Maps ??= [];
            return 0;
        }

        if (!forceUpdate) {
            return this.Maps.Count;
        }

        this.Maps = [];

        if (!file.PathExists()) {
            return 0;
        }

        using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        Span<byte> header = stackalloc byte[6];
        fileStream.Read(header);
        fileStream.Position = 0;
        WadType wadType = InternalWad.GetWadType(header, file);

        switch (wadType) {
            case WadType.IWAD:
            case WadType.PWAD:
            case WadType.IWADIncorrectlyMarked:
            case WadType.PWADIncorrectlyMarked: {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes);
                fileStream.Close();

                this.Maps.AddMapsFromWadEntries(InternalWad.EnumerateWadFileEntries(bytes: fileBytes,
                    file: file,
                    scanEntriesCrc32: false,
                    wadType: wadType));
            } break;

            case WadType.PK3:
            case WadType.IPK3:
            case WadType.PK3IncorrectlyMarked:
            case WadType.IPK3IncorrectlyMarked: {
                using ZipArchive pk3archive = new(fileStream);
                this.Maps.AddMapsFromPK3Archive(pk3archive);
            } break;

            case WadType.PK7:
            case WadType.IPK7:
            case WadType.PK7IncorrectlyMarked:
            case WadType.IPK7IncorrectlyMarked: {
                using var pk7archive = new SevenZipArchive(fileStream, FileAccess.Read);
                this.Maps.AddMapsFromPK7Archive(pk7archive);
            } break;
        }

        return this.Maps.Count;
    }
    [MemberNotNullWhen(true, nameof(InternalWad))]
    protected bool ScanWad(FileInfo file, InternalWad invalidWad) {
        if (!file.PathExists()) {
            return false;
        }

        Task.WaitAll(Task.Run(() => this.MD5 = Hash.Calculate(file, Hash.CalculateMD5)),
            Task.Run(() => this.SHA1 = Hash.Calculate(file, Hash.CalculateSHA1)),
            Task.Run(() => this.CRC32 = Hash.Calculate(file, Hash.CalculateCRC32)));

        this.InternalWad = InternalWad.FindAnyInternalWad(this.MD5, this.SHA1, this.CRC32, file.Length) ?? invalidWad;
        return false;
    }
}
