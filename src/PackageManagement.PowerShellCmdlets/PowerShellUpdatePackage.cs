// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias Legacy;
using System.Linq;
using NuGet.ProjectManagement;
using NuGet.Protocol.VisualStudio;
using NuGet.Versioning;
using SemanticVersion = Legacy::NuGet.SemanticVersion;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent package updates found from the remote package source
    /// </summary>
    internal class PowerShellUpdatePackage : PowerShellPackage
    {
        public string Description { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// Get the view of PowerShellPackage. Used for Get-Package -Updates command.
        /// </summary>
        internal static PowerShellUpdatePackage GetPowerShellPackageUpdateView(PSSearchMetadata data, NuGetVersion version, VersionType versionType, NuGetProject project)
        {
            PowerShellUpdatePackage package = new PowerShellUpdatePackage();

            package.Id = data.Identity.Id;
            package.Description = data.Summary;
            package.ProjectName = project.GetMetadata<string>(NuGetProjectMetadataKeys.Name);

            switch (versionType)
            {
                case VersionType.updates:

                    var versions = data.Versions.Value.Result.Where(p => p > version).OrderByDescending(v => v);

                    package.Versions = versions.ToLazy();

                    if (versions.Any())
                    {
                        SemanticVersion sVersion;
                        SemanticVersion.TryParse(versions.FirstOrDefault().ToNormalizedString(), out sVersion);
                        package.Version = sVersion;
                    }

                    break;

                case VersionType.latest:

                    NuGetVersion nVersion = data
                                                .Versions
                                                .Value
                                                .Result
                                                .Where(p => p > version)
                                                .OrderByDescending(v => v)
                                                .FirstOrDefault();

                    if (nVersion != null)
                    {
                        package.Versions = new [] { nVersion }.ToLazy();
                        SemanticVersion sVersion;
                        SemanticVersion.TryParse(nVersion.ToNormalizedString(), out sVersion);
                        package.Version = sVersion;
                    }

                    break;
            }

            return package;
        }
    }
}
