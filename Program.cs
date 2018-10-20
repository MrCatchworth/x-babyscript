using CommandLine;
using System;
using System.IO;
using System.Linq;
using XRebirthBabyScript.Compile;
using XRebirthBabyScript.Decompile;

namespace XRebirthBabyScript
{
    class Program
    {
        static void Main(string[] args)
        {
            var hasCompileVerb = false;
            var hasDecompileVerb = false;
            Options programOptions = null;

            args = new[] {
                "decompile",
                "-o", ".",
                "GM_Assassination.xml"
            };

            Parser.Default.ParseArguments<Options, CompileOptions, DecompileOptions>(args)
                .WithParsed<Options>(options =>
                {
                    programOptions = options;
                })
                .WithParsed<CompileOptions>(options =>
                {
                    Console.WriteLine("Compile options were specified");
                    hasCompileVerb = true;
                })
                .WithParsed<DecompileOptions>(options =>
                {
                    Console.WriteLine("Decompile options were specified");
                    hasDecompileVerb = true;
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.Error.WriteLine(error.ToString());
                    }
                });

            if (programOptions == null)
            {
                Environment.ExitCode = 1;
                return;
            }

            if (hasCompileVerb && hasDecompileVerb)
            {
                Console.Error.WriteLine("Conflicting modes - do you want to compile, or decompile?");
                Environment.ExitCode = 1;
                return;
            }

            if (!hasCompileVerb && !hasDecompileVerb)
            {
                Console.Error.WriteLine("No mode specified - do you want to compile, or decompile?");
                Environment.ExitCode = 1;
                return;
            }

            var expandedPaths = programOptions.ExpandedInputPaths;

            var outputIsDirectory = false;

            if (programOptions.IsSingleInputFile)
            {
                outputIsDirectory = Directory.Exists(programOptions.OutputPath);
            }
            else
            {
                outputIsDirectory = true;
                if (!Directory.Exists(programOptions.OutputPath))
                {
                    Console.Error.WriteLine($"The output path must be a valid directory when variable input files are specified");
                }
            }

            var logger = new BabyScriptLogger();
            var allFilesSuccessful = true;

            foreach (var inputPath in expandedPaths)
            {
                var conversionProperties = new ConversionProperties
                {
                    Logger = logger,
                    Options = programOptions
                };

                conversionProperties.FileName = inputPath;

                string outputPath;

                if (outputIsDirectory)
                {
                    var extension = hasCompileVerb ? ".xml" : ".xrbs";
                    var outputFileName = Path.GetFileNameWithoutExtension(inputPath) + extension;
                    outputPath = Path.Combine(programOptions.OutputPath, outputFileName);
                }
                else
                {
                    outputPath = programOptions.OutputPath;
                }

                if (File.Exists(outputPath) && !programOptions.Force)
                {
                    Console.Error.Write($"{outputPath} already exists. Overwrite? (y/N)");
                    string response = Console.ReadLine().ToLower();
                    if (!(response.Equals("y") || response.Equals("yes")))
                    {
                        continue;
                    }
                }

                try
                {
                    conversionProperties.InputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    logger.LogError(inputPath, $"Failed to open file for reading: {e.Message}");
                    continue;
                }

                try
                {
                    conversionProperties.OutputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                }
                catch (Exception e)
                {
                    logger.LogError(outputPath, $"Failed to open file for writing: {e.Message}");
                    continue;
                }

                Console.WriteLine($"{inputPath} -> {outputPath}");

                IBabyScriptConverter converter;
                if (hasCompileVerb)
                {
                    converter = new BabyScriptCompiler();
                }
                else
                {
                    converter = new BabyScriptDecompiler();
                }

                var success = converter.Convert(conversionProperties);

                if (!success)
                {
                    allFilesSuccessful = false;
                    continue;
                }

                try
                {
                    conversionProperties.OutputStream.Flush();
                    conversionProperties.OutputStream.Close();
                }
                catch (Exception e)
                {
                    logger.LogError(outputPath, $"Failed to close output file: {e}");
                    allFilesSuccessful = false;
                    continue;
                }
            }

            Environment.ExitCode = allFilesSuccessful ? 0 : 4;

            Console.Read();
        }
    }
}
