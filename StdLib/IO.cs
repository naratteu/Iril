using System;
using System.Runtime.InteropServices;
using System.Text;
namespace StdLib
{
    /// <summary>
    /// Converts .NET string[] command-line args to C-style argc/argv for main().
    /// </summary>
    [DllExport]
    public static unsafe class Args
    {
        private static int _argc;
        private static byte** _argv;

        [DllExport("@_iril_args_init")]
        public static void Init(string[] args)
        {
            // argv[0] = program name, argv[1..] = actual args
            _argc = args.Length + 1;
            // Allocate argv array
            _argv = (byte**)Marshal.AllocHGlobal((_argc + 1) * IntPtr.Size);
            // argv[0] = "app"
            var name = Encoding.UTF8.GetBytes("app\0");
            var nameBuf = (byte*)Marshal.AllocHGlobal(name.Length);
            for (int i = 0; i < name.Length; i++) nameBuf[i] = name[i];
            _argv[0] = nameBuf;
            // argv[1..argc-1] = args
            for (int i = 0; i < args.Length; i++)
            {
                var bytes = Encoding.UTF8.GetBytes(args[i] + "\0");
                var buf = (byte*)Marshal.AllocHGlobal(bytes.Length);
                for (int j = 0; j < bytes.Length; j++) buf[j] = bytes[j];
                _argv[i + 1] = buf;
            }
            _argv[_argc] = null;  // null-terminate
        }

        [DllExport("@_iril_get_argc")]
        public static int GetArgc() => _argc;

        [DllExport("@_iril_get_argv")]
        public static byte** GetArgv() => _argv;
    }
    [DllExport]
    public static class IO
    {
        [DllExport("@__snprintf_chk")]
        public static unsafe int snprintf_chk(byte* s, long maxlen, int flags, long len, byte* format, params object[] arguments)
        {
            // var args = stackalloc __va_list_tag[1];
            // Llvm.va_start ((byte*)args, arguments);
            // Llvm.va_end ((byte*)args);
            throw new NotImplementedException();
        }

        [DllExport("@snprintf")]
        public static unsafe int snprintf(System.Byte* str, System.Int64 size, System.Byte* format, params System.Object[] arguments)
        {
            // var args = stackalloc __va_list_tag[1];
            // Llvm.va_start ((byte*)args, arguments);
            // Llvm.va_end ((byte*)args);
            throw new NotImplementedException();
        }
    }
}
