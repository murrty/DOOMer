namespace HashSlingingSlasher;

using System;

[Serializable]
public sealed class StreamReadEventArgs(long CurrentPosition, long Length) : EventArgs {
    public long CurrentPosition { get; } = CurrentPosition;
    public long Length { get; } = Length;
    public float Percentage { get; } = Length > 0 ? ((100f * CurrentPosition) / Length) : float.PositiveInfinity;
}