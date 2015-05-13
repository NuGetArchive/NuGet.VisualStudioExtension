// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias Legacy;
using System.Collections.Generic;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using SemanticVersion = Legacy::NuGet.SemanticVersion;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent the view of packages installed to project(s)
    /// </summary>
    internal class PowerShellInstalledPackage : PowerShellPackage
    {
        public string ProjectName { get; set; }

        /// <summary>
        /// Get the view of installed packages. Use for Get-Package command.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="versionType"></param>
        /// <returns></returns>
        internal static List<PowerShellInstalledPackage> GetPowerShellPackageView(Dictionary<NuGetProject,
                                                                                  IEnumerable<PackageReference>> dictionary)
        {
            List<PowerShellInstalledPackage> views = new List<PowerShellInstalledPackage>();

            foreach (KeyValuePair<NuGetProject, IEnumerable<PackageReference>> entry in dictionary)
            {
                // entry.Value is an empty list if no packages are installed
                foreach (PackageReference package in entry.Value)
                {
                    PowerShellInstalledPackage view = new PowerShellInstalledPackage();
                    SemanticVersion sVersion;
                    SemanticVersion.TryParse(package.PackageIdentity.Version.ToNormalizedString(), out sVersion);
                    view.Version = sVersion;

                    view.Id = package.PackageIdentity.Id;

                    var versions = new[] { package.PackageIdentity.Version };
                    view.Versions = versions.ToLazy();

                    view.ProjectName = entry.Key.GetMetadata<string>(NuGetProjectMetadataKeys.Name);

                    views.Add(view);
                }
            }

            return views;
        }
    }
}
