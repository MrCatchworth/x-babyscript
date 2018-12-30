using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;

namespace XBabyScript
{
    public enum ConversionType
    {
        Compile,
        Decompile
    }

    public class Options
    {
        private string outputPath;
        [Option('o', "out", Default = null, HelpText = "Path to a file or directory where the converted text will be written. Ignored in watch mode.")]
        public string OutputPath
        {
            get
            {
                return outputPath;
            }
            set
            {
                outputPath = value;

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    outputType = OutputType.Auto;
                }
                else if (Directory.Exists(outputPath))
                {
                    outputType = OutputType.Directory;
                }
                else
                {
                    outputType = OutputType.File;
                }
            }
        }

        private OutputType outputType = OutputType.Auto;
        public OutputType OutputType
        {
            get
            {
                return outputType;
            }
        }

        private string changeExtension(string path, ConversionType convType)
        {
            string newExtension = (convType == ConversionType.Compile) ? ".xml" : "." + Globals.Extension;
            return Path.ChangeExtension(path, newExtension);
        }

        public string GetOutputPathForFile(string inputPath, ConversionType conversionType)
        {
            switch (OutputType)
            {
                case OutputType.File:
                    return OutputPath;

                case OutputType.Directory:
                    string inputFileName = Path.GetFileName(inputPath);
                    return Path.Join(OutputPath, changeExtension(inputFileName, conversionType));

                case OutputType.Auto:
                    return changeExtension(inputPath, conversionType);

                default:
                    throw new ArgumentException($"Invalid conversion type: {conversionType}");
            }
        }

        [Option('f', Default = false, HelpText = "Overwrite files without prompting.")]
        public bool Force { get; set; }

        [Option("convertcase", Default = false, HelpText = "Convert between snake_case and camelCase as appropriate.")]
        public bool ConvertCaseStyle { get; set; }

        [Option("indent", Default = 4, HelpText = "The number of spaces in each level of indentation.")]
        public int Indent { get; set; }

        [Value(0, Required = true, HelpText = "One or more input files to convert. If more than one file is specified, the output path must refer to a directory, not a regular file.")]
        public IEnumerable<string> InputPaths { get; set; }

        public IEnumerable<string> ExpandedInputPaths
        {
            get
            {
                if (InputPaths == null) throw new InvalidOperationException($"{nameof(ExpandedInputPaths)} cannot be retrieved when {nameof(InputPaths)} is null.");
                var expandedPaths = new List<string>(InputPaths);
                var wildcardPresent = false;
                foreach (string inPath in InputPaths)
                {
                    if (inPath.Contains("*") || inPath.Contains("?"))
                    {
                        wildcardPresent = true;
                        var currentPathPos = expandedPaths.IndexOf(inPath);

                        var resolvedPaths = Directory.GetFiles(".", inPath);
                        expandedPaths.InsertRange(currentPathPos, resolvedPaths);

                        expandedPaths.Remove(inPath);
                    }
                }

                IsSingleInputFile = !wildcardPresent && expandedPaths.Count < 2;
                return expandedPaths;
            }
        }

        public bool IsSingleInputFile { get; private set; }
    }

    [Verb("compile", HelpText = "Convert a file from Babyscript to XML.")]
    public class CompileOptions : Options
    {

    }

    [Verb("watch", HelpText = "Watch a directory for changes in XBS files and compile them automatically.")]
    public class WatchOptions : Options
    {

    }

    [Verb("decompile", HelpText = "Convert a file from XML to Babyscript.")]
    public class DecompileOptions : Options
    {

    }
}
