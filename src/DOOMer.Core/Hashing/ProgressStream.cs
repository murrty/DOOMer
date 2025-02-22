namespace HashSlingingSlasher;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal sealed class ProgressStream : Stream {
    internal const int ReportTime = 1000;

    private readonly Stream ParentStream;
    private readonly Timer ReadProgressReportTimer;

    public event EventHandler<StreamReadEventArgs>? StreamReadProgress;
    private long BytesRead;

    public bool DisposeParent { get; set; }

    public ProgressStream(Stream ParentStream) : this(ParentStream, false) { }
    public ProgressStream(Stream ParentStream, bool DisposeParent) {
        this.ParentStream = ParentStream;
        ReadProgressReportTimer = new((_) => StreamReadProgress?.Invoke(this, new StreamReadEventArgs(BytesRead, ParentStream.Length)), null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        this.DisposeParent = DisposeParent;
    }

    public override bool CanRead => ParentStream.CanRead;
    public override bool CanSeek => ParentStream.CanSeek;
    public override bool CanWrite => ParentStream.CanWrite;
    public override long Length => ParentStream.Length;
    public override bool CanTimeout => ParentStream.CanTimeout;
    public override long Position {
        get => ParentStream.Position;
        set => ParentStream.Position = value;
    }
    public override int WriteTimeout {
        get => ParentStream.WriteTimeout;
        set => ParentStream.WriteTimeout = value;
    }
    public override int ReadTimeout {
        get => ParentStream.ReadTimeout;
        set => ParentStream.ReadTimeout = value;
    }

    public void StartReadProgressTimer() {
        ReadProgressReportTimer.Change(ReportTime, ReportTime);
    }
    public void StopReadProgressTimer() {
        ReadProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    public override void Flush() {
        ParentStream.Flush();
    }
    public override Task FlushAsync(CancellationToken cancellationToken) {
        return ParentStream.FlushAsync(cancellationToken);
    }
    public override long Seek(long offset, SeekOrigin origin) {
        return ParentStream.Seek(offset, origin);
    }
    public override void SetLength(long value) {
        ParentStream.SetLength(value);
    }

    public override int Read(byte[] buffer, int offset, int count) {
        int BytesRead = ParentStream.Read(buffer, offset, count);
        this.BytesRead += BytesRead;
        return BytesRead;
        //this.BytesRead = 0;
        //long BytesRead;

        //ProgressReportTimer.Change(ReportTime, ReportTime);
        //while ((BytesRead = ParentStream.Read(buffer, offset, count)) != 0) {
        //    this.BytesRead += BytesRead;
        //}
        //ProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        //return (int)BytesRead;
    }
    public override int Read(Span<byte> buffer) {
        int BytesRead = ParentStream.Read(buffer);
        this.BytesRead += BytesRead;
        return BytesRead;
        //this.BytesRead = 0;
        //long BytesRead;

        //ProgressReportTimer.Change(ReportTime, ReportTime);
        //while ((BytesRead = ParentStream.Read(buffer)) != 0) {
        //    this.BytesRead += BytesRead;
        //}
        //ProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        //return (int)BytesRead;
    }
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
        int BytesRead = await ParentStream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).ConfigureAwait(false);
        this.BytesRead += BytesRead;
        return BytesRead;
        //this.BytesRead = 0;
        //int BytesRead;

        //ProgressReportTimer.Change(ReportTime, ReportTime);
        //while ((BytesRead = await ParentStream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).ConfigureAwait(false)) != 0) {
        //    this.BytesRead += BytesRead;
        //}
        //ProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        //return BytesRead;
    }
    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) {
        int BytesRead = await ParentStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
        this.BytesRead += BytesRead;
        return BytesRead;
        //this.BytesRead = 0;
        //int BytesRead;

        //ProgressReportTimer.Change(ReportTime, ReportTime);
        //while ((BytesRead = await ParentStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0) {
        //    this.BytesRead += BytesRead;
        //}
        //ProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        //return BytesRead;
    }
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) {
        return ParentStream.BeginRead(buffer, offset, count, callback, state);
    }

    public override void WriteByte(byte value) {
        ParentStream.WriteByte(value);
    }
    public override void Write(byte[] buffer, int offset, int count) {
        ParentStream.Write(buffer, offset, count);
    }
    public override void Write(ReadOnlySpan<byte> buffer) {
        ParentStream.Write(buffer);
    }
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
        return ParentStream.WriteAsync(buffer, offset, count, cancellationToken);
    }
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) {
        return ParentStream.WriteAsync(buffer, cancellationToken);
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) {
        return ParentStream.BeginWrite(buffer, offset, count, callback, state);
    }
    public override void Close() {
        ParentStream.Close();
    }
    public override void CopyTo(Stream destination, int bufferSize) {
        ParentStream.CopyTo(destination, bufferSize);
    }
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) {
        return ParentStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }
    public override int EndRead(IAsyncResult asyncResult) {
        return ParentStream.EndRead(asyncResult);
    }
    public override void EndWrite(IAsyncResult asyncResult) {
        ParentStream.EndWrite(asyncResult);
    }
    public override bool Equals(object? obj) {
        return ParentStream.Equals(obj);
    }
    public override int GetHashCode() {
        return ParentStream.GetHashCode();
    }
    public override int ReadByte() {
        return ParentStream.ReadByte();
    }
    public override string? ToString() {
        return ParentStream.ToString();
    }

    protected override void Dispose(bool disposing) {
        if (disposing) {
            if (this.DisposeParent) {
                ParentStream.Dispose();
            }
            ReadProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            ReadProgressReportTimer.Dispose();
        }
    }
    public override async ValueTask DisposeAsync() {
        if (this.DisposeParent) {
            await ParentStream.DisposeAsync();
        }
        await Task.Run(() => {
            ReadProgressReportTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            ReadProgressReportTimer.Dispose();
        });
    }
}