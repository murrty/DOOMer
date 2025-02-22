namespace HashSlingingSlasher;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

public sealed class CRC32 : HashAlgorithm {
    private const uint DefaultPolynomial = 0xEDB88320u;
    private const uint DefaultSeed = 0xFFFFFFFFu;

    private static readonly uint[] DefaultChecksumTable;

    new public static int HashSize => 4;

    public uint Polynomial { get; }
    public uint Seed { get; }
    public uint Xor { get; }

    private readonly uint[] ChecksumTable;

    static CRC32() {
        DefaultChecksumTable = new uint[256];

        for (uint i = 0; i < 256; i++) {
            uint entry = i;
            for (int x = 0; x < 8; x++) {
                entry = ((entry & 1) != 0) ? (DefaultPolynomial ^ (entry >> 1)) : (entry >> 1);
            }
            DefaultChecksumTable[i] = entry;
        }
    }

    public CRC32() : this(DefaultPolynomial, DefaultSeed) { }
    public CRC32(uint polynomial) : this(polynomial, DefaultSeed) { }
    public CRC32(uint polynomial, uint seed) {
        this.Polynomial = polynomial;
        this.Seed = seed;

        if (polynomial != DefaultPolynomial) {
            this.ChecksumTable = new uint[256];

            for (uint i = 0; i < 256; i++) {
                uint entry = i;
                for (int x = 0; x < 8; x++) {
                    entry = ((entry & 1) != 0) ? (polynomial ^ (entry >> 1)) : (entry >> 1);
                }
                this.ChecksumTable[i] = entry;
            }
        }
        else {
            this.ChecksumTable = DefaultChecksumTable;
        }
    }

    #region Instanced
    new public byte[] ComputeHash(Stream source) {
        uint result = this.Seed;

        int current;
        while ((current = source.ReadByte()) != -1) {
            result = this.ChecksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);
        }

        byte[] hash = BitConverter.GetBytes(~result);
        Array.Reverse(hash);
        return hash;
    }
    new public async ValueTask<byte[]> ComputeHashAsync(Stream source, CancellationToken cancellationToken = default) {
        uint result = this.Seed;

        byte[] buffer = new byte[2048];
        int read;
        while ((read = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0) {
            for (int i = 0; i < read; i++) {
                result = this.ChecksumTable[(result & 0xFF) ^ buffer[i]] ^ (result >> 8);
            }
        }

        buffer = BitConverter.GetBytes(~result);
        Array.Reverse(buffer);
        return buffer;
    }
    public uint ComputeHashUint(Stream source) {
        Span<byte> bytes = this.ComputeHash(source);
        return BitConverter.ToUInt32(bytes);
    }
    public async ValueTask<uint> ComputeHashUintAsync(Stream source, CancellationToken cancellationToken = default) {
        byte[] bytes = await this.ComputeHashAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt32(bytes);
    }

    new public byte[] ComputeHash(byte[] source) {
        uint result = this.Seed;

        int current = 0;
        while (current < source.Length) {
            int ExpectedLength = Math.Min(source.Length - current, 2048);
            for (int i = current; i < ExpectedLength; i++) {
                result = this.ChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
            }
            if (ExpectedLength < 2048) {
                break;
            }
            current += 2048;
        }

        return BitConverter.GetBytes(~result);
    }
    public async ValueTask<byte[]> ComputeHashAsync(byte[] source, CancellationToken cancellationToken = default) {
        return await Task.Run(() => {
            uint result = this.Seed;

            int current = 0;
            while (current < source.Length) {
                int ExpectedLength = Math.Min(source.Length - current, 2048);
                for (int i = current; i < ExpectedLength; i++) {
                    result = this.ChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
                }
                if (ExpectedLength < 2048) {
                    break;
                }
                if (cancellationToken.IsCancellationRequested) {
                    throw new TaskCanceledException("The CRC32 computation has stopped.");
                }
                current += 2048;
            }

            return BitConverter.GetBytes(~result);
        });
    }
    public uint ComputeHashUint(byte[] source) {
        source = this.ComputeHash(source);
        return BitConverter.ToUInt32(source);
    }
    public async ValueTask<uint> ComputeHashUintAsync(byte[] source, CancellationToken cancellationToken = default) {
        source = await this.ComputeHashAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt32(source);
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> source) {
        uint result = this.Seed;

        int current = 0;
        while (current < source.Length) {
            int ExpectedLength = Math.Min(source.Length - current, 2048);
            for (int i = current; i < ExpectedLength; i++) {
                result = this.ChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
            }
            if (ExpectedLength < 2048) {
                break;
            }
            current += 2048;
        }

        return BitConverter.GetBytes(~result);
    }
    public uint ComputeHashUint(ReadOnlySpan<byte> source) {
        source = this.ComputeHash(source);
        return BitConverter.ToUInt32(source);
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize) { }
    protected override byte[] HashFinal() => [ ];
    public override void Initialize() { }
    #endregion

    #region Static
    public static byte[] HashData(Stream source) {
        uint result = DefaultSeed;

        int current;
        while ((current = source.ReadByte()) != -1) {
            result = DefaultChecksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);
        }

        byte[] hash = BitConverter.GetBytes(~result);
        Array.Reverse(hash);
        return hash;
    }
    public static async ValueTask<byte[]> HashDataAsync(Stream source, CancellationToken cancellationToken = default) {
        uint result = DefaultSeed;

        byte[] buffer = new byte[2048];
        int read;
        while ((read = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0) {
            for (int i = 0; i < read; i++) {
                result = DefaultChecksumTable[(result & 0xFF) ^ buffer[i]] ^ (result >> 8);
            }
            if (cancellationToken.IsCancellationRequested) {
                throw new TaskCanceledException("The CRC32 computation has been cancelled.");
            }
        }

        buffer = BitConverter.GetBytes(~result);
        Array.Reverse(buffer);
        return buffer;
    }
    public static uint HashDataUint(Stream source) {
        Span<byte> bytes = HashData(source);
        return BitConverter.ToUInt32(bytes);
    }
    public static async ValueTask<uint> HashDataUintAsync(Stream source, CancellationToken cancellationToken = default) {
        byte[] bytes = await HashDataAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt32(bytes);
    }

    public static byte[] HashData(byte[] source) {
        uint result = DefaultSeed;

        for (int i = 0; i < source.Length; i++) {
            result = DefaultChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
        }

        return BitConverter.GetBytes(~result);
    }
    public static async ValueTask<byte[]> HashDataAsync(byte[] source, CancellationToken cancellationToken = default) {
        return await Task.Run(() => {
            uint result = DefaultSeed;

            int current = 0;
            while (current < source.Length) {
                int ExpectedLength = Math.Min(source.Length - current, 2048);
                for (int i = current; i < ExpectedLength; i++) {
                    result = DefaultChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
                }
                if (ExpectedLength < 2048) {
                    break;
                }
                if (cancellationToken.IsCancellationRequested) {
                    throw new TaskCanceledException("The CRC32 computation has been cancelled.");
                }
                current += 2048;
            }

            return BitConverter.GetBytes(~result);
        });
    }
    public static uint HashDataUint(byte[] source) {
        source = HashData(source);
        return BitConverter.ToUInt32(source);
    }
    public static async ValueTask<uint> HashDataUintAsync(byte[] source, CancellationToken cancellationToken = default) {
        source = await HashDataAsync(source, cancellationToken).ConfigureAwait(false);
        return BitConverter.ToUInt32(source);
    }

    public static byte[] HashData(ReadOnlySpan<byte> source) {
        uint result = DefaultSeed;
        for (int i = 0; i < source.Length; i++) {
            result = DefaultChecksumTable[(result & 0xFF) ^ source[i]] ^ (result >> 8);
        }
        return BitConverter.GetBytes(~result);
    }
    public static uint HashDataUint(ReadOnlySpan<byte> source) {
        source = HashData(source);
        return BitConverter.ToUInt32(source);
    }
    #endregion
}