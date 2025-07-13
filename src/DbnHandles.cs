using System.Runtime.InteropServices;

namespace Dbn.Net;

internal sealed unsafe class DecoderSafeHandle : SafeHandle
{
    private DecoderSafeHandle() : base(IntPtr.Zero, ownsHandle: true) { }

    internal DecoderSafeHandle(IntPtr ptr) : this() => SetHandle(ptr);

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        DbnFfi.dbn_decoder_free((DecoderOpaque*)handle);
        return true;
    }
}

internal sealed unsafe class MetadataSafeHandle : SafeHandle
{
    private MetadataSafeHandle() : base(IntPtr.Zero, ownsHandle: true) { }

    internal MetadataSafeHandle(IntPtr ptr) : this() => SetHandle(ptr);

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        DbnFfi.dbn_metadata_free((MetadataC*)handle);
        return true;
    }
}
