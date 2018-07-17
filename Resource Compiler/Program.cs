// <copyright file="Program.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using GammaFour.ResourceGenerator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Formatting;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Options;

    /// <summary>
    /// The command line version of the Custom Tool for generating code from a schema.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Dictionary of command line parameter switches and the states they invoke in the parser.
        /// </summary>
        private static Dictionary<string, ArgumentState> argumentStates = new Dictionary<string, ArgumentState>()
        {
            { "-i", ArgumentState.InputFileName },
            { "-ns", ArgumentState.TargetNamespace },
            { "-out", ArgumentState.OutputFileName }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        private Program(string[] args)
        {
            this.InputFilePath = string.Empty;
            this.TargetNamespace = "DefaultNamespace";

            // The command line parser is driven by different states that are triggered by the flags read.  Unless a flag has been parsed, the command line
            // parser assumes that it's reading the file name from the command line.
            ArgumentState argumentState = ArgumentState.InputFileName;

            // Parse the command line for arguments.
            foreach (string argument in args)
            {
                // Use the dictionary to transition from one state to the next based on the input parameters.
                ArgumentState nextArgumentState;
                if (Program.argumentStates.TryGetValue(argument, out nextArgumentState))
                {
                    argumentState = nextArgumentState;
                    continue;
                }

                // The parsing state will determine which variable is read next.
                switch (argumentState)
                {
                    case ArgumentState.InputFileName:

                        // Expand the environment variables so that paths don't need to be absolute.
                        this.InputFilePath = Environment.ExpandEnvironmentVariables(argument);

                        // The output name defaults to the input file name with a new extension.
                        this.OutputFilePath = string.Format("{0}.cs", Path.GetFileNameWithoutExtension(this.InputFilePath));

                        break;

                    case ArgumentState.OutputFileName:

                        // Expand the environment variables so that paths don't need to be absolute.
                        this.OutputFilePath = Environment.ExpandEnvironmentVariables(argument);
                        break;

                    case ArgumentState.TargetNamespace:

                        // This is the namespace that is used to create the target data model.
                        this.TargetNamespace = argument;
                        break;
                }

                // The default state is to look for the input file name on the command line.
                argumentState = ArgumentState.InputFileName;
            }
        }

        /// <summary>
        /// Gets the input file path.
        /// </summary>
        public string InputFilePath { get; private set; }

        /// <summary>
        /// Gets the output file path.
        /// </summary>
        public string OutputFilePath { get; private set; }

        /// <summary>
        /// Gets the target namespace.
        /// </summary>
        public string TargetNamespace { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The status of running the code generator to create the service.</returns>
        [STAThread]
        public static int Main(string[] args)
        {
            // These are the parameters that are parsed out of the command line.
            Program program = new Program(args);
            return program.Run();
        }

        /// <summary>
        /// Compile the resources.
        /// </summary>
        /// <returns>0 if successful, 1 if not.</returns>
        private int Run()
        {
            try
            {
                // This will read the input XML schema into a large buffer.
                string fileContents;
                using (StreamReader streamReader = new StreamReader(this.InputFilePath))
                {
                    fileContents = streamReader.ReadToEnd();
                }

                // Generate the destination code using the code generator, write it to the output stream.
                string generatedContents = this.GenerateCode(fileContents);
                using (StreamWriter streamWriter = new StreamWriter(this.OutputFilePath))
                {
                    streamWriter.Write("// <auto-generated />\r\n");
                    streamWriter.Write(generatedContents);
                }

                // This means the program executed successfully.
                return 0;
            }
            catch (Exception exception)
            {
                // This will catch any generic errors and dump them to the console.
                Console.WriteLine(exception.Message);
            }

            // This means the compilation failed.
            return 1;
        }

        /// <summary>
        /// The method that does the actual work of generating code given the input file
        /// </summary>
        /// <param name="inputFileContent">File contents as a string</param>
        /// <returns>The generated code file as a byte-array</returns>
        private string GenerateCode(string inputFileContent)
        {
            // This schema describes the data model that is to be generated.
            XDocument xDocument = XDocument.Parse(inputFileContent);

            // Extract the name of the target class from the input file name.
            string className = Path.GetFileNameWithoutExtension(this.InputFilePath);

            // This creates the compilation unit from the schema.
            CompilationUnit compilationUnit = new CompilationUnit(xDocument, className, this.TargetNamespace);

            // A workspace is needed in order to turn the compilation unit into code.
            AdhocWorkspace adhocWorkspace = new AdhocWorkspace();
            OptionSet options = adhocWorkspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, false);
            adhocWorkspace.Options = options;

            // Format the compilation unit.
            SyntaxNode syntaxNode = Formatter.Format(compilationUnit.Syntax, adhocWorkspace);
            return syntaxNode.ToString();
        }
    }
}