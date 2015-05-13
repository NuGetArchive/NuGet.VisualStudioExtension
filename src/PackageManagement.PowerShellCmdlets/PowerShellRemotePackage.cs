// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.VisualStudio;
using NuGet.Versioning;
using SemanticVersion = Legacy::NuGet.SemanticVersion;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent packages found from the remote package source
    /// </summary>
    internal class PowerShellRemotePackage : IPowerShellPackage
    {
        public string Id { get; set; }

        public Lazy<Task<IEnumerable<NuGetVersion>>> Versions { get; set; }

        public SemanticVersion Version { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Get the view of PowerShellPackage. Used for Get-Package -ListAvailable command.
        /// </summary>
        /// <param name="metadata">list of PSSearchMetadata</param>
        /// <param name="versionType"></param>
        /// <returns></returns>
        internal static List<PowerShellRemotePackage> GetPowerShellPackageView(IEnumerable<PSSearchMetadata> metadata, VersionType versionType)
        {
            List<PowerShellRemotePackage> view = new List<PowerShellRemotePackage>();
            foreach (PSSearchMetadata data in metadata)
            {
                PowerShellRemotePackage package = new PowerShellRemotePackage();
                package.Id = data.Identity.Id;
                package.Description = data.Summary;

                switch (versionType)
                {
                    case VersionType.all:
                        {
                            var dataVersions = data.Versions.Value.Result;
                            package.Versions = dataVersions.ToLazy();

                            if (dataVersions != null
                                && dataVersions.Any())
                            {
                                SemanticVersion sVersion;
                                SemanticVersion.TryParse(dataVersions.FirstOrDefault().ToNormalizedString(), out sVersion);
                                package.Version = sVersion;
                            }
                        }

                        break;

                    case VersionType.latest:
                        {
                            var dataVersions = data.Versions.Value.Result;

                            NuGetVersion nVersion = data.Version == null ?
                                data.Versions.Value.Result.OrderByDescending(v => v).FirstOrDefault() :
                                data.Version;

                            package.Versions = new[] { nVersion }.ToLazy();

                            if (nVersion != null)
                            {
                                SemanticVersion sVersion;
                                SemanticVersion.TryParse(nVersion.ToNormalizedString(), out sVersion);
                                package.Version = sVersion;
                            }
                        }

                        break;
                }

                view.Add(package);
            }
            return view;
        }
    }
}
