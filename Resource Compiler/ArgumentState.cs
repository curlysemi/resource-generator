// <copyright file="ArgumentState.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    /// <summary>
    /// These are the parsing states used to read the arguments on the command line.
    /// </summary>
    internal enum ArgumentState
    {
        /// <summary>
        /// The target namespace for the generated code.
        /// </summary>
        TargetNamespace,

        /// <summary>
        /// The input file name.
        /// </summary>
        InputFileName,

        /// <summary>
        /// The output file name.
        /// </summary>
        OutputFileName
    }
}