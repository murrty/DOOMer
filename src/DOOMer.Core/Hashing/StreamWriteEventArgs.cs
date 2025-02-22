namespace HashSlingingSlasher;

using System;

[Serializable]
public sealed class StreamWriteEventArgs(long BytesWritten, long Length) : EventArgs {
    public long BytesWritten { get; } = BytesWritten;
    public long Length { get; } = Length;
    public float Percentage { get; } = Length > 0 ? ((100f * BytesWritten) / Length) : float.PositiveInfinity;
}