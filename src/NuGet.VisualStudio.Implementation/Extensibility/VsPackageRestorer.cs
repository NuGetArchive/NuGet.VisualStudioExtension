// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using SystemTask = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio
{
    [Export(typeof(IVsPackageRestorer))]
    [Export(typeof(IVsPackageRestorer2))]
    public class VsPackageRestorer : IVsPackageRestorer2
    {
        private readonly Configuration.ISettings _settings;
        private readonly ISolutionManager _solutionManager;
        private readonly IPackageRestoreManager _restoreManager;
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;

        [ImportingConstructor]
        public VsPackageRestorer(
            Configuration.ISettings settings,
            ISolutionManager solutionManager,
            IPackageRestoreManager restoreManager,
            ISourceRepositoryProvider sourceRepositoryProvider)
        {
            _settings = settings;
            _solutionManager = solutionManager;
            _restoreManager = restoreManager;
            _sourceRepositoryProvider = sourceRepositoryProvider;
        }

        public bool IsUserConsentGranted()
        {
            var packageRestoreConsent = new PackageManagement.VisualStudio.PackageRestoreConsent(_settings);
            return packageRestoreConsent.IsGranted;
        }

        public void RestorePackages(Project project)
        {
            // We simply use ThreadHelper.JoinableTaskFactory.Run instead of PumpingJTF.Run, unlike,
            // VsPackageInstaller and VsPackageUninstaller. Because, no powershell scripts get executed
            // as part of the operations performed below. Powershell scripts need to be executed on the
            // pipeline execution thread and they might try to access DTE. Doing that under
            // ThreadHelper.JoinableTaskFactory.Run will consistently result in a hang
            ThreadHelper.JoinableTaskFactory.Run(async () =>
                await RestorePackagesConfigProjects(CancellationToken.None));
        }

        public async SystemTask RestorePackages(IVsPackageSourceContext sources, CancellationToken token)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            await ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                var allSources = GetSources(sources);

                var nugetProjects = _solutionManager.GetNuGetProjects().ToList();

                await RestorePackagesCore(nugetProjects, allSources, token);
            });
        }

        public async SystemTask RestorePackages(CancellationToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var enabledSources = _sourceRepositoryProvider
                .GetRepositories()
                .Select(repo => repo.PackageSource.Source)
                .ToList();

            await ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                var projects = _solutionManager.GetNuGetProjects();

                await RestorePackagesCore(projects.ToList(), enabledSources, token);
            });
        }

        public async SystemTask RestorePackages(IEnumerable<Project> projects, IVsPackageSourceContext sources, CancellationToken token)
        {
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            await ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                var allSources = GetSources(sources);

                var nugetProjects = await GetProjects(projects);

                await RestorePackagesCore(nugetProjects, allSources, token);
            });
        }

        private async Task<List<NuGetProject>> GetProjects(IEnumerable<Project> projects)
        {
            var nugetProjects = new List<NuGetProject>();

            foreach (var project in projects)
            {
                nugetProjects.Add(await PackageManagementHelpers.GetProjectAsync(_solutionManager, project));
            }

            return nugetProjects;
        }

        private async SystemTask RestorePackagesConfigProjects(CancellationToken token)
        {
            var solutionDirectory = _solutionManager.SolutionDirectory;
            var nuGetProjectContext = new EmptyNuGetProjectContext();

            try
            {
                await _restoreManager.GetPackagesInSolutionAsync(solutionDirectory, token);
            }
            catch (Exception ex)
            {
                ExceptionHelper.WriteToActivityLog(ex);
            }
        }

        private async SystemTask RestoreProjectJsonProjects(List<BuildIntegratedNuGetProject> projects, List<string> sources, CancellationToken token)
        {
            var solutionDirectory = _solutionManager.SolutionDirectory;
            var nuGetProjectContext = new EmptyNuGetProjectContext();

            foreach (var project in projects)
            {
                try
                {
                    // Project.json based restore
                    // The result is not used here, we ignore failures.
                    var result = await BuildIntegratedRestoreUtility.RestoreAsync(
                        project,
                        nuGetProjectContext,
                        sources,
                        _settings,
                        token);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.WriteToActivityLog(ex);
                }
            }
        }

        /// <summary>
        /// Resolve all sources to their full paths.
        /// </summary>
        private List<string> GetSources(IVsPackageSourceContext context)
        {
            if (context.ExtensionSources == null)
            {
                throw new ArgumentNullException(nameof(context.ExtensionSources));
            }

            if (context.RegistrySources == null)
            {
                throw new ArgumentNullException(nameof(context.RegistrySources));
            }

            if (context.Sources == null)
            {
                throw new ArgumentNullException(nameof(context.Sources));
            }

            Action<string> errorHandler = (s) => { };
            var repoProvider = new PreinstalledRepositoryProvider(errorHandler, _sourceRepositoryProvider);

            var sources = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (context.IncludeEnabledSources)
            {
                sources.UnionWith(_sourceRepositoryProvider.GetRepositories().Select(repo => repo.PackageSource.Source));
            }

            sources.UnionWith(context.Sources);

            foreach (var source in context.RegistrySources)
            {
                repoProvider.AddFromRegistry(source, false);
            }

            foreach (var source in context.RegistrySources)
            {
                repoProvider.AddFromExtension(_sourceRepositoryProvider, source);
            }

            // Add all source paths from the preinstalled repository provider for the extension and registry repos.
            sources.UnionWith(repoProvider.GetRepositories().Select(repo => repo.PackageSource.Source));

            return sources.ToList();
        }

        /// <summary>
        /// Run a restore across v2 and v3 projects.
        /// </summary>
        private async SystemTask RestorePackagesCore(List<NuGetProject> projects, List<string> sources, CancellationToken token)
        {
            var nuGetProjectContext = new EmptyNuGetProjectContext();

            var solutionDirectory = _solutionManager.SolutionDirectory;

            var buildIntegratedProjects = projects
                .Select(project => project as BuildIntegratedNuGetProject)
                .Where(project => project != null).ToList();

            await RestoreProjectJsonProjects(buildIntegratedProjects, sources, token);

            if (projects.Count > buildIntegratedProjects.Count())
            {
                await RestorePackagesConfigProjects(token);
            }
        }
    }
}
