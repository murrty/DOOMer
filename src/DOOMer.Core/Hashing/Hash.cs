namespace HashSlingingSlasher;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

public readonly struct Hash {
    /// <summary>
    /// Gets the bytes of the hash value.
    /// </summary>
    public readonly byte[] Bytes { get; }
    /// <summary>
    /// Gets the string of the hash value.
    /// </summary>
    private readonly string HashString { get; }

    /// <summary>
    /// Initializes a new <see cref="Hash"/> value.
    /// </summary>
    /// <param name="bytes">The bytes of the hash that will represent a hash value of data.</param>
    public Hash(byte[] bytes) {
        this.Bytes = bytes;
        this.HashString = bytes.Length > 0 ? BitConverter.ToString(bytes).RemoveChar('-').ToUpperInvariant() : string.Empty;
    }
    /// <summary>
    /// Initializes a new <see cref="Hash"/> value.
    /// </summary>
    /// <param name="bytes">The bytes of the hash that will represent a hash value of data.</param>
    public Hash(Span<byte> bytes) {
        this.Bytes = bytes.ToArray();
        this.HashString = bytes.Length > 0 ? BitConverter.ToString(this.Bytes).RemoveChar('-').ToUpperInvariant() : string.Empty;
    }
    /// <summary>
    /// Initializes a new <see cref="Hash"/> value from a CRC-16 or CRC-32 value.
    /// </summary>
    /// <param name="hash">The CRC-16 hash that will represent a hash value of data.</param>
    public Hash(ushort hash) {
        this.Bytes = BitConverter.GetBytes(hash);
        this.HashString = BitConverter.ToString(this.Bytes).RemoveChar('-').ToUpperInvariant() ?? string.Empty;
    }
    /// <summary>
    /// Initializes a new <see cref="Hash"/> value from a CRC-16 or CRC-32 value.
    /// </summary>
    /// <param name="hash">The CRC-32 hash that will represent a hash value of data.</param>
    public Hash(uint hash) {
        this.Bytes = BitConverter.GetBytes(hash);
        this.HashString = BitConverter.ToString(this.Bytes).RemoveChar('-').ToUpperInvariant() ?? string.Empty;
    }

    public static Hash Calculate(FileInfo file, Func<Stream, Hash> hashCalcFunc) {
        using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        return hashCalcFunc(fileStream);
    }

    public static Hash CalculateMD5(Stream stream) {
        byte[] HashBytes = MD5.HashData(stream);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateMD5Async(Stream stream) {
        byte[] HashBytes = await MD5.HashDataAsync(stream).ConfigureAwait(false);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateMD5Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        byte[] HashBytes = await MD5.HashDataAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashBytes);
    }

    public static Hash CalculateSHA1(Stream stream) {
        byte[] HashBytes = SHA1.HashData(stream);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA1Async(Stream stream) {
        byte[] HashBytes = await SHA1.HashDataAsync(stream).ConfigureAwait(false);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA1Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        byte[] HashBytes = await SHA1.HashDataAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashBytes);
    }

    public static Hash CalculateSHA256(Stream stream) {
        byte[] HashBytes = SHA256.HashData(stream);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA256Async(Stream stream) {
        byte[] HashBytes = await SHA256.HashDataAsync(stream).ConfigureAwait(false);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA256Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        byte[] HashBytes = await SHA256.HashDataAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashBytes);
    }

    public static Hash CalculateSHA384(Stream stream) {
        byte[] HashBytes = SHA384.HashData(stream);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA384Async(Stream stream) {
        byte[] HashBytes = await SHA384.HashDataAsync(stream).ConfigureAwait(false);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA384Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        byte[] HashBytes = await SHA384.HashDataAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashBytes);
    }

    public static Hash CalculateSHA512(Stream stream) {
        byte[] HashBytes = SHA512.HashData(stream);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA512Async(Stream stream) {
        byte[] HashBytes = await SHA512.HashDataAsync(stream).ConfigureAwait(false);
        return new Hash(HashBytes);
    }
    public static async Task<Hash> CalculateSHA512Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        byte[] HashBytes = await SHA512.HashDataAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashBytes);
    }

    public static Hash CalculateCRC32(Stream stream) {
        uint HashUint = CRC32.HashDataUint(stream);
        return new Hash(HashUint);
    }
    public static async Task<Hash> CalculateCRC32Async(Stream stream) {
        uint HashUint = await CRC32.HashDataUintAsync(stream).ConfigureAwait(false);
        return new Hash(HashUint);
    }
    public static async Task<Hash> CalculateCRC32Async(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        uint HashUint = await CRC32.HashDataUintAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashUint);
    }

    public static Hash CalculateCRC16Maxim(Stream stream) {
        ushort HashUshort = CRC16.Maxim.HashDataUshort(stream);
        return new Hash(HashUshort);
    }
    public static async Task<Hash> CalculateCRC16MaximAsync(Stream stream) {
        ushort HashUshort = await CRC16.Maxim.HashDataUshortAsync(stream).ConfigureAwait(false);
        return new Hash(HashUshort);
    }
    public static async Task<Hash> CalculateCRC16MaximAsync(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        ushort HashUshort = await CRC16.Maxim.HashDataUshortAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashUshort);
    }

    public static Hash CalculateCRC16CCITT(Stream stream) {
        ushort HashUshort = CRC16.CCITT.HashDataUshort(stream);
        return new Hash(HashUshort);
    }
    public static async Task<Hash> CalculateCRC16CCITTAsync(Stream stream) {
        ushort HashUshort = await CRC16.CCITT.HashDataUshortAsync(stream).ConfigureAwait(false);
        return new Hash(HashUshort);
    }
    public static async Task<Hash> CalculateCRC16CCITTAsync(Stream stream, EventHandler<StreamReadEventArgs> ReadProgress) {
        await using ProgressStream ReadStream = new(stream);
        ReadStream.StreamReadProgress += ReadProgress;
        ReadStream.StartReadProgressTimer();
        ushort HashUshort = await CRC16.CCITT.HashDataUshortAsync(ReadStream);
        ReadStream.StopReadProgressTimer();
        ReadStream.StreamReadProgress -= ReadProgress;
        return new Hash(HashUshort);
    }

    public override readonly string ToString() {
        return this.HashString;
    }
}
