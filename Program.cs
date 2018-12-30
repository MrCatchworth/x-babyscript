using CommandLine;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using XBabyScript.Compile;
using XBabyScript.Decompile;

namespace XBabyScript
{
    public enum OutputType
    {
        File,
        Directory,
        Auto
    }

    class Program
    {
        private static void CompileWatchedFile(WatchOptions options, string sourcePath, WatcherChangeTypes changeTypes)
        {
            Thread.Sleep(2000);

            Console.Error.WriteLine($"Event in watched file {sourcePath} ({changeTypes}) ...");

            var outputPath = Path.ChangeExtension(sourcePath, ".xml");

            var logger = new BabyScriptLogger();
            var conversionProperties = new ConversionProperties
            {
                Logger = logger,
                Options = options
            };

            try
            {
                conversionProperties.InputStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                logger.LogError(sourcePath, $"Failed to open file for reading: {e.Message}");
                return;
            }

            try
            {
                conversionProperties.OutputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            }
            catch (Exception e)
            {
                logger.LogError(outputPath, $"Failed to open file for writing: {e.Message}");
                return;
            }

            var success = false;
            try
            {
                success = new BabyScriptCompiler().Convert(conversionProperties);
            }
            catch (Exception e)
            {
                logger.LogError(outputPath, $"Unexpected error while compiling: {e.Message}");
            }
            finally {
                conversionProperties.OutputStream.Dispose();
                conversionProperties.InputStream.Dispose();
            }

            if (success)
            {
                Console.Error.WriteLine("Done");
            }
            else
            {
                Console.Error.WriteLine("Failed");
            }
        }
        private static void StartWatching(WatchOptions options)
        {
            // Verify number of input paths (1).
            var numInputPaths = options.InputPaths.Count();
            if (numInputPaths != 1)
            {
                Console.Error.WriteLine($"Expected 1 directory to watch; got {numInputPaths}");
                Environment.ExitCode = 1;
                return;
            }

            // Verify that the input path exists.
            var watchPath = options.InputPaths.Single();
            if (!Directory.Exists(watchPath))
            {
                Console.Error.WriteLine($"{watchPath} does not point to a valid directory to watch");
                Environment.ExitCode = 1;
                return;
            }

            // Watch for file changes in that directory.
            var watcher = new FileSystemWatcher();
            watcher.Path = watchPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.xbs";

            var watchHandler = new FileSystemEventHandler((sender, eArgs) =>
            {
                CompileWatchedFile(options, eArgs.FullPath, eArgs.ChangeType);
            });
            watcher.Created += watchHandler;
            watcher.Changed += watchHandler;

            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Now watching {watchPath} for changes ...");

            // We want to run the program indefinitely, and quit on an interrupt signal.
            var quitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                quitEvent.Set();
                watcher.EnableRaisingEvents = false;
                eArgs.Cancel = true;
            };

            quitEvent.WaitOne();
        }

        static void Main(string[] args)
        {
            var numVerbsSpecified = 0;
            Options programOptions = null;
            CompileOptions compileOptions = null;
            DecompileOptions decompileOptions = null;
            WatchOptions watchOptions = null;

            Parser.Default.ParseArguments<Options, CompileOptions, DecompileOptions, WatchOptions>(args)
                .WithParsed<Options>(options =>
                {
                    programOptions = options;
                })
                .WithParsed<CompileOptions>(options =>
                {
                    numVerbsSpecified++;
                    compileOptions = options;
                })
                .WithParsed<DecompileOptions>(options =>
                {
                    numVerbsSpecified++;
                    decompileOptions = options;
                })
                .WithParsed<WatchOptions>(options =>
                {
                    numVerbsSpecified++;
                    watchOptions = options;
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

            if (numVerbsSpecified != 1)
            {
                Console.Error.WriteLine("Incorrect number of program modes specified");
                Environment.ExitCode = 1;
                return;
            }

            if (watchOptions != null)
            {
                StartWatching(watchOptions);
                return;
            }

            var conversionType = compileOptions != null ? ConversionType.Compile : ConversionType.Decompile;

            var expandedPaths = programOptions.ExpandedInputPaths;

            if (!programOptions.IsSingleInputFile && programOptions.OutputType != OutputType.Auto)
            {
                if (!Directory.Exists(programOptions.OutputPath))
                {
                    Console.Error.WriteLine($"Output path {programOptions.OutputPath} must refer to a valid directory");
                    Environment.ExitCode = 1;
                    return;
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

                string outputPath = programOptions.GetOutputPathForFile(inputPath, conversionType);

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
                if (compileOptions != null)
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
        }
    }
}
