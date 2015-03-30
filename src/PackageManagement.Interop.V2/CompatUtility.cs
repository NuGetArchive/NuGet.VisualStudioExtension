extern alias Legacy;
using Legacy.NuGet;
using NuGet.Packaging.Core;
using NuGet.Resolver;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.PackageManagement.Interop.V2
{
    /// <summary>
    /// A helper class for switching between NuGet 2.* and 3.0.0 behavior
    /// </summary>
    public static class CompatUtility
    {
        /// <summary>
        /// True - Use NuGet.Core for package operations
        /// False - Use NuGet.PackageManagement or package operations
        /// </summary>
        public static bool LegacyModeEnabled
        {
            get
            {
                return !StringComparer.Ordinal.Equals(Environment.GetEnvironmentVariable("NUGET_3_PREVIEW"), "1");
            }
        }

        public static AggregateRepository CreateAggregateRepositoryFromSources(LegacyModeContext legacyContext, IEnumerable<string> sources)
        {
            AggregateRepository repository;
            if (sources != null && sources.Any())
            {
                var repositories = sources.Select(s => legacyContext.SourceProvider.ResolveSource(s))
                                             .Select(legacyContext.RepositoryFactory.CreateRepository)
                                             .ToList();
                repository = new AggregateRepository(repositories);
            }
            else
            {
                repository = legacyContext.SourceProvider.CreateAggregateRepository(legacyContext.RepositoryFactory, ignoreFailingRepositories: true);
            }

            return repository;
        }

        public static Legacy.NuGet.SemanticVersion GetVersion(NuGetVersion version)
        {
            Legacy.NuGet.SemanticVersion legacyVersion = null;

            if (version != null)
            {
                legacyVersion = Legacy.NuGet.SemanticVersion.Parse(version.ToString());
            }

            return legacyVersion;
        }

        /// <summary>
        /// Converts DependencyBehavior
        /// Returns null for Ignore
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public static DependencyVersion GetDependencyVersion(DependencyBehavior behavior)
        {
            switch (behavior)
            {
                case DependencyBehavior.Highest:
                    return DependencyVersion.Highest;
                case DependencyBehavior.HighestMinor:
                    return DependencyVersion.HighestMinor;
                case DependencyBehavior.HighestPatch:
                    return DependencyVersion.HighestPatch;
            }

            return DependencyVersion.Lowest;
        }

        /// <summary>
        /// Install packages
        /// </summary>
        public static void ExecuteNuGetProjectAction(LegacyModeContext modeContext, LegacyExecuteContext executionContext, IEnumerable<PackageIdentity> packages)
        {
            var project = modeContext.SolutionManager.GetProject(executionContext.ProjectSafeName);

            var primary = new AggregateRepository(modeContext.RepositoryFactory, executionContext.PrimarySources, false);
            var secondary = new AggregateRepository(modeContext.RepositoryFactory, executionContext.SecondarySources, true);

            var combined = new PriorityPackageRepository(primary, secondary);

            var packageManager = modeContext.PackageManagerFactory.CreatePackageManager(combined, executionContext.AllowFallbackRepositories);

            packageManager.WhatIf = executionContext.WhatIf;

            packageManager.DependencyVersion = GetDependencyVersion(executionContext.DependencyBehavior);

            packageManager.BindingRedirectEnabled = !executionContext.SkipBindingRedirects;

            packageManager.Logger = executionContext.Logger;

            var projectManager = packageManager.GetProjectManager(project);

            bool ignoreDependencies = executionContext.DependencyBehavior == DependencyBehavior.Ignore;

            foreach (var package in packages)
            {
                bool allowPrerelease = executionContext.AllowPrerelease || (package.Version != null && package.Version.IsPrerelease);

                packageManager.InstallPackage(projectManager, package.Id, GetVersion(package.Version), ignoreDependencies, allowPrerelease, executionContext.Logger);
            }
        }
    }
}
