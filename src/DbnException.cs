using System.Runtime.InteropServices;

namespace Dbn.Net;

public sealed class DbnException : Exception
{
    private static unsafe string PopLastError()
    {
        byte* ptr;
        if (!DbnFfi.dbn_last_error(&ptr))
        {
            return "unknown DBN error";
        }

        try
        {
            return Marshal.PtrToStringUTF8((nint)ptr) ?? "invalid UTF‑8";
        }
        finally
        {
            DbnFfi.dbn_string_free(ptr);
        }
    }

    internal static T Throw<T>() => throw new DbnException(PopLastError());

    private DbnException(string message) : base(message) { }
}
