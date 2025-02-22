namespace DOOMer.WinForms;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DOOMer.Core;

public static class Import {
    public static bool TryImportZdl(Configuration config, string? iniPath) {
        if (iniPath.IsNullEmptyWhitespace()) {
            iniPath = Configuration.ZdlGlobalIni;
        }

        if (!File.Exists(iniPath)) {
            return false;
        }

        bool valueRead = false;
        int currentIndex = 0;

        MultiplayerArguments? mpMode = null;

        while (Ini.TryReadKey(iniPath, "zdl.ports", $"p{currentIndex++}n", out string? valueName)
        && Ini.TryReadKey(iniPath, "zdl.ports", $"p{currentIndex - 1}f", out string? valuePath)) {
            FileInfo file = new(valuePath);
            LoadedPort port = new(valueName, file);

            if (config.Ports.Contains(port)) {
                continue;
            }

            config.Ports.Add(port);
            valueRead = true;
        }

        currentIndex = 0;
        while (Ini.TryReadKey(iniPath, "zdl.iwads", $"i{currentIndex++}n", out string? valueName)
        && Ini.TryReadKey(iniPath, "zdl.iwads", $"i{currentIndex - 1}f", out string? valuePath)) {
            FileInfo file = new(valuePath);
            LoadedInternal iwad = new(valueName, file);

            if (config.IWADs.Contains(iwad)) {
                continue;
            }

            config.IWADs.Add(iwad);
            valueRead = true;
        }

        currentIndex = 0;
        string? valuePathDisabled = null;
        while (Ini.TryReadKey(iniPath, "zdl.save", $"file{currentIndex++}", out string? valuePathEnabled)
        || Ini.TryReadKey(iniPath, "zdl.save", $"file{currentIndex - 1}d", out valuePathDisabled)) {
            if (valuePathEnabled.IsNullEmptyWhitespace()) {
                if (valuePathDisabled.IsNullEmptyWhitespace()) {
                    break;
                }

                LoadedExternal external = new(Path.GetFileName(valuePathDisabled), valuePathDisabled) {
                    Enabled = false,
                };

                if (config.Externals.Contains(external)) {
                    continue;
                }

                config.Externals.Add(external);
                valueRead = true;
                valuePathDisabled = null;
                continue;
            }

            FileInfo file = new(valuePathEnabled);
            if (file.Exists && file.Length > 0) {
                LoadedExternal external = new(file: file) {
                    Enabled = false,
                };

                if (config.Externals.Contains(external)) {
                    continue;
                }

                config.Externals.Add(external);
                valueRead = true;
                valuePathDisabled = null;
                continue;
            }

            DirectoryInfo directory = new(valuePathEnabled);
            if (directory.Exists) {
                LoadedExternal external = new(directory: directory, asGroup: false, relativePath: valuePathEnabled.IsPathRelativeCompatible());

                if (config.Externals.Contains(external)) {
                    continue;
                }

                config.Externals.Add(external);
                valueRead = true;
                valuePathDisabled = null;
            }
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "iwad", out var lastIWAD)) {
            config.LastLoadedIWAD = lastIWAD;
            valueRead = true;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "port", out var lastPort)) {
            config.LastLoadedPort = lastPort;
            valueRead = true;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "warp", out var lastMap)) {
            config.LastMap = lastMap;
            valueRead = true;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "gametype", out string? val) && int.TryParse(val, out var gameType)) {
            mpMode ??= new(gameType switch {
                1 => MultiplayerGameMode.Coop,
                2 => MultiplayerGameMode.Deathmatch,
                3 => MultiplayerGameMode.AltDeathmatch,
                _ => MultiplayerGameMode.Singleplayer,
            });
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "netmode", out val) && int.TryParse(val, out var netMode)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.NetMode = netMode switch {
                0 => MultiplayerNetMode.ClassicP2P,
                1 => MultiplayerNetMode.ClassicP2P,
                _ => MultiplayerNetMode.Default,
            };
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "host", out val)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.HostnameIp = val;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "host", out val)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.Port = val;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "extratic", out val)) {
            if (val == "1") {
                mpMode ??= new(MultiplayerGameMode.Singleplayer);
                mpMode.Extratic = true;
            }
            else if (val == "0") {
                mpMode ??= new(MultiplayerGameMode.Singleplayer);
                mpMode.Extratic = false;
            }
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "savegame", out val)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.Savegame = val;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "fraglimit", out val) && uint.TryParse(val, out uint fraglimit)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.FragLimit = fraglimit;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "timelimit", out val) && uint.TryParse(val, out uint timelimit)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.TimeLimit = timelimit;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "dmflags", out val) && uint.TryParse(val, out uint dmflags)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.DMFLAGS = dmflags;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "dmflags2", out val) && uint.TryParse(val, out uint dmflags2)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.DMFLAGS2 = dmflags2;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "dmflags3", out val) && uint.TryParse(val, out uint dmflags3)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.DMFLAGS3 = dmflags3;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "dup", out val) && uint.TryParse(val, out uint dup)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.Dup = dup.Clamp(0, 9);
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "players", out val) && uint.TryParse(val, out uint players)) {
            mpMode ??= new(MultiplayerGameMode.Singleplayer);
            mpMode.Players = players;
        }

        if (Ini.TryReadKey(iniPath, "zdl.save", "dlgmode", out val)) {
            if (val.Equals("open", StringComparison.OrdinalIgnoreCase)) {
                config.ShowMultiplayer = true;
                valueRead = true;
            }
            else if (val.Equals("closed", StringComparison.OrdinalIgnoreCase)) {
                config.ShowMultiplayer = false;
                valueRead = true;
            }
        }

        if (mpMode is not null) {
            config.MultiplayerInstance = mpMode;
            valueRead = true;
        }

        int[] ints = Ini.ReadIntArray(iniPath, "zdl.general", "windowsize");
        if (ints.Length > 1) {
            config.FormSize = new(ints[0], ints[1]);
            valueRead = true;
        }

        ints = Ini.ReadIntArray(iniPath, "zdl.general", "windowpos");
        if (ints.Length > 1) {
            config.FormLocation = new(ints[0], ints[1]);
            valueRead = true;
        }

        return valueRead;
    }
}
