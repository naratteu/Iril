#nullable enable
using System;
using System.Runtime.InteropServices;

namespace StdLib
{
    [DllExport]
    public static unsafe class PonyHelloRuntime
    {
        [DllExport("@pony_init")]
        public static int Init(int argc, byte* argv) => 0;

        [DllExport("@pony_ctx")]
        public static byte* Context() => (byte*)1;

        [DllExport("@pony_create")]
        public static byte* Create(byte* ctx, byte* descriptor, byte hasPending)
        {
            var actor = Allocate(512);
            *(byte**)actor = descriptor;
            return actor;
        }

        [DllExport("@pony_alloc")]
        public static byte* Alloc(byte* ctx, long size) => Allocate(size);

        [DllExport("@pony_alloc_small")]
        public static byte* AllocSmall(byte* ctx, int size) => Allocate(512);

        [DllExport("@pony_alloc_msg")]
        public static byte* AllocMessage(int size, int id)
        {
            var message = Allocate(Math.Max(size, 64));
            *(int*)(message + 4) = id;
            return message;
        }

        [DllExport("@pony_realloc")]
        public static byte* Realloc(byte* ctx, byte* ptr, long size, long oldSize) => Allocate(size);

        [DllExport("@ponyint_become")]
        public static void Become(byte* ctx, byte* actor) { }

        [DllExport("@pony_gc_send")]
        public static void GcSend(byte* ctx, byte* value) { }

        [DllExport("@pony_gc_recv")]
        public static void GcRecv(byte* ctx) { }

        [DllExport("@pony_send_done")]
        public static void SendDone(byte* ctx) { }

        [DllExport("@pony_recv_done")]
        public static void RecvDone(byte* ctx) { }

        [DllExport("@pony_trace")]
        public static void Trace(byte* ctx, byte* value) { }

        [DllExport("@pony_traceknown")]
        public static void TraceKnown(byte* ctx, byte* value, byte* descriptor, int mutability) { }

        [DllExport("@pony_traceunknown")]
        public static void TraceUnknown(byte* ctx, byte* value, int mutability) { }

        [DllExport("@pony_start")]
        public static byte Start(byte* scheduler, byte* detached) => 1;

        [DllExport("@pony_get_exitcode")]
        public static int ExitCode() => 0;

        [DllExport("@pony_os_stdout_setup")]
        public static void StdoutSetup() { }

        [DllExport("@pony_os_stdin_setup")]
        public static byte StdinSetup() => 0;

        [DllExport("@pony_os_stdin_read")]
        public static long StdinRead(byte* buffer, long size) => 0;

        [DllExport("@pony_asio_event_destroy")]
        public static void DestroyEvent(byte* value) { }

        [DllExport("@pony_asio_event_unsubscribe")]
        public static void UnsubscribeEvent(byte* value) { }

        [DllExport("@pony_os_stdout")]
        public static byte* Stdout() => null;

        [DllExport("@pony_os_stderr")]
        public static byte* Stderr() => null;

        [DllExport("@pony_os_std_print")]
        public static void Print(byte* stream, byte* text, long length)
        {
            for (var i = 0L; i < length; i++)
                Console.Write((char)text[i]);
        }

        [DllExport("@llvm.ctlz.i64")]
        public static long CountLeadingZeros(long value, byte isUndefined)
        {
            ulong bits = (ulong)value;
            long count = 0;
            while (bits != 0) { bits >>= 1; count++; }
            return 64 - count;
        }

        [DllExport("@llvm.umin.i64")]
        public static long UnsignedMin(long left, long right) => (ulong)left < (ulong)right ? left : right;

        [DllExport("@llvm.uadd.sat.i64")]
        public static long UnsignedAddSaturating(long left, long right)
        {
            ulong sum = (ulong)left + (ulong)right;
            return (long)(sum < (ulong)left ? ulong.MaxValue : sum);
        }

        [DllExport("@pony_hello_allocate")]
        static byte* Allocate(long size) => (byte*)Marshal.AllocHGlobal(checked((IntPtr)Math.Max(size, 1)));
    }
}
