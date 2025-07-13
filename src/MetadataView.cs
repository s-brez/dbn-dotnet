using System.Runtime.InteropServices;

namespace Dbn.Net;

public readonly unsafe ref struct MetadataView
{
    private readonly MetadataC* _m;

    internal MetadataView(MetadataC* raw) => _m = raw;


    public byte Version => _m->version;
    public byte Schema => _m->schema;
    public bool TsOut => _m->ts_out != 0;
    public ulong StartNs => _m->start;
    public ulong EndNs => _m->end;
    public ulong Limit => _m->limit;


    public string Dataset =>
        Marshal.PtrToStringUTF8((nint)_m->dataset, (int)_m->dataset_len)!;

    public IEnumerable<string> Symbols => Utf8Slice(_m->symbols, _m->symbols_len);
    public IEnumerable<string> Partial => Utf8Slice(_m->partial, _m->partial_len);
    public IEnumerable<string> NotFound => Utf8Slice(_m->not_found, _m->not_found_len);

    private static unsafe List<string> Utf8Slice(byte** arr, nuint len)
    {
        var list = new List<string>((int)len);
        for (nuint i = 0; i < len; ++i)
            list.Add(Marshal.PtrToStringUTF8((nint)arr[i])!);
        return list;
    }
}
