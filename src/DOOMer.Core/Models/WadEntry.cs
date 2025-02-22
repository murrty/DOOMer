namespace DOOMer.Core;

using System;
using System.Diagnostics;

[DebuggerDisplay("{Name} \\{ Offset = {Offset}, Length = {Length} \\}")]
public sealed class WadEntry {
    public int Offset { get; init; }
    public int Length { get; init; }
    public string Name { get; init; }
    public uint CRC32 { get; set; }
    public WadEntry(int offset, int length, string name, ReadOnlySpan<byte> bytes) {
        this.Offset = offset;
        this.Length = length;
        this.Name = name;
        if (length > 0 && bytes.Length > (offset + length + 1)) {
            ReadOnlySpan<byte> entryBytes = bytes[offset..(offset + length + 1)];
            byte[] hashBytes = HashSlingingSlasher.CRC32.HashData(entryBytes);
            this.CRC32 = BitConverter.ToUInt32(hashBytes, 0);
        }
    }
}
