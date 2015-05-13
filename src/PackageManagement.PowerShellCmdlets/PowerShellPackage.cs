// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias Legacy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Versioning;
using SemanticVersion = Legacy::NuGet.SemanticVersion;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent the view of packages by Id and Versions
    /// </summary>
    internal class PowerShellPackage : IPowerShellPackage
    {
        public string Id { get; set; }

        public Lazy<Task<IEnumerable<NuGetVersion>>> Versions { get; set; }

        public SemanticVersion Version { get; set; }
    }
}
