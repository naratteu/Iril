using System;
using System.IO;
using Iril;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            //
            // Start loading in the background
            //
            Library.LoadStandardLibrariesAsync();

            //
            // Inputs
            //
            var files = new List<string>();
            var extraArgs = new List<string>();
            var outName = "";
            var safeMemory = false;
            var reentrant = false;
            var showHelp = false;
            var showVersion = false;

            //
            // Parse command line
            //
            for (int i = 0; i < args.Length;)
            {
                var a = args[i];
                if (a[0] == '-')
                {
                    if (a == "-o")
                    {
                        if (i + 1 < args.Length)
                        {
                            outName = args[i + 1];
                            i += 2;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else if (a == "-h" || a == "--help" || a == "-?")
                    {
                        showHelp = true;
                        i++;
                    }
                    else if (a == "-v" || a == "--version")
                    {
                        showVersion = true;
                        i++;
                    }
                    else if (a == "--safe-memory")
                    {
                        safeMemory = true;
                        i++;
                    }
                    else if (a == "--reentrant")
                    {
                        reentrant = true;
                        i++;
                    }
                    else
                    {
                        extraArgs.Add(a);
                        i++;
                    }
                }
                else
                {
                    files.Add(a);
                    i++;
                }
            }

            if (showVersion)
            {
                var version = typeof(Program).Assembly.GetName().Version;
                Console.WriteLine($"Krueger Systems IRIL {version}");
                if (!showHelp)
                    return 0;
            }

            if (showHelp)
            {
                Console.WriteLine($"OVERVIEW: C/C++ to .NET assembly compiler by Frank A. Krueger");
                Console.WriteLine();
                Console.WriteLine($"USAGE: iril [options] <inputs>");
                Console.WriteLine();
                Console.WriteLine($"INPUTS: .c and .ll files");
                Console.WriteLine();
                Console.WriteLine($"OPTIONS:");
                Console.WriteLine($"  -h, -?, --help     Display this help");
                Console.WriteLine($"  -o <asm file>      Path to the assembly .dll to output");
                Console.WriteLine($"  --reentrant        Generate reentrant code");
                Console.WriteLine($"  --safe-memory      Verify memory accesses to make code safe from crashes");
                Console.WriteLine($"  -v, --version      Display the version");
                return 0;
            }

            //
            // Cleanup input
            //
            if (string.IsNullOrWhiteSpace(outName) && files.Count > 0)
            {
                outName = Path.ChangeExtension(Path.GetFileName(files[0]), ".dll");
            }

            //
            // Compile C Files
            //
            var clang = new ClangTool();
            var cfiles = files.Where(x => clang.InputExtensions.Contains(Path.GetExtension(x)));
            var context = new ToolContext
            {
                InputFiles = cfiles.ToArray(),
                ExtraArguments = extraArgs.ToArray(),
                OutputFile = outName,
            };
            var cllfiles = clang.Run(context);

            var llfiles = (from f in files
                           let e = Path.GetExtension(f)
                           where e == ".ll" || e == ".o"
                           select f)
                          .Concat(cllfiles)
                          .ToList();

            //
            // Early out
            //
            if (llfiles.Count == 0)
            {
                if (context.ExtraArguments.Contains("-c"))
                    return 0;
                Error("No inputs");
                return 1;
            }

            try
            {

                //
                // Parse
                //
                Info($"Parsing {llfiles.Count} files...");
                var modules = llfiles.AsParallel().Select(x =>
                {
                    var code = File.ReadAllText(x);
                    code = PreprocessLLVMIR(code);
                    return Module.Parse(code, x);
                }).ToList();

                //
                // Compile
                //
                Info("Compiling...");
                var comp = new Compilation(new CompilationOptions(modules, outName, safeMemory: safeMemory, reentrant: reentrant));
                comp.Compile();

                //
                // Show errors
                //
                var errors = (from m in modules from e in m.Errors select e)
                    .Concat(comp.Messages)
                    .OrderBy(x => x.FilePath).ThenBy(x => x.Text)
                    .ToList();
                foreach (var e in errors)
                {
                    Console.Write("iril: ");
                    if (e.Type == MessageType.Error)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("error: ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("warning: ");
                    }
                    Console.ResetColor();

                    if (!string.IsNullOrEmpty(e.FilePath))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(e.FilePath);
                        Console.Write(": ");
                        Console.ResetColor();
                    }

                    Console.WriteLine(e.Text);

                    if (!string.IsNullOrEmpty(e.Surrounding))
                    {
                        Console.WriteLine(e.Surrounding);
                    }
#if DEBUG
                    if (e.Exception != null)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(e.Exception);
                        Console.ResetColor();
                    }
#endif
                }

                //
                // Output
                //
                Info($"Writing {outName}...");
                comp.WriteAssembly(outName);

                if (errors.Count > 0)
                {
                    Info($"{errors.Count(x => x.Type == MessageType.Error)} errors, {errors.Count(x => x.Type == MessageType.Warning)} warnings");
                }

                return modules.Any(m => m.HasErrors) ? 3 : 0;
            }
            catch (Exception ex)
            {
                Error(ex.ToString());
                return 2;
            }
        }

        /// <summary>
        /// Preprocesses LLVM IR generated by clang-14 to be compatible with Iril's older parser.
        /// - Replaces 'fneg' (LLVM 10+) with equivalent 'fsub' form.
        /// - Strips dereferenceable_or_null attributes.
        /// </summary>
        static string PreprocessLLVMIR(string ll)
        {
            // Replace: %x = fneg TYPE %y
            //    with: %x = fsub TYPE -0.0, %y
            // (fneg is semantically equivalent to fsub -0.0, x)
            ll = System.Text.RegularExpressions.Regex.Replace(
                ll,
                @"fneg (\S+) ",
                m => $"fsub {m.Groups[1].Value} -0.0, ");

            // Strip dereferenceable_or_null(N) return attributes in calls
            ll = System.Text.RegularExpressions.Regex.Replace(
                ll,
                @"\bdereferenceable_or_null\(\d+\)",
                "");

            return ll;
        }

        public static void Info(string message)
        {
            Console.Write("iril: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("info: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {
            Console.Write("iril: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("error: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}
