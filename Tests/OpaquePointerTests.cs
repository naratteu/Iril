using System.IO;
using System.Reflection;
using NUnit.Framework;
using Iril;

namespace Tests
{
    /// <summary>
    /// Proves LLVM 15+ opaque-pointer (<c>ptr</c>) support end to end: each input
    /// is hand-written opaque-pointer IR (no clang / ilspycmd dependency) that is
    /// compiled to a .NET assembly, loaded, and executed. The asserted return
    /// value fails if opaque-pointer parsing OR codegen is wrong.
    /// </summary>
    [TestFixture]
    public class OpaquePointerTests : TestsBase
    {
        int RunCompute (string resource, string asmName)
        {
            var module = Iril.Module.Parse (GetCode (resource), resource);
            var compilation = new Compilation (new CompilationOptions (
                new[] { module }, assemblyName: asmName + ".dll"));
            compilation.Compile ();
            AssertNoErrors (compilation);

            var asmPath = Path.Combine (Path.GetTempPath (), asmName + ".dll");
            try { File.Delete (asmPath); } catch { }
            compilation.WriteAssembly (asmPath);

            var asm = Assembly.Load (File.ReadAllBytes (asmPath));
            var globals = asm.GetType (asmName + ".Globals");
            Assert.NotNull (globals, "Globals type not found");
            var compute = globals.GetMethod ("compute",
                BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull (compute, "compute() method not found");
            return (int)compute.Invoke (null, null);
        }

        [Test]
        public void AllocaStoreLoad () =>
            Assert.AreEqual (42, RunCompute ("opaque_mem.ll", "OpaqueMem"));

        [Test]
        public void GetElementPtrSourceType () =>
            Assert.AreEqual (15, RunCompute ("opaque_gep.ll", "OpaqueGep"));

        [Test]
        public void IndirectCallThroughPtr () =>
            Assert.AreEqual (42, RunCompute ("opaque_fnptr.ll", "OpaqueFnptr"));
    }
}
