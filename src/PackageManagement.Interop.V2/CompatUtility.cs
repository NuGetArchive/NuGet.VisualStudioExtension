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
                legacyVersion = new Legacy.NuGet.SemanticVersion(version.Version, version.Release);
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

        public static void ExecuteNuGetProjectAction(LegacyModeContext modeContext, LegacyExecuteContext executionContext, PackageIdentity package, IEnumerable<string> sources)
        {
            // var repo = CreateAggregateRepositoryFromSources(context, sources);

            var packageManager = modeContext.PackageManagerFactory.CreatePackageManager();

            packageManager.WhatIf = executionContext.WhatIf;

            packageManager.DependencyVersion = GetDependencyVersion(executionContext.DependencyBehavior);

            packageManager.BindingRedirectEnabled = !executionContext.SkipBindingRedirects;

            bool ignoreDependencies = executionContext.DependencyBehavior == DependencyBehavior.Ignore;

            bool allowPrerelease = executionContext.AllowPrerelease || (package.Version != null && package.Version.IsPrerelease);

            packageManager.InstallPackage(package.Id, GetVersion(package.Version), ignoreDependencies, allowPrerelease);
        }
    }
}
