using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.VisualStudio
{
    public class BuildIntegratedProjectSystem : BuildIntegratedNuGetProject
    {

        public BuildIntegratedProjectSystem(FileInfo projectJsonFile, string projectName, string uniqueName)
            : base(projectJsonFile)
        {
            FrameworkName framework = new FrameworkName(".NETCore,Version=v5.0");

            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, projectName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.UniqueName, uniqueName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.TargetFramework, NuGetFramework.Parse(framework.ToString()));

            var supported = new List<FrameworkName>() { framework };

            InternalMetadata.Add(NuGetProjectMetadataKeys.SupportedFrameworks, supported);
        }
    }
}
