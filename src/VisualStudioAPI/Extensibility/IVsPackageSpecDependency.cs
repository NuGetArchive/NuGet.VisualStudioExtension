// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents a dependency entry in a package spec.
    /// </summary>
    [ComImport]
    [Guid("139629DC-32FA-4288-9ABB-AF045C079DAD")]
    public interface IVsPackageSpecDependency : IVsPackageSpecEntry
    {
        /// <summary>
        /// The dependency version range for the package dependency.
        /// </summary>
        string VersionRange { get; }
    }
}
