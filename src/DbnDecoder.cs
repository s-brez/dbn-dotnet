using System.Text;

namespace Dbn.Net;

public sealed unsafe class DbnDecoder : IDisposable
{
    private readonly DecoderSafeHandle _handle;
    private MetadataSafeHandle? _metaHandle;
    private MetadataC* _metaPtr;

    private DbnDecoder(DecoderSafeHandle h) => _handle = h;

    public static DbnDecoder Open(string path)
    {
        byte[] utf8 = Encoding.UTF8.GetBytes(path + '\0');
        fixed (byte* p = utf8)
        {
            var ptr = DbnFfi.dbn_decoder_open(p);
            if (ptr is null) DbnException.Throw<object>();
            return new DbnDecoder(new DecoderSafeHandle((nint)ptr));
        }
    }

    public MetadataView Metadata
    {
        get
        {
            if (_metaPtr == null)
            {
                var p = DbnFfi.dbn_decoder_metadata(
                            (DecoderOpaque*)_handle.DangerousGetHandle());
                if (p is null) DbnException.Throw<object>();
                _metaHandle = new MetadataSafeHandle((nint)p);
                _metaPtr = p;
            }
            return new MetadataView(_metaPtr);
        }
    }

    public bool TryNext(out RecordBuffer buf, out RType rtype)
    {
        byte* ptr; nuint len; byte rt;
        bool ok = DbnFfi.dbn_decoder_next(
                      (DecoderOpaque*)_handle.DangerousGetHandle(),
                      &ptr, &len, &rt);

        if (!ok)
        {
            buf = default;
            rtype = 0;
            byte* err;
            if (DbnFfi.dbn_last_error(&err))
            {
                DbnFfi.dbn_string_free(err);
                DbnException.Throw<object>();
            }
            
            return false;
        }

        buf = new RecordBuffer(ptr, len);
        rtype = (RType)rt;
        
        return true;
    }

    public void Dispose()
    {
        _metaHandle?.Dispose();
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
