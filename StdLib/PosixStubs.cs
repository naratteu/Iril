#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace StdLib
{
    [DllExport]
    public static unsafe class CLibExt
    {
        // ─── Math ─────────────────────────────────────────────────────────────────

        [DllExport("@atan2")]
        public static double atan2(double y, double x) => Math.Atan2(y, x);

        [DllExport("@cos")]
        public static double cos(double x) => Math.Cos(x);

        [DllExport("@exp")]
        public static double exp(double x) => Math.Exp(x);

        [DllExport("@sin")]
        public static double sin(double x) => Math.Sin(x);

        [DllExport("@sqrt")]
        public static double sqrt(double x) => Math.Sqrt(x);

        [DllExport("@log")]
        public static double log(double x) => Math.Log(x);

        [DllExport("@nan")]
        public static double nan(byte* tagp) => double.NaN;

        // ─── errno ────────────────────────────────────────────────────────────────

        static readonly GCHandle _errnoHandle;
        static readonly byte* _errnoPtr;

        static CLibExt()
        {
            var buf = new byte[8];
            _errnoHandle = GCHandle.Alloc(buf, GCHandleType.Pinned);
            _errnoPtr = (byte*)_errnoHandle.AddrOfPinnedObject();

            var sbuf = new byte[256];
            _strerrorHandle = GCHandle.Alloc(sbuf, GCHandleType.Pinned);
            _strerrorPtr = (byte*)_strerrorHandle.AddrOfPinnedObject();

            var rbuf = new byte[4096];
            _realpathHandle = GCHandle.Alloc(rbuf, GCHandleType.Pinned);
            _realpathPtr = (byte*)_realpathHandle.AddrOfPinnedObject();
        }

        [DllExport("@__errno_location")]
        public static byte* errno_location() => _errnoPtr;

        // ─── strtod ───────────────────────────────────────────────────────────────

        [DllExport("@strtod")]
        public static double strtod(byte* nptr, byte** endptr)
        {
            if (nptr == null) { if (endptr != null) *endptr = nptr; return 0; }
            int len = 0;
            while (nptr[len] != 0) len++;
            var s = Encoding.ASCII.GetString(nptr, len).Trim();
            if (double.TryParse(s, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double d))
            {
                if (endptr != null) *endptr = nptr + len;
                return d;
            }
            if (endptr != null) *endptr = nptr;
            return 0;
        }

        // ─── strerror ─────────────────────────────────────────────────────────────

        static readonly GCHandle _strerrorHandle;
        static readonly byte* _strerrorPtr;

        [DllExport("@strerror")]
        public static byte* strerror(int errnum)
        {
            var msg = Encoding.ASCII.GetBytes("Error " + errnum.ToString());
            int n = Math.Min(msg.Length, 255);
            for (int i = 0; i < n; i++) _strerrorPtr[i] = msg[i];
            _strerrorPtr[n] = 0;
            return _strerrorPtr;
        }

        // ─── strncpy / strcpy ─────────────────────────────────────────────────────

        [DllExport("@strncpy")]
        public static byte* strncpy(byte* dst, byte* src, long n)
        {
            byte* ret = dst;
            long i = 0;
            while (i < n && *src != 0) { *dst++ = *src++; i++; }
            while (i < n) { *dst++ = 0; i++; }
            return ret;
        }

        [DllExport("@strcpy")]
        public static byte* strcpy(byte* dst, byte* src)
        {
            byte* ret = dst;
            while ((*dst++ = *src++) != 0) { }
            return ret;
        }

        // ─── realpath ─────────────────────────────────────────────────────────────

        static readonly GCHandle _realpathHandle;
        static readonly byte* _realpathPtr;

        [DllExport("@realpath")]
        public static byte* realpath(byte* path, byte* resolved)
        {
            if (path == null) return null;
            int len = 0;
            while (path[len] != 0) len++;
            var s = Encoding.UTF8.GetString(path, len);
            try
            {
                var full = System.IO.Path.GetFullPath(s);
                var bytes = Encoding.UTF8.GetBytes(full);
                byte* dst = resolved != null ? resolved : _realpathPtr;
                int copyLen = Math.Min(bytes.Length, 4095);
                for (int i = 0; i < copyLen; i++) dst[i] = bytes[i];
                dst[copyLen] = 0;
                return dst;
            }
            catch { return null; }
        }

        // ─── sprintf / vsprintf ───────────────────────────────────────────────────

        [DllExport("@sprintf")]
        public static int sprintf(byte* str, byte* format, params object[] arguments)
            => vsprintf(str, format, arguments);

        // Unwrap va_list: reads slot index from va_list memory, returns real object[] args
        static unsafe object[] UnwrapVaList(object[] arguments)
        {
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr && vaPtr != IntPtr.Zero)
            {
                try
                {
                    int slot = *(int*)(void*)vaPtr;
                    if (slot >= 0 && slot < 256)
                    {
                        var realArgs = Llvm._vaSlots[slot];
                        if (realArgs != null) return realArgs;
                    }
                }
                catch { }
            }
            return arguments;
        }

        [DllExport("@ferror")]
        public static unsafe int ferror(byte* fp)
        {
            if (fp == null) return 0;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            return (s >= 1 && s < 256 && _fError[s]) ? 1 : 0;
        }

        [DllExport("@vsprintf")]
        public static unsafe int vsprintf(byte* str, byte* format, params object[] arguments)
        {
            // Inline va_list unwrap
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr && vaPtr != IntPtr.Zero)
            {
                try
                {
                    int slot_v = *(int*)(void*)vaPtr;
                    if (slot_v >= 0 && slot_v < 256)
                    {
                        var realArgsSV = Llvm._vaSlots[slot_v];
                        if (realArgsSV != null) arguments = realArgsSV;
                    }
                }
                catch { }
            }
            int fmtLen = 0;
            while (format[fmtLen] != 0) fmtLen++;
            var fmt = Encoding.ASCII.GetString(format, fmtLen);

            var sb = new StringBuilder();
            int argIdx = 0;
            int i = 0;
            while (i < fmt.Length)
            {
                if (fmt[i] != '%') { sb.Append(fmt[i++]); continue; }
                i++; // skip %
                if (i >= fmt.Length) break;
                if (fmt[i] == '%') { sb.Append('%'); i++; continue; }

                // flags
                while (i < fmt.Length && "+-0 #".IndexOf(fmt[i]) >= 0) i++;
                // width
                int width = 0;
                while (i < fmt.Length && char.IsDigit(fmt[i]))
                    width = width * 10 + (fmt[i++] - '0');
                // precision
                int prec = -1;
                if (i < fmt.Length && fmt[i] == '.')
                {
                    i++; prec = 0;
                    while (i < fmt.Length && char.IsDigit(fmt[i]))
                        prec = prec * 10 + (fmt[i++] - '0');
                }
                // length modifier
                while (i < fmt.Length && "hlLqzjt".IndexOf(fmt[i]) >= 0) i++;

                if (i >= fmt.Length) break;
                char conv = fmt[i++];

                object? arg = argIdx < arguments.Length ? arguments[argIdx++] : null;
                string piece;
                // Use if/else instead of switch to avoid IL switch instruction
                // which Iril's assembly importer doesn't support
                if (conv == 'd' || conv == 'i')
                    piece = Convert.ToInt64(arg ?? 0).ToString();
                else if (conv == 'u')
                    piece = Convert.ToUInt64(arg ?? 0).ToString();
                else if (conv == 'f')
                {
                    double dv = Convert.ToDouble(arg ?? 0.0);
                    int p2 = prec < 0 ? 6 : prec;
                    piece = dv.ToString("F" + p2, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (conv == 'g' || conv == 'G')
                {
                    double dv = Convert.ToDouble(arg ?? 0.0);
                    int p2 = prec <= 0 ? 6 : prec;
                    piece = dv.ToString("G" + p2, System.Globalization.CultureInfo.InvariantCulture);
                    if (conv == 'g') piece = piece.ToLowerInvariant();
                }
                else if (conv == 'e' || conv == 'E')
                {
                    double dv = Convert.ToDouble(arg ?? 0.0);
                    int p2 = prec < 0 ? 6 : prec;
                    piece = dv.ToString("E" + p2, System.Globalization.CultureInfo.InvariantCulture);
                    if (conv == 'e') piece = piece.ToLowerInvariant();
                }
                else if (conv == 's')
                {
                    if (arg is IntPtr ip)
                        piece = Marshal.PtrToStringAnsi(ip) ?? "(null)";
                    else if (arg is long lp)
                        piece = Marshal.PtrToStringAnsi((IntPtr)lp) ?? "(null)";
                    else
                        piece = arg?.ToString() ?? "(null)";
                }
                else if (conv == 'c')
                    piece = ((char)Convert.ToInt32(arg ?? 0)).ToString();
                else if (conv == 'x')
                    piece = Convert.ToInt64(arg ?? 0).ToString("x");
                else if (conv == 'X')
                    piece = Convert.ToInt64(arg ?? 0).ToString("X");
                else if (conv == 'o')
                    piece = Convert.ToString(Convert.ToInt64(arg ?? 0), 8);
                else if (conv == 'p')
                    piece = "0x" + Convert.ToInt64(arg ?? 0).ToString("x");
                else
                    piece = conv.ToString();
                sb.Append(piece);
            }

            var result = Encoding.ASCII.GetBytes(sb.ToString());
            for (int j = 0; j < result.Length; j++) str[j] = result[j];
            str[result.Length] = 0;
            return result.Length;
        }

        // ─── IO / printf stubs ─────────────────────────────────────────────────────────────

        [DllExport("@fflush")]
        public static int fflush(byte* stream) => 0;

        // Full printf-style format+print: called by Syscalls-generated vfprintf
        [DllExport("@_iril_format_write")]
        public static unsafe int FormatWrite(object[] arguments, byte* format)
        {
            if (format == null) return 0;
            // Unwrap va_list first
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr && vaPtr != IntPtr.Zero)
            {
                try
                {
                    int slot_v = *(int*)(void*)vaPtr;
                    if (slot_v >= 0 && slot_v < 256)
                    {
                        var realArgsSV = Llvm._vaSlots[slot_v];
                        if (realArgsSV != null) arguments = realArgsSV;
                    }
                }
                catch { }
            }
            int fLen = 0; while (format[fLen] != 0) fLen++;
            var fmt = Encoding.ASCII.GetString(format, fLen);
            var sb = new System.Text.StringBuilder();
            int ai = 0, fi = 0;
            while (fi < fmt.Length)
            {
                if (fmt[fi] != '%') { sb.Append(fmt[fi++]); continue; }
                fi++; if (fi >= fmt.Length) break;
                if (fmt[fi] == '%') { sb.Append('%'); fi++; continue; }
                while (fi < fmt.Length && "+-0 #".IndexOf(fmt[fi]) >= 0) fi++;
                while (fi < fmt.Length && char.IsDigit(fmt[fi])) fi++;
                int prec = -1;
                if (fi < fmt.Length && fmt[fi] == '.') { fi++; prec = 0; while (fi < fmt.Length && char.IsDigit(fmt[fi])) prec = prec * 10 + (fmt[fi++] - '0'); }
                while (fi < fmt.Length && "hlLqzjt".IndexOf(fmt[fi]) >= 0) fi++;
                if (fi >= fmt.Length) break;
                char conv = fmt[fi++];
                object arg = ai < arguments.Length ? arguments[ai++] : null;
                string piece;
                if (conv == 'd' || conv == 'i') piece = Convert.ToInt64(arg ?? 0).ToString();
                else if (conv == 'u') piece = Convert.ToUInt64(arg ?? 0).ToString();
                else if (conv == 'f') { double dv = Convert.ToDouble(arg ?? 0.0); piece = dv.ToString("F" + (prec < 0 ? 6 : prec), System.Globalization.CultureInfo.InvariantCulture); }
                else if (conv == 'g' || conv == 'G') { double dv = Convert.ToDouble(arg ?? 0.0); int p2 = prec <= 0 ? 6 : prec; piece = dv.ToString("G" + p2, System.Globalization.CultureInfo.InvariantCulture); if (conv == 'g') piece = piece.ToLowerInvariant(); }
                else if (conv == 'e' || conv == 'E') { double dv = Convert.ToDouble(arg ?? 0.0); piece = dv.ToString("E" + (prec < 0 ? 6 : prec), System.Globalization.CultureInfo.InvariantCulture); if (conv == 'e') piece = piece.ToLowerInvariant(); }
                else if (conv == 's')
                {
                    if (arg is IntPtr ip) piece = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ip) ?? "";
                    else if (arg is long lp) piece = System.Runtime.InteropServices.Marshal.PtrToStringAnsi((IntPtr)lp) ?? "";
                    else piece = arg?.ToString() ?? "";
                }
                else if (conv == 'c') piece = ((char)Convert.ToInt32(arg ?? 0)).ToString();
                else if (conv == 'x') piece = Convert.ToInt64(arg ?? 0).ToString("x");
                else if (conv == 'X') piece = Convert.ToInt64(arg ?? 0).ToString("X");
                else piece = conv.ToString();
                sb.Append(piece);
            }
            var result = sb.ToString();
            Console.Write(result);
            return result.Length;
        }

        // Resolves varargs: if single IntPtr pointing to a GCHandle, unwraps to original object[]
        // This is called from Syscalls-generated vfprintf to handle va_list passing
        [DllExport("@_iril_resolve_varargs")]
        public static unsafe object[] ResolveVarArgs(object[] arguments)
        {
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr && vaPtr != IntPtr.Zero)
            {
                try
                {
                    int slot_v = *(int*)(void*)vaPtr;
                    if (slot_v >= 0 && slot_v < 256)
                    {
                        var realArgsSV = Llvm._vaSlots[slot_v];
                        if (realArgsSV != null) return realArgsSV;
                    }
                }
                catch { }
            }
            return arguments;
        }

        // vfprintf internal implementation (not exported - Syscalls owns @vfprintf symbol)
        // Called internally only, NOT via DllExport to avoid symbol conflict
        public static unsafe int vfprintf_impl(byte* stream, byte* format, params object[] arguments)
        {
            // Unwrap va_list if arguments is a single IntPtr pointing to a GCHandle
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr && vaPtr != IntPtr.Zero)
            {
                try
                {
                    int slot_v = *(int*)(void*)vaPtr;
                    if (slot_v >= 0 && slot_v < 256)
                    {
                        var realArgsSV = Llvm._vaSlots[slot_v];
                        if (realArgsSV != null) arguments = realArgsSV;
                    }
                }
                catch { }
            }
            // Format string to string and print
            if (format == null) return 0;
            int fmtLen = 0; while (format[fmtLen] != 0) fmtLen++;
            var fmt = Encoding.ASCII.GetString(format, fmtLen);
            var sb = new System.Text.StringBuilder();
            int argIdx = 0, fi = 0;
            while (fi < fmt.Length)
            {
                if (fmt[fi] != '%') { sb.Append(fmt[fi++]); continue; }
                fi++;
                if (fi >= fmt.Length) break;
                if (fmt[fi] == '%') { sb.Append('%'); fi++; continue; }
                while (fi < fmt.Length && "+-0 #".IndexOf(fmt[fi]) >= 0) fi++;
                while (fi < fmt.Length && char.IsDigit(fmt[fi])) fi++;
                int prec = -1;
                if (fi < fmt.Length && fmt[fi] == '.')
                {
                    fi++; prec = 0;
                    while (fi < fmt.Length && char.IsDigit(fmt[fi])) prec = prec * 10 + (fmt[fi++] - '0');
                }
                while (fi < fmt.Length && "hlLqzjt".IndexOf(fmt[fi]) >= 0) fi++;
                if (fi >= fmt.Length) break;
                char conv = fmt[fi++];
                object arg = argIdx < arguments.Length ? arguments[argIdx++] : null;
                string piece;
                if (conv == 'd' || conv == 'i') piece = Convert.ToInt64(arg ?? 0).ToString();
                else if (conv == 'u') piece = Convert.ToUInt64(arg ?? 0).ToString();
                else if (conv == 'f') { double dv = Convert.ToDouble(arg ?? 0.0); piece = dv.ToString("F" + (prec < 0 ? 6 : prec), System.Globalization.CultureInfo.InvariantCulture); }
                else if (conv == 'g' || conv == 'G') { double dv = Convert.ToDouble(arg ?? 0.0); int p2 = prec <= 0 ? 6 : prec; piece = dv.ToString("G" + p2, System.Globalization.CultureInfo.InvariantCulture); if (conv == 'g') piece = piece.ToLowerInvariant(); }
                else if (conv == 'e' || conv == 'E') { double dv = Convert.ToDouble(arg ?? 0.0); piece = dv.ToString("E" + (prec < 0 ? 6 : prec), System.Globalization.CultureInfo.InvariantCulture); if (conv == 'e') piece = piece.ToLowerInvariant(); }
                else if (conv == 's')
                {
                    if (arg is IntPtr ip) piece = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ip) ?? "";
                    else if (arg is long lp) piece = System.Runtime.InteropServices.Marshal.PtrToStringAnsi((IntPtr)lp) ?? "";
                    else piece = arg?.ToString() ?? "";
                }
                else if (conv == 'c') piece = ((char)Convert.ToInt32(arg ?? 0)).ToString();
                else if (conv == 'x') piece = Convert.ToInt64(arg ?? 0).ToString("x");
                else if (conv == 'X') piece = Convert.ToInt64(arg ?? 0).ToString("X");
                else piece = conv.ToString();
                sb.Append(piece);
            }
            var result = sb.ToString();
            Console.Write(result);
            return result.Length;
        }

        // fprintf - duplicate of vfprintf to avoid cross-method import issues
        [DllExport("@fprintf")]
        public static unsafe int fprintf(byte* stream, byte* format, params object[] arguments)
        {
            if (arguments.Length == 1 && arguments[0] is IntPtr vaPtr2 && vaPtr2 != IntPtr.Zero)
            {
                try
                {
                    int slot_v = *(int*)(void*)vaPtr2;
                    if (slot_v >= 0 && slot_v < 256)
                    {
                        var realArgsSV = Llvm._vaSlots[slot_v];
                        if (realArgsSV != null) arguments = realArgsSV;
                    }
                }
                catch { }
            }
            if (format == null) return 0;
            int fmtLen2 = 0; while (format[fmtLen2] != 0) fmtLen2++;
            var fmt2 = Encoding.ASCII.GetString(format, fmtLen2);
            var sb2 = new System.Text.StringBuilder();
            int argIdx2 = 0, fi2 = 0;
            while (fi2 < fmt2.Length)
            {
                if (fmt2[fi2] != '%') { sb2.Append(fmt2[fi2++]); continue; }
                fi2++; if (fi2 >= fmt2.Length) break;
                if (fmt2[fi2] == '%') { sb2.Append('%'); fi2++; continue; }
                while (fi2 < fmt2.Length && "+-0 #".IndexOf(fmt2[fi2]) >= 0) fi2++;
                while (fi2 < fmt2.Length && char.IsDigit(fmt2[fi2])) fi2++;
                int prec2 = -1;
                if (fi2 < fmt2.Length && fmt2[fi2] == '.') { fi2++; prec2 = 0; while (fi2 < fmt2.Length && char.IsDigit(fmt2[fi2])) prec2 = prec2 * 10 + (fmt2[fi2++] - '0'); }
                while (fi2 < fmt2.Length && "hlLqzjt".IndexOf(fmt2[fi2]) >= 0) fi2++;
                if (fi2 >= fmt2.Length) break;
                char conv2 = fmt2[fi2++];
                object arg2 = argIdx2 < arguments.Length ? arguments[argIdx2++] : null;
                string piece2;
                if (conv2 == 'd' || conv2 == 'i') piece2 = Convert.ToInt64(arg2 ?? 0).ToString();
                else if (conv2 == 'u') piece2 = Convert.ToUInt64(arg2 ?? 0).ToString();
                else if (conv2 == 'f') { double dv2 = Convert.ToDouble(arg2 ?? 0.0); piece2 = dv2.ToString("F" + (prec2 < 0 ? 6 : prec2), System.Globalization.CultureInfo.InvariantCulture); }
                else if (conv2 == 'g' || conv2 == 'G') { double dv2 = Convert.ToDouble(arg2 ?? 0.0); int p2 = prec2 <= 0 ? 6 : prec2; piece2 = dv2.ToString("G" + p2, System.Globalization.CultureInfo.InvariantCulture); if (conv2 == 'g') piece2 = piece2.ToLowerInvariant(); }
                else if (conv2 == 'e' || conv2 == 'E') { double dv2 = Convert.ToDouble(arg2 ?? 0.0); piece2 = dv2.ToString("E" + (prec2 < 0 ? 6 : prec2), System.Globalization.CultureInfo.InvariantCulture); if (conv2 == 'e') piece2 = piece2.ToLowerInvariant(); }
                else if (conv2 == 's')
                {
                    if (arg2 is IntPtr ip2) piece2 = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ip2) ?? "";
                    else if (arg2 is long lp2) piece2 = System.Runtime.InteropServices.Marshal.PtrToStringAnsi((IntPtr)lp2) ?? "";
                    else piece2 = arg2?.ToString() ?? "";
                }
                else if (conv2 == 'c') piece2 = ((char)Convert.ToInt32(arg2 ?? 0)).ToString();
                else if (conv2 == 'x') piece2 = Convert.ToInt64(arg2 ?? 0).ToString("x");
                else if (conv2 == 'X') piece2 = Convert.ToInt64(arg2 ?? 0).ToString("X");
                else piece2 = conv2.ToString();
                sb2.Append(piece2);
            }
            var result2 = sb2.ToString();
            Console.Write(result2);
            return result2.Length;
        }

        // ─── More math ────────────────────────────────────────────────────────────

        [DllExport("@modf")]
        public static double modf(double x, double* iptr)
        {
            double intpart = Math.Truncate(x);
            if (iptr != null) *iptr = intpart;
            return x - intpart;
        }

        [DllExport("@pow")]
        public static double pow(double x, double y) => Math.Pow(x, y);

        // ─── More string functions ────────────────────────────────────────────────

        [DllExport("@strrchr")]
        public static byte* strrchr(byte* s, int c)
        {
            byte* last = null;
            while (*s != 0)
            {
                if (*s == (byte)c) last = s;
                s++;
            }
            if (c == 0) return s; // special case
            return last;
        }

        [DllExport("@atoi")]
        public static int atoi(byte* s)
        {
            if (s == null) return 0;
            int len = 0;
            while (s[len] != 0) len++;
            var str = Encoding.ASCII.GetString(s, len).Trim();
            return int.TryParse(str, out int v) ? v : 0;
        }

        [DllExport("@tolower")]
        public static int tolower(int c)
            => c >= 'A' && c <= 'Z' ? c + ('a' - 'A') : c;

        [DllExport("@strtol")]
        public static long strtol(byte* nptr, byte** endptr, int base_)
        {
            if (nptr == null) { if (endptr != null) *endptr = nptr; return 0; }
            int len = 0;
            while (nptr[len] != 0) len++;
            var s = Encoding.ASCII.GetString(nptr, len).Trim();
            if (long.TryParse(s, out long v))
            {
                if (endptr != null) *endptr = nptr + len;
                return v;
            }
            if (endptr != null) *endptr = nptr;
            return 0;
        }

        [DllExport("@putc")]
        public static int putc(int c, byte* stream)
        {
            Console.Write((char)c);
            return c;
        }

        // ─── Time ─────────────────────────────────────────────────────────────────

        [DllExport("@time")]
        public static long time(long* tloc)
        {
            var t = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (tloc != null) *tloc = t;
            return t;
        }

        // ─── POSIX stubs ──────────────────────────────────────────────────────────

        [DllExport("@wait")]
        public static int wait(int* status)
        {
            if (status != null) *status = 0;
            return -1;
        }

        // @read(fd, buf, size) - POSIX read using slot fd
        [DllExport("@read")]
        public static unsafe long read(int fd, byte* buf, long count)
        {
            if (fd < 1 || fd >= 256 || _fStreams == null || _fStreams[fd] == null) return -1;
            if (buf == null || count <= 0) return 0;
            if (_fEof[fd]) return 0;
            try
            {
                var tmp = new byte[count];
                int n = _fStreams[fd].Read(tmp, 0, (int)count);
                if (n == 0) { _fEof[fd] = true; return 0; }
                for (int i = 0; i < n; i++) buf[i] = tmp[i];
                return n;
            }
            catch { _fError[fd] = true; return -1; }
        }

        [DllExport("@close")]
        public static unsafe int close(int fd)
        {
            if (fd < 1 || fd >= 256 || _fStreams == null || _fStreams[fd] == null) return 0;
            try { _fReaders[fd]?.Close(); _fStreams[fd]?.Close(); } catch { }
            _fStreams[fd] = null; _fReaders[fd] = null; _fEof[fd] = false; _fError[fd] = false; _fUnget[fd] = -1;
            return 0;
        }

        [DllExport("@fsync")]
        public static int fsync(int fd) => 0;

        [DllExport("@open")]
        public static unsafe int open(byte* path, int flags, params object[] mode)
        {
            // Open a file and return slot index as fd (slots 1-255)
            if (path == null) return -1;
            if (_fStreams == null) return -1;
            int plen = 0; while (path[plen] != 0) plen++;
            var pathStr = Encoding.UTF8.GetString(path, plen);
            try
            {
                System.IO.FileMode fm;
                System.IO.FileAccess fa;
                // flags: 0=O_RDONLY, 1=O_WRONLY, 2=O_RDWR, 0x40=O_CREAT, 0x200=O_TRUNC etc
                int accessFlags = flags & 3;
                if (accessFlags == 0) { fm = System.IO.FileMode.Open; fa = System.IO.FileAccess.Read; }
                else if (accessFlags == 1) { fm = System.IO.FileMode.OpenOrCreate; fa = System.IO.FileAccess.Write; }
                else { fm = System.IO.FileMode.OpenOrCreate; fa = System.IO.FileAccess.ReadWrite; }
                if ((flags & 0x200) != 0) fm = System.IO.FileMode.Create; // O_TRUNC
                var stream = new System.IO.FileStream(pathStr, fm, fa, System.IO.FileShare.ReadWrite);
                int slot = -1;
                for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
                if (slot < 0) { stream.Close(); return -1; }
                _fStreams[slot] = stream;
                // For read access, also create StreamReader for text reading
                if (fa == System.IO.FileAccess.Read)
                    _fReaders[slot] = new System.IO.StreamReader(stream, Encoding.UTF8, false);
                _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
                return slot;  // fd = slot (1-255)
            }
            catch { return -1; }
        }

        [DllExport("@dup")]
        public static int dup(int oldfd) => -1;

        [DllExport("@execle")]
        public static int execle(byte* path, byte* arg, params object[] args) => -1;

        [DllExport("@fcntl")]
        public static int fcntl(int fd, int cmd, params object[] args) => 0;

        [DllExport("@fork")]
        public static int fork() => -1;

        [DllExport("@pipe")]
        public static int pipe(int* pipefd) { if (pipefd != null) { pipefd[0] = -1; pipefd[1] = -1; } return -1; }

        [DllExport("@isatty")]
        public static int isatty(int fd) => 0;

        [DllExport("@fileno")]
        public static int fileno(byte* stream) => -1;

        // ─── File I/O using slot table (System.IO types only - importable by Iril) ──
        // Slots: FILE* handle = (byte*)(slot*8+8), slot is index into arrays below.
        // All logic is inlined to avoid calling unexported helpers.

        static readonly System.IO.Stream[] _fStreams = new System.IO.Stream[256];
        static readonly System.IO.StreamReader[] _fReaders = new System.IO.StreamReader[256];
        static readonly bool[] _fEof = new bool[256];
        static readonly bool[] _fError = new bool[256];
        static readonly int[] _fUnget = new int[256];
        static int _fNextSlot = 0;

        // Initializes stdout/stderr/stdin slots and returns their FILE* handles
        // Called from Syscalls static constructor to set up @stdout/@stderr globals
        [DllExport("@_iril_get_stdout_ptr")]
        public static unsafe byte* GetStdoutPtr()
        {
            if (_fStreams == null) return null;
            int slot = -1;
            for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
            if (slot < 0) return null;
            _fStreams[slot] = Console.OpenStandardOutput();
            _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
            return (byte*)(IntPtr)(slot * 8 + 8);
        }

        [DllExport("@_iril_get_stderr_ptr")]
        public static unsafe byte* GetStderrPtr()
        {
            if (_fStreams == null) return null;
            int slot = -1;
            for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
            if (slot < 0) return null;
            _fStreams[slot] = Console.OpenStandardError();
            _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
            return (byte*)(IntPtr)(slot * 8 + 8);
        }

        [DllExport("@_iril_get_stdin_ptr")]
        public static unsafe byte* GetStdinPtr()
        {
            if (_fStreams == null) return null;
            int slot = -1;
            for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
            if (slot < 0) return null;
            var stream = Console.OpenStandardInput();
            _fStreams[slot] = stream;
            _fReaders[slot] = new System.IO.StreamReader(stream, Encoding.UTF8, false);
            _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
            return (byte*)(IntPtr)(slot * 8 + 8);
        }

        // @tfopen is an internal function in vio_orig.c that wraps fopen.
        // We intercept it here to add diagnostics.
        [DllExport("@tfopen")]
        public static unsafe byte* tfopen(byte* path, byte* mode)
        {
            Console.Error.WriteLine("[tfopen] called!");
            return fopen(path, mode);
        }

        [DllExport("@fopen")]
        public static unsafe byte* fopen(byte* path, byte* mode)
        {
            if (path == null) return null;
            int plen = 0; while (path[plen] != 0) plen++;
            var pathStr = Encoding.UTF8.GetString(path, plen);
            int mlen = 0; if (mode != null) while (mode[mlen] != 0) mlen++;
            var modeStr = mode != null ? Encoding.ASCII.GetString(mode, mlen) : "r";
            // Guard: static fields might be null if .cctor not run
            if (_fStreams == null) return null;
            // Handle special device paths
            System.IO.Stream specialStream = null;
            if (pathStr == "/dev/stdout" || pathStr == "/dev/fd/1" || pathStr == "stdout")
                specialStream = Console.OpenStandardOutput();
            else if (pathStr == "/dev/stderr" || pathStr == "/dev/fd/2" || pathStr == "stderr")
                specialStream = Console.OpenStandardError();
            else if (pathStr == "/dev/stdin" || pathStr == "/dev/fd/0" || pathStr == "stdin")
                specialStream = Console.OpenStandardInput();
            if (specialStream != null)
            {
                int slot = -1;
                for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
                if (slot < 0) { specialStream.Dispose(); return null; }
                _fStreams[slot] = specialStream;
                if (modeStr.StartsWith("r"))
                    _fReaders[slot] = new System.IO.StreamReader(specialStream, Encoding.UTF8, false);
                _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
                return (byte*)(IntPtr)(slot * 8 + 8);
            }
            try
            {
                System.IO.FileMode fm; System.IO.FileAccess fa;
                if (modeStr.StartsWith("r")) { fm = System.IO.FileMode.Open; fa = System.IO.FileAccess.Read; }
                else if (modeStr.StartsWith("w")) { fm = System.IO.FileMode.Create; fa = System.IO.FileAccess.Write; }
                else if (modeStr.StartsWith("a")) { fm = System.IO.FileMode.Append; fa = System.IO.FileAccess.Write; }
                else { fm = System.IO.FileMode.Open; fa = System.IO.FileAccess.Read; }
                bool exists = System.IO.File.Exists(pathStr);
                Console.Error.WriteLine("[fopen] file exists: " + exists);
                var stream = new System.IO.FileStream(pathStr, fm, fa, System.IO.FileShare.ReadWrite);
                // Inline AllocSlot: find first free slot
                int slot = -1;
                for (int i = 1; i < 256; i++) { if (_fStreams[i] == null) { slot = i; break; } }
                if (slot < 0) { stream.Close(); return null; }
                _fStreams[slot] = stream;
                _fReaders[slot] = (fa == System.IO.FileAccess.Read) ?
                    new System.IO.StreamReader(stream, Encoding.UTF8, false) : null;
                _fEof[slot] = false; _fError[slot] = false; _fUnget[slot] = -1;
                Console.Error.WriteLine("[fopen] opened slot=" + slot);
                // Inline SlotToPtr: slot*8+8 as byte*
                return (byte*)(IntPtr)(slot * 8 + 8);
            }
            catch (Exception ex) { Console.Error.WriteLine("[fopen] EXCEPTION: " + ex.Message); return null; }
        }

        [DllExport("@fclose")]
        public static unsafe int fclose(byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;  // inline PtrToSlot
            if (s < 1 || s >= 256 || _fStreams[s] == null) return -1;
            try { _fReaders[s]?.Close(); _fStreams[s]?.Close(); } catch { }
            _fStreams[s] = null; _fReaders[s] = null; _fEof[s] = false; _fError[s] = false; _fUnget[s] = -1;
            return 0;
        }

        [DllExport("@fgets")]
        public static unsafe byte* fgets(byte* buf, int size, byte* fp)
        {
            if (buf == null || size <= 0 || fp == null) return null;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null || _fEof[s]) return null;
            try
            {
                var sb = new System.Text.StringBuilder();
                int n = 0; bool any = false;
                if (_fUnget[s] >= 0)
                {
                    sb.Append((char)_fUnget[s]); _fUnget[s] = -1; n++; any = true;
                    if (sb[0] == '\n' || n >= size - 1) goto done;
                }
                var r = _fReaders[s];
                if (r == null) return null;
                while (n < size - 1)
                {
                    int ch = r.Read(); if (ch == -1) { _fEof[s] = true; break; }
                    any = true; sb.Append((char)ch); n++; if ((char)ch == '\n') break;
                }
            done:
                if (!any) return null;
                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                int cpy = Math.Min(bytes.Length, size - 1);
                for (int i = 0; i < cpy; i++) buf[i] = bytes[i];
                buf[cpy] = 0; return buf;
            }
            catch { _fError[s] = true; return null; }
        }

        [DllExport("@fgetc")]
        public static unsafe int fgetc(byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null || _fEof[s]) return -1;
            if (_fUnget[s] >= 0) { int c = _fUnget[s]; _fUnget[s] = -1; return c; }
            try { int ch = _fReaders[s]?.Read() ?? -1; if (ch == -1) _fEof[s] = true; return ch; }
            catch { _fError[s] = true; return -1; }
        }

        [DllExport("@getc")]
        public static unsafe int getc(byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null || _fEof[s]) return -1;
            if (_fUnget[s] >= 0) { int c = _fUnget[s]; _fUnget[s] = -1; return c; }
            try { int ch = _fReaders[s]?.Read() ?? -1; if (ch == -1) _fEof[s] = true; return ch; }
            catch { _fError[s] = true; return -1; }
        }

        [DllExport("@ungetc")]
        public static unsafe int ungetc(int c, byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null) return -1;
            _fUnget[s] = c; _fEof[s] = false; return c;
        }

        [DllExport("@feof")]
        public static unsafe int feof(byte* fp)
        {
            if (fp == null) return 1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            return (s >= 1 && s < 256 && _fStreams[s] != null && !_fEof[s]) ? 0 : 1;
        }

        [DllExport("@fseek")]
        public static unsafe int fseek(byte* fp, long offset, int whence)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null) return -1;
            try
            {
                System.IO.SeekOrigin o;
                if (whence == 0) o = System.IO.SeekOrigin.Begin;
                else if (whence == 1) o = System.IO.SeekOrigin.Current;
                else o = System.IO.SeekOrigin.End;
                _fStreams[s].Seek(offset, o); _fEof[s] = false;
                _fReaders[s]?.DiscardBufferedData(); return 0;
            }
            catch { return -1; }
        }

        [DllExport("@ftell")]
        public static unsafe long ftell(byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            try { return _fStreams[s]?.Position ?? -1; } catch { return -1; }
        }

        [DllExport("@rewind")]
        public static unsafe void rewind(byte* fp)
        {
            if (fp == null) return;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s < 1 || s >= 256 || _fStreams[s] == null) return;
            try { _fStreams[s].Seek(0, System.IO.SeekOrigin.Begin); _fEof[s] = false; _fReaders[s]?.DiscardBufferedData(); } catch { }
        }

        // @fputc - slot-aware version (overrides Syscalls @fputc)
        [DllExport("@fputc")]
        public static unsafe int fputc(int c, byte* fp)
        {
            if (fp == null) return -1;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s >= 1 && s < 256 && _fStreams != null && _fStreams[s] != null)
            {
                try { _fStreams[s].WriteByte((byte)(c & 0xFF)); return c & 0xFF; }
                catch { _fError[s] = true; return -1; }
            }
            // Fallback: write to console
            Console.Write((char)c);
            return c;
        }

        // @fwrite - slot-aware version
        [DllExport("@fwrite")]
        public static unsafe long fwrite(byte* buf, long size, long count, byte* fp)
        {
            if (buf == null || size <= 0 || count <= 0 || fp == null) return 0;
            int s = ((int)(long)(IntPtr)fp - 8) / 8;
            if (s >= 1 && s < 256 && _fStreams != null && _fStreams[s] != null)
            {
                long total = size * count;
                var tmp = new byte[total];
                for (int i = 0; i < total; i++) tmp[i] = buf[i];
                try { _fStreams[s].Write(tmp, 0, (int)total); return count; }
                catch { _fError[s] = true; return 0; }
            }
            return 0;
        }

        // @write(fd, buf, count) - POSIX write via fd
        [DllExport("@write")]
        public static unsafe long write2(int fd, byte* buf, long count)
        {
            if (fd < 0 || buf == null || count <= 0) return -1;
            if (fd == 1)
            { // stdout
                for (long i = 0; i < count; i++) Console.Write((char)buf[i]); return count;
            }
            if (fd == 2)
            { // stderr
                for (long i = 0; i < count; i++) Console.Error.Write((char)buf[i]); return count;
            }
            if (fd >= 1 && fd < 256 && _fStreams != null && _fStreams[fd] != null)
            {
                var tmp = new byte[count];
                for (int i = 0; i < count; i++) tmp[i] = buf[i];
                try { _fStreams[fd].Write(tmp, 0, (int)count); return count; }
                catch { _fError[fd] = true; return -1; }
            }
            return -1;
        }

        [DllExport("@popen")]
        public static byte* popen(byte* command, byte* mode) => null;

        [DllExport("@pclose")]
        public static int pclose(byte* fp) => -1;

        [DllExport("@fdopen")]
        public static byte* fdopen(int fd, byte* mode) => null;

        [DllExport("@setbuf")]
        public static void setbuf(byte* stream, byte* buf) { }

        [DllExport("@_exit")]
        public static void _exit(int status) => System.Environment.Exit(status);
    }

}
