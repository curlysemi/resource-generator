// <copyright file="VisualStudioPackage.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Tools
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(VisualStudioPackage.PackageGuidString)]
    public sealed class VisualStudioPackage : Package
    {
        /// <summary>
        /// VisualStudioPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "0ADB068F-9832-4BC2-91FE-8A5CF2E66313";
    }
}
