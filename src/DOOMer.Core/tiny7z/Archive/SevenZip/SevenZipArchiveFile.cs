﻿// -----------------
// <auto-generated/>
// -----------------

#nullable enable
namespace pdj.tiny7z.Archive;

using System;
using pdj.tiny7z.Common;

public class SevenZipArchiveFile : ArchiveFile {
    public UInt64? UnPackIndex { get; set; }

    public MultiFileStream.Source? Source { get; set; }

    public SevenZipArchiveFile() : base() {
        this.UnPackIndex = null;
        this.Source = null;
    }
}
