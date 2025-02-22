namespace DOOMer.Core;

public enum WadType {
    /// <summary>
    ///     The WAD type is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    ///     The wad is an IWAD.
    /// </summary>
    IWAD,
    /// <summary>
    ///     The wad is a PWAD.
    /// </summary>
    PWAD,
    /// <summary>
    ///     The wad is a PACK package.
    /// </summary>
    PACK,
    /// <summary>
    ///     The wad is a WAD2 package.
    /// </summary>
    WAD2,
    /// <summary>
    ///     The wad is a WAD3 package.
    /// </summary>
    WAD3,
    /// <summary>
    ///     The wad is a ZDoom PK3 WAD or zip archive.
    /// </summary>
    PK3,
    /// <summary>
    ///     The wad is a ZDoom IPK3 WAD or zip archive.
    /// </summary>
    IPK3,
    /// <summary>
    ///     The wad is a PK7 WAD or 7z archive.
    /// </summary>
    PK7,
    /// <summary>
    ///     The wad is a IPK7 WAD or 7z archive.
    /// </summary>
    IPK7,

    /// <summary>
    ///     The wad is a dehack.
    /// </summary>
    Dehack,
    /// <summary>
    ///     The wad is a dehack supported with the BOOM Extensions.
    /// </summary>
    DehackBOOMEx,

    /// <summary>
    ///     The wad is an IWAD, but is incorrectly marked.
    /// </summary>
    IWADIncorrectlyMarked,
    /// <summary>
    ///     The wad is a PWAD, but is incorrectly marked.
    /// </summary>
    PWADIncorrectlyMarked,
    /// <summary>
    ///     The wad is a ZDoom PK3 WAD or zip archive, but is incorrectly marked.
    /// </summary>
    PK3IncorrectlyMarked,
    /// <summary>
    ///     The wad is a ZDoom IPK3 WAD or zip archive, but is incorrectly marked.
    /// </summary>
    IPK3IncorrectlyMarked,
    /// <summary>
    ///     The wad is a PK7 WAD or 7z archive, but is incorrectly marked.
    /// </summary>
    PK7IncorrectlyMarked,
    /// <summary>
    ///     The wad is a IPK7 WAD or 7z archive, but is incorrectly marked.
    /// </summary>
    IPK7IncorrectlyMarked,
}
