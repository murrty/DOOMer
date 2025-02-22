namespace DOOMer.WinForms;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using DOOMer.Core;
using static @const;

internal static class FindResourceFile {
    [AllowNull]
    internal static string LastDirectory { get; private set => field = value.UnlessNullEmptyWhitespace(Environment.CurrentDirectory); } = Environment.CurrentDirectory;

    public static void UpdateLastDirectory(OpenFileDialog ofd) {
        LastDirectory = Path.GetDirectoryName(ofd.FileNames?.Length > 0 ? ofd.FileNames[0] : ofd.FileName);
    }
    public static void UpdateLastDirectory(SaveFileDialog sfd) {
        LastDirectory = Path.GetDirectoryName(sfd.FileNames?.Length > 0 ? sfd.FileNames[0] : sfd.FileName);
    }
    public static void UpdateLastDirectory(FolderBrowserDialog fbd) {
        LastDirectory = Path.GetDirectoryName(fbd.SelectedPath);
    }

    public static OpenFileDialog GetPortDialog(bool multiSelect) {
        return new () {
            Title = multiSelect ? SelectMultiplePorts : SelectOnePort,
            InitialDirectory = LastDirectory,
            Filter = PortFilter,
            Multiselect = multiSelect,
            RestoreDirectory = false,
        };
    }
    public static OpenFileDialog GetIWadDialog(bool multiSelect) {
        return new () {
            Title = multiSelect ? SelectMultipleIWADs : SelectOneIWAD,
            InitialDirectory = LastDirectory,
            Filter = IWADFilter,
            Multiselect = multiSelect,
            RestoreDirectory = false,
        };
    }
    public static OpenFileDialog GetExternalFileDialog(bool multiSelect) {
        return new () {
            Title = multiSelect ? SelectMultipleExternalFiles : SelectOneExternalFile,
            InitialDirectory = LastDirectory,
            Filter = ExternalFilter,
            Multiselect = multiSelect,
            RestoreDirectory = false,
        };
    }
    public static FolderBrowserDialog GetExternalDirectoryDialog(AddExternalType externalType, bool multiSelect) {
        return new () {
            Description = externalType switch {
                AddExternalType.ExternalGroup => multiSelect ? SelectMultipleGroups : SelectOneGroup,
                AddExternalType.ExternalDirectory => multiSelect ? SelectMultipleDirectories : SelectOneDirectory,
                AddExternalType.ExternalGroupOrDirectory => multiSelect ? SelectMultipleGroupsOrDirectories : SelectOneDirectoryOrGroup,
                _ => multiSelect ? SelectMulitpleDirectoryFallback : SelectOneDirectoryFallback,
            },
            InitialDirectory = LastDirectory,
            //Multiselect = multiSelect,
            UseDescriptionForTitle = true,
            AddToRecent = false,
        };
    }
    public static OpenFileDialog GetSaveDialog(bool multiSelect) {
        return new () {
            Title = multiSelect ? SelectMultipleSaves : SelectOneSave,
            InitialDirectory = LastDirectory,
            Filter = SaveFilter,
            Multiselect = multiSelect,
            RestoreDirectory = false,
        };
    }
}

file static class @const {
    public static readonly string PortFilter = string.Join('|', ExecutableFilesFilter, AllFilesFilter);
    public static readonly string IWADFilter = string.Join('|', IWadsFileFilter, ArchiveFileFilter, SpecializedArchiveFileFilter, AllFilesFilter);
    public static readonly string ExternalFilter = string.Join('|', DoomResourceFileFilter, WADFileFilter, PatchFileFilter, ConfigFileFilter, ArchiveFileFilter, SpecializedArchiveFileFilter, AllFilesFilter);
    public static readonly string SaveFilter = string.Join('|', AllSavesFileFilter, DoomSavesFileFilter, HereticSaveFileFilter, HexenSaveFileFilter, StrifeSaveFileFilter, AllFilesFilter);

    public const string SelectOnePort = "Select a source port to add...";
    public const string SelectMultiplePorts = "Select source ports to add...";
    public const string SelectOneIWAD = "Select an internal wad (IWAD) file...";
    public const string SelectMultipleIWADs = "Select internal wad (IWAD) files...";
    public const string SelectOneExternalFile = "Select an external file to add...";
    public const string SelectMultipleExternalFiles = "Select external files to add...";
    public const string SelectOneGroup = "Select a directory to add files as a group...";
    public const string SelectMultipleGroups = "Select directories to add files to as individual groups...";
    public const string SelectOneDirectory = "Select a directory to add...";
    public const string SelectMultipleDirectories = "Select directories to add...";
    public const string SelectOneDirectoryOrGroup = "Select a directory to add as a group or as a loaded directory...";
    public const string SelectMultipleGroupsOrDirectories = "Select directories to add as groups or as a loaded directory...";
    public const string SelectOneDirectoryFallback = "Select a directory...";
    public const string SelectMulitpleDirectoryFallback = "Select directories...";
    public const string SelectOneSave = "Select a save file to use...";
    public const string SelectMultipleSaves = "Select save files to use...";

    public const string drff  = "*.wad;*.iwad;*.pk3;*.ipk3;*.pk7;*.ipk7;*.zip;*.7z;*.p7z;*.pkz;*.pke;*.deh;*.bex;*.cfg";
    public const string wff   = "*.wad;*.iwad";
    public const string pff   = "*.deh;*.bex";
    public const string cff   = "*.cfg";
    public const string aff   = "*.pk3;*.ipk3;*.pk7;*.ipk7;*.zip;*.7z;*.p7z;*.pkz;*.pke";
    public const string saff  = "*.pk3;*.ipk3;*.pk7;*.ipk7;*.p7z;*.pkz;*.pke";
    public const string iwff  = "*.wad;*.iwad;*.pk3;*.ipk3;*.pk7;*.ipk7";
    public const string dsff  = "*.zds;*.dsg;*.esg";
    public const string hssff = "*.hsg";
    public const string hxsff = "*.hxs";
    public const string ssff  = "*.ssg";

    public const string DoomResourceFileFilter = $"All DOOM resource files ({drff})|{drff}";
    public const string WADFileFilter = $"WAD files ({wff})|{wff}";
    public const string PatchFileFilter = $"Path files ({pff})|{pff}";
    public const string ConfigFileFilter = $"Config files ({cff})|{cff}";
    public const string ArchiveFileFilter = $"All supported archives ({aff})|{aff}";
    public const string SpecializedArchiveFileFilter = $"All supported specialized archies ({saff})|{saff}";
    public const string IWadsFileFilter = $"IWAD files ({iwff})|{iwff}";
    public const string DoomSavesFileFilter = $"DOOM save files ({dsff})|{dsff}";
    public const string HereticSaveFileFilter = $"Heretic save files ({hssff})|{hssff}";
    public const string HexenSaveFileFilter = $"Hexen save files ({hxsff})|{hxsff}";
    public const string StrifeSaveFileFilter = $"Strife save file ({ssff})|{ssff}";
    public const string AllSavesFileFilter = $"All compatible save files ({dsff};{hssff});{hxsff};{ssff})|{dsff};{hssff});{hxsff};{ssff}";

    public const string ExecutableFilesFilter = "Executable file (*.exe)|*.exe";
    public const string AllFilesFilter = "All files (*.*)|*.*";
}
