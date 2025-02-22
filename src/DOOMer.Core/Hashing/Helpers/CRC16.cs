namespace HashSlingingSlasher;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public sealed class CRC16 {
    private readonly ushort Polynomial;
    private readonly ushort Seed;
    private readonly ushort[] ChecksumTable;

    public CRC16(ushort polynomial, ushort seed) {
        this.ChecksumTable = new ushort[256];

        for (ushort i = 0; i < this.ChecksumTable.Length; i++) {
            ushort value = 0;
            ushort temp = i;
            for (byte x = 0; x < 8; x++) {
                if (((value ^ temp) & 0x0001) != 0) {
                    value = (ushort)((value >> 1) ^ polynomial);
                }
                else {
                    value >>= 1;
                }
                temp >>= 1;
            }
            this.ChecksumTable[i] = value;
        }

        this.Polynomial = polynomial;
        this.Seed = seed;
    }

    public static CRC16 Maxim { get; } = new(polynomial: 0xA001, seed: 0x00);
    public static CRC16 CCITT { get; } = new(polynomial: 0x1021, seed: 0x1D0F);

    public byte[] HashData(Stream source) {
        ushort result = this.Seed;

        int current;
        while ((current = source.ReadByte()) != -1) {
            byte index = (byte)(result ^ (byte)current);
            result = (ushort)((result >> 8) ^ this.ChecksumTable[index]);
        }

        byte[] hash = BitConverter.GetBytes(~result);
        Array.Reverse(hash);
        return hash;
    }
    public async ValueTask<byte[]> HashDataAsync(Stream source, CancellationToken cancellationToken = default) {
        ushort result = this.Seed;

        byte[] buffer = new byte[2048];
        int read;
        while ((read = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0) {
            for (int i = 0; i < read; i++) {
                byte index = (byte)(result ^ buffer[i]);
                result = (ushort)((result >> 8) ^ this.ChecksumTable[index]);
            }
            if (cancellationToken.IsCancellationRequested) {
                throw new TaskCanceledException("The CRC16 computation has been cancelled.");
            }
        }

        buffer = BitConverter.GetBytes(~result);
        Array.Reverse(buffer);
        return buffer;
    }
    public ushort HashDataUshort(Stream source) {
        Span<byte> bytes = this.HashData(source);
        return BitConverter.ToUInt16(bytes);
    }
    public async ValueTask<ushort> HashDataUshortAsync(Stream source, CancellationToken cancellationToken = default) {
        byte[] bytes = await this.HashDataAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt16(bytes);
    }

    public byte[] HashData(byte[] source) {
        ushort result = Seed;

        for (int i = 0; i < source.Length; i++) {
            byte index = (byte)(result ^ source[i]);
            result = (ushort)((result >> 8) ^ this.ChecksumTable[index]);
        }

        return BitConverter.GetBytes(~result);
    }
    public async ValueTask<byte[]> HashDataAsync(byte[] source, CancellationToken cancellationToken = default) {
        return await Task.Run(() => {
            ushort result = Seed;

            int current = 0;
            while (current < source.Length) {
                int ExpectedLength = Math.Min(source.Length - current, 2048);
                for (int i = current; i < ExpectedLength; i++) {
                    byte index = (byte)(result ^ source[i]);
                    result = (ushort)((result >> 8) ^ this.ChecksumTable[index]);
                }
                if (ExpectedLength < 2048) {
                    break;
                }
                if (cancellationToken.IsCancellationRequested) {
                    throw new TaskCanceledException("The CRC16 computation has been cancelled.");
                }
                current += 2048;
            }

            return BitConverter.GetBytes(~result);
        });
    }
    public ushort HashDataUshort(byte[] source) {
        source = this.HashData(source);
        return BitConverter.ToUInt16(source);
    }
    public async ValueTask<ushort> HashDataUshortAsync(byte[] source, CancellationToken cancellationToken = default) {
        source = await this.HashDataAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt16(source);
    }

    public byte[] HashData(ReadOnlySpan<byte> source) {
        ushort result = Seed;

        int current = 0;
        while (current < source.Length) {
            int ExpectedLength = Math.Min(source.Length - current, 2048);
            for (int i = current; i < ExpectedLength; i++) {
                byte index = (byte)(result ^ source[i]);
                result = (ushort)((result >> 8) ^ this.ChecksumTable[index]);
            }
            if (ExpectedLength < 2048) {
                break;
            }
            current += 2048;
        }

        return BitConverter.GetBytes(~result);
    }
    public ushort HashDataUshort(ReadOnlySpan<byte> source) {
        source = this.HashData(source);
        return BitConverter.ToUInt16(source);
    }

    public override bool Equals(object? obj) {
        return obj is CRC16 other && this.Polynomial == other.Polynomial && this.Seed == other.Seed;
    }
    public override int GetHashCode() {
        return this.Seed << 16 | Polynomial;
    }
    public override string ToString() {
        return $"CRC-16 {{ Polynomial: {this.Polynomial}, Seed: {this.Seed} }}";
    }
}