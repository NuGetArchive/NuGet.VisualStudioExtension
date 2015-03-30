using NuGet.PackageManagement.Interop.V2;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.InteropV2
{
    internal static class LegacyHelpers
    {

        internal static LegacyExecuteContext CreateContext(ISourceRepositoryProvider sourceRepositoryProvider, 
            string primarySource,
            ISolutionManager solutionManager, 
            NuGetProject nuGetProject,
            INuGetProjectContext nuGetProjectContext)
        {
            LegacyExecuteContext executeContext = new LegacyExecuteContext();
            executeContext.AllowFallbackRepositories = true;

            var allSources = sourceRepositoryProvider.GetRepositories().Where(e => e.PackageSource.IsEnabled);

            executeContext.ProjectSafeName = solutionManager.GetNuGetProjectSafeName(nuGetProject);

            executeContext.Logger = new LegacyLogger(nuGetProjectContext);

            executeContext.PrimarySources = new string[] { primarySource };

            // get all enabled sources that aren't the primary
            executeContext.SecondarySources = allSources.Where(e =>
                !StringComparer.OrdinalIgnoreCase.Equals(primarySource, e.PackageSource.Source))
                .Select(e => e.PackageSource.Source);

            return executeContext;
        }
    }
}
