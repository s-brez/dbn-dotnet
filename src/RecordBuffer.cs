using System.Runtime.CompilerServices;

namespace Dbn.Net;

public readonly unsafe ref struct RecordBuffer
{
    private readonly byte* _ptr;
    private readonly int _len;

    internal RecordBuffer(byte* ptr, nuint len)
    {
        _ptr = ptr;
        _len = checked((int)len);
    }

    public ReadOnlySpan<byte> Bytes => new ReadOnlySpan<byte>(_ptr, _len);

    public ref readonly T As<T>() where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() > _len)
        {
            throw new ArgumentException($"Buffer too small for {typeof(T)}");
        }

        return ref Unsafe.AsRef<T>(_ptr);
    }
}
