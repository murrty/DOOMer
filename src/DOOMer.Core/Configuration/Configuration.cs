namespace DOOMer.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class Configuration : IJsonOnSerializing, IJsonOnSerialized, IJsonOnDeserialized {
    [JsonIgnore]
    public static string ZdlGlobalIni {
        get {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vectec Software", "qZDL.ini");
        }
    }

    [JsonIgnore]
    public static string DOOMerConfigFile {
        get {
            return Path.Join(Environment.CurrentDirectory, "doomer.json");
        }
    }

    static Configuration() {
        ReloadConfig();
    }

    [JsonIgnore]
    public static Configuration Default { get; set; }

    [JsonIgnore]
    public bool Modified { get => field || this.MultiplayerInstance?.Modified == true; private set; }

    [JsonPropertyName("iwads")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<LoadedInternal> IWADs { get; set { if (field != value) { field = value; this.Modified = true; } } } = [];

    [JsonPropertyName("ports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<LoadedPort> Ports { get; set { if (field != value) { field = value; this.Modified = true; } } } = [];

    [JsonPropertyName("external")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<LoadedExternal> Externals { get; set { if (field != value) { field = value; this.Modified = true; } } } = [];

    [JsonPropertyName("command_line_args")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> CommandLineArguments { get; set { if (field != value) { field = value; this.Modified = true; } } } = [];

    [JsonPropertyName("mutliplayer_save_files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> MultiplayerSaveFiles { get; set { if (field != value) { field = value; this.Modified = true; } } } = [];

    [JsonPropertyName("multiplayer_server")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MultiplayerArguments? MultiplayerInstance { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("show_multiplayer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ShowMultiplayer { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("enabled_pwads_launch_with_dropped_files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool EnabledPWADsLaunchWithDroppedFiles { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("last_iwad")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastLoadedIWAD { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("last_port")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastLoadedPort { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("last_map")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastMap { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("last_command_line_arg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastCommandLineArg { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("last_multiplayer_save_file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastMultiplayerSaveFile { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("form_location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(PointJsonSerializer))]
    public Point FormLocation { get; set { if (field != value) { field = value; this.Modified = true; } } }

    [JsonPropertyName("form_size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(SizeJsonSerializer))]
    public Size FormSize { get; set { if (field != value) { field = value; this.Modified = true; } } }

    private void CheckProperties() {
        if (this.LastLoadedIWAD.IsNullEmptyWhitespace()) {
            this.LastLoadedIWAD = default;
        }

        if (this.LastLoadedPort.IsNullEmptyWhitespace()) {
            this.LastLoadedPort = default;
        }

        if (this.LastMap.IsNullEmptyWhitespace()) {
            this.LastMap = default;
        }

        if (this.LastCommandLineArg.IsNullEmptyWhitespace()) {
            this.LastCommandLineArg = default;
        }
        else if (!this.CommandLineArguments.Contains(this.LastCommandLineArg)) {
            this.CommandLineArguments.Add(this.LastCommandLineArg);
        }

        if (this.LastMultiplayerSaveFile.IsNullEmptyWhitespace()) {
            this.LastMultiplayerSaveFile = default;
        }

        if (this.IWADs?.Count < 1) {
            this.IWADs = default!;
        }

        if (this.Ports?.Count < 1) {
            this.Ports = default!;
        }

        if (this.Externals?.Count < 1) {
            this.Externals = default!;
        }

        if (this.CommandLineArguments?.Count < 1) {
            this.CommandLineArguments = default!;
        }

        if (this.MultiplayerSaveFiles?.Count < 1) {
            this.MultiplayerSaveFiles = default!;
        }
    }

    [MemberNotNull(nameof(Default))]
    public static bool ReloadConfig() {
        Default ??= new();

        FileInfo file = new(DOOMerConfigFile);
        if (!file.Exists || file.Length < 3) {
            return false;
        }

        using var textStream = file.OpenText();
        string configData = textStream.ReadToEnd();
        var config = JsonSerializer.Deserialize<Configuration>(configData);

        if (config is null) {
            return false;
        }

        Default = config;
        return true;
    }
    public static void SaveConfig() {
        if (!Default.Modified) {
            return;
        }

        string filePath = DOOMerConfigFile;
        string? directory = Path.GetDirectoryName(filePath);

        if (directory.IsNullEmptyWhitespace() || !Directory.CreateDirectory(directory).Exists) {
            return;
        }

        if (Default.IsConfigDefault()) {
            File.Delete(filePath);
            return;
        }

        File.WriteAllText(filePath, JsonSerializer.Serialize(Default));
    }

    public bool IsConfigDefault() {
        return this.IWADs == default
            && this.Ports == default
            && this.Externals == default
            && this.CommandLineArguments == default
            && this.MultiplayerSaveFiles == default
            && this.MultiplayerInstance?.IsConfigDefault() != false
            && this.ShowMultiplayer == default
            && this.EnabledPWADsLaunchWithDroppedFiles == default
            && this.LastLoadedIWAD == default
            && this.LastLoadedPort == default
            && this.LastMap == default
            && this.LastCommandLineArg == default
            && this.LastMultiplayerSaveFile == default
            && this.FormLocation == default
            && this.FormSize == default;
    }
    public bool ConfigsMatch(Configuration other) {
        if (other is null) {
            return this is null;
        }

        if (this is null) {
            return false;
        }

        return this.IWADs == other.IWADs
            && this.Ports == other.Ports
            && this.Externals == other.Externals
            && this.CommandLineArguments == other.CommandLineArguments
            && this.MultiplayerSaveFiles == other.MultiplayerSaveFiles
            && (this.MultiplayerInstance == other.MultiplayerInstance || this.MultiplayerInstance?.ConfigsMatch(other.MultiplayerInstance) == true)
            && this.ShowMultiplayer == other.ShowMultiplayer
            && this.EnabledPWADsLaunchWithDroppedFiles == other.EnabledPWADsLaunchWithDroppedFiles
            && this.LastLoadedIWAD == other.LastLoadedIWAD
            && this.LastLoadedPort == other.LastLoadedPort
            && this.LastMap == other.LastMap
            && this.LastCommandLineArg == other.LastCommandLineArg
            && this.LastMultiplayerSaveFile == other.LastMultiplayerSaveFile
            && this.FormLocation == other.FormLocation
            && this.FormSize == other.FormSize;
    }

    void IJsonOnSerializing.OnSerializing() {
        if (this.MultiplayerInstance?.IsConfigDefault() == true) {
            this.MultiplayerInstance = null;
        }
        this.CheckProperties();
    }
    void IJsonOnSerialized.OnSerialized() {
        this.Modified = false;
    }
    void IJsonOnDeserialized.OnDeserialized() {
        this.CheckProperties();
        this.Modified = false;
    }
}
