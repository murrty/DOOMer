namespace DOOMer.Core;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;

public sealed class DoomArguments : ArgumentBase {
    [JsonIgnore]
    public LoadedPort? Port { get; set { if (field != value) { field = value; this.Modified = true; } } }
    [JsonIgnore]
    public LoadedInternal? IWad { get; set { if (field != value) { field = value; this.Modified = true; } } } // -iwad iwad[.wad;.iwad;.ipk3;...]
    [JsonIgnore]
    public IReadOnlyList<LoadedExternal> ExternalFiles { get; set { if (field != value) { field = value; this.Modified = true; } } } // External files wad[.wad;.pwad;.pk3;.deh;.bex;...]
    [JsonIgnore]
    public IReadOnlyList<string>? ExternalPWADs { get; set { if (field != value) { field = value; this.Modified = true; } } }
    [JsonIgnore]
    public IReadOnlyList<string>? ExternalDehacks { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("multiplayer")]
    public MultiplayerArguments? Multiplayer { get; set { if (field != value) { field = value; this.Modified = true; } } } // Instance defined
    [JsonPropertyName("command_line_args")]
    public string? CommandLineArgs { get; set { if (field != value) { field = value; this.Modified = true; } } } // User defined
    [JsonPropertyName("skill_level")]
    public int SkillLevel { get; set { if (field != value) { field = value; this.Modified = true; } } } // -skill 1-5
    [JsonPropertyName("map")]
    public string? Map { get; set { if (field != value) { field = value; this.Modified = true; } } } // -map map
    [JsonPropertyName("map_xlat")]
    public string? MapTranslator { get; set { if (field != value) { field = value; this.Modified = true; } } } // -xlat file

    // Other commands
    [JsonPropertyName("width")]
    public int Width { get; set { if (field != value) { field = value; this.Modified = true; } } } // -width x
    [JsonPropertyName("height")]
    public int Height { get; set { if (field != value) { field = value; this.Modified = true; } } } // -height y
    [JsonPropertyName("blockmap")]
    public bool GenerateBlockMap { get; set { if (field != value) { field = value; this.Modified = true; } } } // -blockmap
    [JsonPropertyName("config_file")]
    public string? ConfigFile { get; set { if (field != value) { field = value; this.Modified = true; } } } // -config cfgfile
    [JsonPropertyName("disable_cd_audio")]
    public bool DisableCdAudio { get; set { if (field != value) { field = value; this.Modified = true; } } } // -disablecdaudio
    [JsonPropertyName("disable_idle_priority")]
    public bool DisableIdlePriority { get; set { if (field != value) { field = value; this.Modified = true; } } } // -noidle
    [JsonPropertyName("disable_joystick")]
    public bool DisableJoystick { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nojoy
    [JsonPropertyName("disable_music")]
    public bool DisableMusic { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nomusic
    [JsonPropertyName("disable_sound_effects")]
    public bool DisableSoundEffects { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nosfx
    [JsonPropertyName("disable_sound")]
    public bool DisableSound { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nosound
    [JsonPropertyName("disable_startup_screens")]
    public bool DisableStartupScreens { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nostartup
    [JsonPropertyName("use_old_sprites")]
    public bool UseOldSprites { get; set { if (field != value) { field = value; this.Modified = true; } } } // -oldsprites
    [JsonPropertyName("disable_auto_load")]
    public bool DisableAutoLoad { get; set { if (field != value) { field = value; this.Modified = true; } } } // -noautoload
    [JsonPropertyName("disable_auto_exec")]
    public bool DisableAutoExec { get; set { if (field != value) { field = value; this.Modified = true; } } } // -disableautoexec
    [JsonPropertyName("demo_file")]
    public string? DemoFile { get; set { if (field != value) { field = value; this.Modified = true; } } } // -record/-playdemo/-timedemo demofile[.lmp]
    [JsonPropertyName("record_demo")]
    public bool RecordDemo { get; set { if (field != value) { field = value; this.Modified = true; } } } // Uses '-record demofile[.lmp]' if true, otherwise '-playdemo demofile[.lmp]'
    [JsonPropertyName("fast_demo")]
    public bool FasterDemoPlayback { get; set { if (field != value) { field = value; this.Modified = true; } } }// Uses -timedemo demofile[.lmp] if 'RecordDemo' is false.
    [JsonPropertyName("player_class")]
    public string? PlayerClass { get; set { if (field != value) { field = value; this.Modified = true; } } } // +playerclass class

    // Play options
    [JsonPropertyName("fast_mosters")]
    public bool FastMonsters { get; set { if (field != value) { field = value; this.Modified = true; } } } // -fast
    [JsonPropertyName("no_monsters")]
    public bool NoMonsters { get; set { if (field != value) { field = value; this.Modified = true; } } } // -nomonsters
    [JsonPropertyName("monsters_respawn")]
    public bool MonstersRespawn { get; set { if (field != value) { field = value; this.Modified = true; } } } // -respawn
    [JsonPropertyName("time_limit")]
    public int TimeLimit { get; set { if (field != value) { field = value; this.Modified = true; } } } // -timer x
    [JsonPropertyName("turbo")]
    public int Turbo { get; set { if (field != value) { field = value; this.Modified = true; } } } // -turbo x, 100 = normal, 255 = max

    public DoomArguments(LoadedPort? port, LoadedInternal? iwad, List<LoadedExternal> externalFiles) : this(port, iwad, externalFiles, null, null, 0, null) { }
    public DoomArguments(LoadedPort? port, LoadedInternal? iwad, List<LoadedExternal> externalFiles, MultiplayerArguments? multiplayer, string? commandLineArgs, int skill, string? map) {
        this.Port = port;
        this.IWad = iwad;
        this.ExternalFiles = externalFiles;
        this.Multiplayer = (multiplayer is not null && multiplayer.GameMode == MultiplayerGameMode.Singleplayer) ? null : multiplayer;
        this.CommandLineArgs = commandLineArgs.IsNullEmptyWhitespace() ? null : commandLineArgs.Trim();
        this.SkillLevel = skill.Clamp(0, 5);
        this.Map = map.IsNullEmptyWhitespace() ? null : map.Trim();
    }

    public const string DeHackFileFlag = "-deh";
    public const string PWADFileFlag = "-file";

    public static string GetFileFlag(string extension) {
        return InternalWad.IsAnyDehack(extension) ?
            DeHackFileFlag : PWADFileFlag;
    }

    public override void RebuildArguments() {
        this.Clear();
        this.generate();
    }

    private void generate() {
        #region Main arguments
        // Add iwad argument.
        if (this.IWad?.FileExists == true) {
            this.Add(argument: $"-iwad \"{this.IWad.File.FullName}\"");
        }

        // Add skill level argument.
        if (this.SkillLevel > 0) {
            this.Add(argument: "-skill " + this.SkillLevel);
        }

        // Local mthod used to load a wad into the lists.
        void LoadWad(LoadedExternal wad) {
            // If the wad isn't enabled, return.
            if (!wad.Enabled) {
                return;
            }

            // Check if the external item is a group.
            if (wad.IsGroup) {
                // If so, just load the dependants.
                wad.Dependants.For(LoadWad);
                return;
            }

            // CHeck if the external item is a file.
            if (wad.IsFile) {
                // If so, make sure the wad file truly exists.
                if (!wad.FileExists) {
                    return;
                }

                // Load each dependant wad.
                wad.Dependants.For(LoadWad);

                // Add it to the files to load.
                this.AddOrMergeFlag(flag: GetFileFlag(wad.File.Extension), argument: $"\"{wad.File.FullName}\"");
                return;
            }

            // Check if the external item is a directory.
            if (wad.IsDirectory) {
                // If so, make sure the directory truly exists.
                if (!wad.DirectoryExists) {
                    return;
                }

                // Load each dependant wad.
                wad.Dependants.For(LoadWad);

                // Check if the directory is loaded along side the file command.
                if (wad.DirectoryAsFile) {
                    this.AddOrMergeFlag(flag: "-file", argument: $"\"{wad.Directory.FullName}\"");
                    return;
                }

                // Otherwise, we enumerate the files.
                foreach (var info in wad.Directory.EnumerateFiles("*", SearchOption.AllDirectories)) {
                    // Check if the file actually exists.
                    if (!info.Exists || info.Length < 5) {
                        continue;
                    }

                    // Add it to the files to load.
                    this.AddOrMergeFlag(flag: GetFileFlag(info.Extension), argument: $"\"{info.FullName}\"");
                }
            }
        }

        // Load external files.
        this.ExternalFiles?.For(LoadWad);

        // Load any drag + drop pwads.
        if (this.ExternalPWADs?.Count > 0) {
            this.AddOrMergeFlagRange(PWADFileFlag, this.ExternalPWADs);
        }

        // Load any drag + drop dehacks.
        if (this.ExternalDehacks?.Count > 0) {
            this.AddOrMergeFlagRange(DeHackFileFlag, this.ExternalDehacks);
        }

        // Add the map argument.
        if (!this.Map.IsNullEmptyWhitespace()) {
            this.Add("+map " + this.Map);
        }

        // Add the map translation argument.
        if (!this.MapTranslator.IsNullEmptyWhitespace() && File.Exists(this.MapTranslator)) {
            this.Add($"-xlat \"{this.MapTranslator}\"");
        }
        #endregion Main arguments

        #region Other arguments
        // These are all pretty self explanitory.
        if (this.Width > 0) {
            this.Add("-width " + this.Width);
        }

        if (this.Height > 0) {
            this.Add("-height " + this.Height);
        }

        if (this.GenerateBlockMap) {
            this.Add("-blockmap");
        }

        if (!this.ConfigFile.IsNullEmptyWhitespace() && File.Exists(this.ConfigFile)) {
            this.Add($"-config \"{this.ConfigFile}\"");
        }

        if (this.DisableCdAudio) {
            this.Add("-nocdaudio");
        }

        // Only affected on windows.
        if (this.DisableIdlePriority) {
            this.Add("-noidle");
        }

        // Only affected on windows.
        if (this.DisableJoystick) {
            this.Add("-nojoy");
        }

        if (this.DisableMusic) {
            this.Add("-nomusic");
        }

        if (this.DisableSoundEffects) {
            this.Add("-nosfx");
        }

        if (this.DisableSound) {
            this.Add("-nosound");
        }

        if (this.DisableStartupScreens) {
            this.Add("-nostartup");
        }

        if (this.UseOldSprites) {
            this.Add("-oldsprites");
        }

        if (this.DisableAutoLoad) {
            this.Add("-noautoload");
        }

        if (this.DisableAutoExec) {
            this.Add("-disableautoexec");
        }

        if (!this.DemoFile.IsNullEmptyWhitespace() && File.Exists(this.DemoFile)) {
            if (this.RecordDemo) {
                this.Add($"-record \"{this.DemoFile}\"");
            }
            else if (this.FasterDemoPlayback) {
                this.Add($"-timedemo \"{this.DemoFile}\"");
            }
            else {
                this.Add($"-playdemo \"{this.DemoFile}\"");
            }
        }

        // Only certain games use player class.
        if (!this.PlayerClass.IsNullEmptyWhitespace()) {
            this.Add("+playerclass " + this.PlayerClass);
        }
        #endregion Other arguments

        #region Play arguments
        // These are all pretty self explanitory.
        if (this.FastMonsters) {
            this.Add("-fast");
        }

        if (this.NoMonsters) {
            this.Add("-nomonsters");
        }

        if (this.MonstersRespawn) {
            this.Add("-respawn");
        }

        if (this.TimeLimit > 0 && (this.Multiplayer?.GameMode is MultiplayerGameMode.Singleplayer or null)) {
            this.Add("-timer " + this.TimeLimit);
        }

        if (this.Turbo > 0) {
            this.Add("-turbo " + int.Min(255, this.Turbo));
        }
        #endregion Play arguments

        // Generate multiplayer arguments, if necessary.
        if (this.Multiplayer?.GameMode is not null and not MultiplayerGameMode.Singleplayer) {
            this.Add(this.Multiplayer.GetArgumentString());
        }

        // Add user specified arguments, if any.
        if (!this.CommandLineArgs.IsNullEmptyWhitespace()) {
            this.Add(this.CommandLineArgs.Trim());
        }
    }

    public Process? Launch() {
        if (this.Port is null) {
            return null;
        }

        return this.LaunchWith(port: this.Port);
    }
    public Process? LaunchWith(LoadedPort port) {
        if (!port.FileExists) {
            return null;
        }

        return this.LaunchInternal(portPath: port.Path,
            workingDirectory: port.WorkingDirectory);
    }
    public Process? LaunchWith(FileInfo file) {
        if (!file.PathExists()) {
            return null;
        }

        string? workingDirectory = Path.GetDirectoryName(file.FullName);

        if (workingDirectory.IsNullEmptyWhitespace()) {
            return null;
        }

        return this.LaunchInternal(portPath: file.FullName,
            workingDirectory: workingDirectory);
    }
    public Process? LaunchWith(string filePath) {
        if (!File.Exists(filePath)) {
            return null;
        }

        string? workingDirectory = Path.GetDirectoryName(filePath);

        if (workingDirectory.IsNullEmptyWhitespace() || !Directory.Exists(workingDirectory)) {
            return null;
        }

        return this.LaunchInternal(portPath: filePath,
            workingDirectory: workingDirectory);
    }
    private Process? LaunchInternal(string portPath, string workingDirectory) {
        Process process = new() {
            StartInfo = new(portPath) {
                Arguments = this.GetArgumentString().UnlessNullEmptyWhitespace(string.Empty),
                CreateNoWindow = false,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
            },
            EnableRaisingEvents = true,
        };

        if (!process.Start()) {
            process.Dispose();
            return null;
        }

        process.Exited += (_,_) => process.Dispose();
        return process;
    }
}
