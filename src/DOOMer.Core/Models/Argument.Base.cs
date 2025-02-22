namespace DOOMer.Core;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

[DebuggerDisplay("{guid} Coumt = {Count}")]
public abstract class ArgumentBase : IReadOnlyList<string> {
    Guid guid = Guid.NewGuid();
    private sealed class Argument {
        private readonly List<string> arguments = [];

        public string Flag { get; }
        public bool EmitIfEmpty { get; set; } = true;

        public Argument(string name) {
            this.Flag = name.Trim();
        }
        public Argument(string name, string argument) : this(name: name) {
            this.arguments.Add(argument);
        }
        public Argument(string name, IEnumerable<string> arguments) : this(name: name) {
            this.arguments.AddRange(arguments);
        }

        public void Add(string value) {
            this.arguments.Add(value.Trim());
        }
        public void AddRange(IEnumerable<string> values) {
            this.arguments.AddRange(values);
        }
        public void Remove(string value) {
            this.arguments.Remove(value);
        }
        public void Clear() {
            this.arguments.Clear();
        }

        public bool Equals(string flag) {
            return this.Flag.Equals(flag, StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool Equals(object? obj) {
            if (obj is string str) {
                return this.Flag.Equals(str, StringComparison.OrdinalIgnoreCase);
            }
            return obj is Argument arg && this.Flag.Equals(arg.Flag, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode() {
            return this.Flag.ToLowerInvariant().GetHashCode();
        }
        public override string ToString() {
            if (this.arguments.Count > 0) {
                return $"{this.Flag.TrimEnd()} {string.Join(' ', this.arguments).TrimStart()}".Trim();
            }
            return this.EmitIfEmpty ? this.Flag.Trim() : string.Empty;
        }
    }

    [JsonIgnore]
    private readonly List<Argument> args = [];

    [JsonIgnore]
    public int Count => this.args.Count;

    [JsonIgnore]
    public bool Modified { get; set; }

    [JsonIgnore]
    private string? argstring;

    public string this[int index] {
        get {
            return this.args[index].ToString();
        }
    }

    /// <summary>
    ///     Parses through argument settings to generate an argument string.
    /// </summary>
    public abstract void RebuildArguments();
    public string GetArgumentString() {
        if (this.Modified) {
            this.Modified = false;
            this.RebuildArguments();
            return this.argstring = string.Join(' ', this.args).Trim().UnlessNullEmptyWhitespace(string.Empty);
        }

        if (this.args.Count < 1) {
            return string.Empty;
        }

        return this.argstring ??= string.Empty;
    }

    public void Add(string argument) {
        this.args.Add(new Argument(argument));
        this.Modified = true;
    }
    public void AddRange(IEnumerable<string> arguments) {
        arguments.ForEach(this.Add);
        this.Modified = true;
    }

    public void AddFlag(string flag) {
        this.AddFlag(flag: flag, emitIfEmpty: true);
        this.Modified = true;
    }
    public void AddFlag(string flag, bool emitIfEmpty) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));

        if (flagArg is not null) {
            if (flagArg.EmitIfEmpty != emitIfEmpty) {
                flagArg.EmitIfEmpty = emitIfEmpty;
                this.Modified = true;
            }
            return;
        }

        flagArg = new(name: flag) {
            EmitIfEmpty = emitIfEmpty,
        };
        this.args.Add(flagArg);
        this.Modified = true;
    }
    public void AddFlag(string flag, string argument) {
        this.AddFlag(flag: flag, argument: argument, emitIfEmpty: true);
        this.Modified = true;
    }
    public void AddFlag(string flag, string argument, bool emitIfEmpty) {
        Argument flagArg = new(flag, argument) {
            EmitIfEmpty = emitIfEmpty,
        };
        this.args.Add(flagArg);
        this.Modified = true;
    }
    public void AddFlagRange(IEnumerable<string> flags) {
        flags.ForEach(flag => this.args.Add(new Argument(flag)));
        this.Modified = true;
    }
    public void AddFlagRange(string flag, IEnumerable<string> arguments) {
        this.args.Add(new Argument(flag, arguments));
        this.Modified = true;
    }

    public void AddOrMergeFlag(string flag, string argument) {
        this.AddOrMergeFlag(flag: flag, argument: argument, emitIfEmpty: true);
    }
    public void AddOrMergeFlag(string flag, string argument, bool emitIfEmpty) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));
        if (flagArg is not null) {
            flagArg.Add(argument);
            flagArg.EmitIfEmpty = emitIfEmpty;
            this.Modified = true;
            return;
        }

        flagArg = new(flag) {
            EmitIfEmpty = emitIfEmpty,
        };
        flagArg.Add(argument);
        this.args.Add(flagArg);
        this.Modified = true;
    }
    public void AddOrMergeFlagRange(string flag, IEnumerable<string> arguments) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));
        if (flagArg is null) {
            this.AddFlagRange(flag, arguments);
            return;
        }
        flagArg.AddRange(arguments);
        this.Modified = true;
    }

    public void AddOrReplaceFlag(string flag, string argument) {
        this.AddOrReplaceFlag(flag: flag, argument: argument, emitIfEmpty: true);
    }
    public void AddOrReplaceFlag(string flag, string argument, bool emitIfEmpty) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));
        if (flagArg is not null) {
            flagArg.Clear();
            flagArg.Add(argument);
            flagArg.EmitIfEmpty = emitIfEmpty;
            this.Modified = true;
            return;
        }

        flagArg = new(name: flag, argument: argument) {
            EmitIfEmpty = emitIfEmpty,
        };
        flagArg.Add(argument);
        this.args.Add(flagArg);
        this.Modified = true;
    }
    public void AddOrReplaceFlagRange(string flag, IEnumerable<string> arguments) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));
        if (flagArg is null) {
            this.AddFlagRange(flag, arguments);
            this.Modified = true;
            return;
        }
        flagArg.Clear();
        flagArg.AddRange(arguments);
        this.Modified = true;
    }

    public void Insert(int index, string argument) {
        this.args.Insert(index, new Argument(argument));
        this.Modified = true;
    }
    public void InsertRange(int index, IEnumerable<string> arguments) {
        foreach (var argument in arguments) {
            this.args.Insert(index++, new Argument(argument));
            this.Modified = true;
        }
    }

    public void EmitFlagIfEmpty(string flag, bool state) {
        Argument? flagArg = this.args.Find(x => x.Flag.Equals(flag));

        if (flagArg is null || flagArg.EmitIfEmpty == state) {
            return;
        }

        flagArg.EmitIfEmpty = state;
        this.Modified = true;
    }

    public bool Remove(string argument) {
        int flagArg = this.args.FindIndex(x => x.Flag.Equals(argument));

        if (flagArg < 0) {
            return false;
        }

        this.args.RemoveAt(flagArg);
        this.Modified = true;
        return true;
    }
    public void RemoveFlag(string flag) {
        for (int i = 0; i < this.args.Count; i++) {
            if (this.args[i] is not Argument existing || !existing.Equals(flag)) {
                continue;
            }

            this.args.RemoveAt(i);
            this.Modified = true;
            return;
        }
    }
    public void RemoveAt(int index) {
        this.args.RemoveAt(index);
        this.Modified = true;
    }

    public void Clear() {
        this.args.Clear();
        this.argstring = string.Empty;
    }

    public override string ToString() {
        return this.GetArgumentString();
    }
    public string[] ToArray() {
        return this.args.Count > 1 ? [..this.args.Select(x => x.ToString())] : [];
    }

    public IEnumerator<string> GetEnumerator() {
        return this.args.Select(x => x.ToString().UnlessNullEmptyWhitespace(string.Empty)).Where(x => !x.IsNullEmptyWhitespace()).GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
        return this.args.GetEnumerator();
    }
}
