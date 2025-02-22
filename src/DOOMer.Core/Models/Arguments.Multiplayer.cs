namespace DOOMer.Core;

using System.IO;
using System.Text.Json.Serialization;

public sealed class MultiplayerArguments : ArgumentBase {
    [JsonPropertyName("game_mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MultiplayerGameMode GameMode { get; set { if (field != value) { field = value; this.Modified = true; } } }
    [JsonPropertyName("net_mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MultiplayerNetMode NetMode { get; set { if (field != value) { field = value; this.Modified = true; } } } // -netmode <0/1>
    [JsonPropertyName("players")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint Players { get; set { if (field != value) { field = value; this.Modified = true; } } } // -host playercount

    [JsonPropertyName("host")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? HostnameIp { get; set { if (field != value) { field = value; this.Modified = true; } } } // -join hostname/ip[:port]
    [JsonPropertyName("port")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Port { get; set { if (field != value) { field = value; this.Modified = true; } } } // -port port

    [JsonPropertyName("frag_limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint FragLimit { get; set { if (field != value) { field = value; this.Modified = true; } } }
    [JsonPropertyName("time_limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint TimeLimit { get; set { if (field != value) { field = value; this.Modified = true; } } } // +set timelimit 0-
    [JsonPropertyName("dwflags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint DMFLAGS { get; set { if (field != value) { field = value; this.Modified = true; } } } // +set dmflags 0-2,147,483,467
    [JsonPropertyName("dwflags2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint DMFLAGS2 { get; set { if (field != value) { field = value; this.Modified = true; } } } // +set dmflags2 0-2,147,483,467
    [JsonPropertyName("dwflags3")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint DMFLAGS3 { get; set { if (field != value) { field = value; this.Modified = true; } } } // +set dmflags2 0-2,147,483,467

    [JsonPropertyName("dup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint Dup { get; set { if (field != value) { field = value; this.Modified = true; } } } // -dup 0-9
    [JsonPropertyName("extratic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Extratic { get; set { if (field != value) { field = value; this.Modified = true; } } } // -extratic
    [JsonPropertyName("save_game")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Savegame { get; set { if (field != value) { field = value; this.Modified = true; } } } // -loadgame "file"

    public MultiplayerArguments(MultiplayerGameMode gameMode) {
        this.GameMode = gameMode;
    }

    public override void RebuildArguments() {
        this.Clear();
        this.generate();
    }
    private void generate() {
        switch (this.GameMode) {
            case MultiplayerGameMode.Deathmatch:
                this.Add("-deathmatch");
                break;

            case MultiplayerGameMode.AltDeathmatch:
                this.Add("-altdeath");
                break;

            case MultiplayerGameMode.Coop: break;

            default:
                this.Clear();
                return;
        }

        if (this.Players > 0) {
            this.Add(argument: "-host " + this.Players);
            if (!this.Port.IsNullEmptyWhitespace()) {
                this.Add(argument: "-port " + this.Port);
            }
        }
        else if (!this.HostnameIp.IsNullEmptyWhitespace()) {
            if (!this.Port.IsNullEmptyWhitespace()) {
                this.Add(argument: "-join " + this.HostnameIp.Trim().Replace(" ", "%20") + ':' + this.Port);
            }
            else {
                this.Add(argument: "-join " + this.HostnameIp.Trim().Replace(" ", "%20"));
            }
        }
        else {
            this.Clear();
            return;
        }

        if (this.DMFLAGS > 0) {
            this.Add(argument: "+set dmflags " + this.DMFLAGS);
        }

        if (this.DMFLAGS2 > 0) {
            this.Add(argument: "+set dmflags2 " + this.DMFLAGS2);
        }

        if (this.DMFLAGS3 > 0) {
            this.Add(argument: "+set dmflags3 " + this.DMFLAGS3);
        }

        if (this.FragLimit > 0) {
            this.Add(argument: "+set fraglimit " + this.FragLimit);
        }

        if (this.TimeLimit > 0) {
            this.Add(argument: "+set timelimit " + this.TimeLimit);
        }

        if (this.Extratic) {
            this.Add(argument: "-extratic");
        }

        switch (this.NetMode) {
            case MultiplayerNetMode.ClassicP2P: {
                this.Add(argument: "-netmode 0");
            } break;
            case MultiplayerNetMode.ClientServer: {
                this.Add(argument: "-netmode 1");
            } break;
        }

        if (this.Dup > 0) {
            this.Add(argument: "-dup " + this.Dup);
        }

        if (!this.Savegame.IsNullEmptyWhitespace()) {
            FileInfo file = new(this.Savegame);
            if (file.Exists) {
                this.Add(argument: $"-loadgame \"{file.FullName}\"");
            }
        }

        if (this.Count < 1) {
            this.Clear();
        }
    }

    public bool IsConfigDefault() {
        return this.GameMode == default
            && this.NetMode == default
            && this.Players == default
            && this.HostnameIp == default
            && this.Port == default
            && this.FragLimit == default
            && this.TimeLimit == default
            && this.DMFLAGS == default
            && this.DMFLAGS2 == default
            && this.DMFLAGS3 == default
            && this.Dup == default
            && this.Extratic == default
            && this.Savegame == default;
    }
    public bool ConfigsMatch(MultiplayerArguments? other) {
        if (other is null) {
            return this is null;
        }

        if (this is null) {
            return false;
        }

        return this.GameMode == other.GameMode
            && this.NetMode == other.NetMode
            && this.Players == other.Players
            && this.HostnameIp == other.HostnameIp
            && this.Port == other.Port
            && this.FragLimit == other.FragLimit
            && this.TimeLimit == other.TimeLimit
            && this.DMFLAGS == other.DMFLAGS
            && this.DMFLAGS2 == other.DMFLAGS2
            && this.DMFLAGS3 == other.DMFLAGS3
            && this.Dup == other.Dup
            && this.Extratic == other.Extratic
            && this.Savegame == other.Savegame;
    }
}
