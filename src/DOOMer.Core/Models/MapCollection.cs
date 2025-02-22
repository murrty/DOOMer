namespace DOOMer.Core;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using pdj.tiny7z.Archive;

[DebuggerDisplay("Count = {Count}")]
public sealed class MapCollection : IReadOnlyList<string> {
    private readonly List<string> _inner = [];

    public int Count => _inner.Count;

    public string this[int index] {
        get {
            return _inner[index];
        }
        set {
            _inner[index] = value.ToUpperInvariant().Trim();
        }
    }

    public bool Add(string map) {
        if (map.IsNullEmptyWhitespace() || this.Contains(map)) {
            return false;
        }
        _inner.Add(map.ToUpperInvariant().Trim());
        return true;
    }
    public bool AddRange(IEnumerable<string> enumerable) {
        bool itemAdded = false;
        foreach (string item in enumerable) {
            if (this.Add(item)) {
                itemAdded = true;
            }
        }
        return itemAdded;
    }
    public void Clear() {
        _inner.Clear();
    }
    public bool Contains(string map) {
        for (int i = 0; i < _inner.Count; i++) {
            if (_inner[i].Equals(map, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        }
        return false;
    }
    public void Remove(string map) {
        for (int i = 0; i < _inner.Count; i++) {
            if (_inner[i].Equals(map, StringComparison.OrdinalIgnoreCase)) {
                _inner.RemoveAt(i);
                return;
            }
        }
    }

    public bool AddMapsFromWadEntries(WadEntry[]? entries) {
        if (entries?.Length is not > 0) {
            return false;
        }

        bool mapsAdded = false;
        foreach (WadEntry entry in entries) {
            if (entry.Length > 0 || entry.Offset < 1) {
                continue;
            }

            if (entry.Name.Contains("_start", StringComparison.InvariantCultureIgnoreCase)
            || entry.Name.Contains("_end", StringComparison.InvariantCultureIgnoreCase)) {
                continue;
            }

            if (this.Add(entry.Name)) {
                mapsAdded = true;
            }
        }

        return mapsAdded;
    }
    public bool AddMapsFromPK3Archive(ZipArchive archive) {
        bool mapsAdded = false;

        foreach (ZipArchiveEntry entry in archive.Entries) {
            if (this.IsValidBSPFilePath(entry.FullName) || this.AddedFullNameAsMap(entry.FullName)) {
                continue;
            }

            if (!entry.Name.Contains("MAPINFO", StringComparison.InvariantCultureIgnoreCase)) {
                continue;
            }

            using var entryStream = entry.Open();
            byte[] bytes = new byte[entry.Length];
            entryStream.Read(bytes);

            if (this.AddMapsFromMAPINFO(Encoding.UTF8.GetString(bytes))) {
                mapsAdded = true;
            }
        }

        return mapsAdded;
    }
    public bool AddMapsFromPK7Archive(SevenZipArchive archive) {
        if (!archive.IsValid) {
            return false;
        }

        var extractor = archive.Extractor();
        if (extractor.Files?.Count is not > 0) {
            return false;
        }

        bool mapsAdded = false;

        foreach (SevenZipArchiveFile entry in extractor.Files.Where(x => !x.IsDirectory).Cast<SevenZipArchiveFile>()) {
            if (this.IsValidBSPFilePath(entry.EntryName) || this.AddedFullNameAsMap(entry.Name)) {
                continue;
            }

            if (!entry.Name.Contains("MAPINFO", StringComparison.InvariantCultureIgnoreCase) || !entry.UnPackIndex.HasValue) {
                continue;
            }

            using MemoryStream ms = new();
            extractor.ExtractFile(entry.UnPackIndex.Value, ms);
            byte[] bytes = ms.ToArray();
            return this.AddMapsFromMAPINFO(Encoding.UTF8.GetString(bytes));
        }

        return mapsAdded;
    }

    bool IsValidBSPFilePath(string FileName) {
        if (!FileName.EndsWith(".bsp")) {
            return false;
        }

        int mapsIndex = FileName.IndexOf("maps", StringComparison.InvariantCultureIgnoreCase);

        if (mapsIndex < 0) {
            return false;
        }

        if (FileName.IndexOf("dm", mapsIndex, StringComparison.InvariantCultureIgnoreCase) > -1 || FileName.IndexOf("obj", mapsIndex, StringComparison.InvariantCultureIgnoreCase) > 1) {
            this.Add(FileName[5..(FileName.Length - 5 - 4)]);
            return true;
        }

        // OLD
        //if (FileName.EndsWith(".bsp")) {
        //    int p1, p2;
        //    if ((p1 = FileName.IndexOf("maps", StringComparison.InvariantCultureIgnoreCase)) > -1) {
        //        if ((p2 = FileName.IndexOf("dm", p1, StringComparison.InvariantCultureIgnoreCase)) == -1) {
        //            return FileName.IndexOf("obj", p1, StringComparison.InvariantCultureIgnoreCase) > -1;
        //        }
        //        else {
        //            return true;
        //        }
        //    }
        //}
        return false;
    }
    bool AddedFullNameAsMap(string fullName) {
        if (!fullName.StartsWith("maps/", StringComparison.InvariantCultureIgnoreCase)
        && !fullName.StartsWith("maps\\", StringComparison.InvariantCultureIgnoreCase)) {
            return false;
        }

        string mapName = Path.GetFileNameWithoutExtension(fullName);
        this.Add(mapName);
        return true;
    }
    bool AddMapsFromMAPINFO(string mapInfo) {
        string[] entries = mapInfo.Split('\n');
        bool mapsAdded = false;

        for (int x = 0; x < entries.Length; x++) {
            string line = entries[x];
            if (!line.StartsWith("map ", StringComparison.InvariantCultureIgnoreCase)) {
                continue;
            }
            string mapName = line.Split(' ')[1];
            this.Add(mapName);
            mapsAdded = true;
        }

        return mapsAdded;
    }

    public IEnumerator<string> GetEnumerator() {
        return _inner.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
        return _inner.GetEnumerator();
    }
}
