namespace HashSlingingSlasher;

using System;
using System.Buffers;

internal ref struct ValueStringBuilder {
    private char[]? _arrayToReturnToPool;
    private Span<char> _chars;
    private int _pos;

    public ValueStringBuilder(Span<char> initialBuffer) {
        _arrayToReturnToPool = null;
        _chars = initialBuffer;
        _pos = 0;
    }

    public ValueStringBuilder(int initialCapacity) {
        _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
        _chars = _arrayToReturnToPool;
        _pos = 0;
    }
    public void Append(char value) {
        if (_pos > _chars.Length - 1) {
            this.Grow(1);
        }
        _chars[_pos++] = value;
    }
    public void Append(ReadOnlySpan<char> value) {
        int pos = _pos;
        if (pos > _chars.Length - value.Length) {
            this.Grow(value.Length);
        }

        value.CopyTo(_chars[_pos..]);
        _pos += value.Length;
    }
    private void Grow(int additionalCapacityBeyondPos) {
        // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative
        char[] poolArray = ArrayPool<char>.Shared.Rent((int)Math.Max((uint)(_pos + additionalCapacityBeyondPos), (uint)_chars.Length * 2));

        _chars[.._pos].CopyTo(poolArray);

        char[]? toReturn = _arrayToReturnToPool;
        _chars = _arrayToReturnToPool = poolArray;
        if (toReturn != null) {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    public override readonly string ToString() {
        return _chars[.._pos].ToString();
        //string s = _chars[.._pos].ToString();
        //Dispose();
        //return s;
    }
    public void Dispose() {
        char[]? toReturn = _arrayToReturnToPool;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        if (toReturn != null) {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}