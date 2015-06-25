using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Newtonsoft.Json.Linq;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Implementation
{
    [Export(typeof(IVsPackageSpecManager))]
    public class VsPackageSpecManager : IVsPackageSpecManager
    {
        private readonly ISolutionManager _solutionManager;
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;
        private readonly Configuration.ISettings _settings;

        [ImportingConstructor]
        public VsPackageSpecManager(
            ISolutionManager solutionManager,
            ISourceRepositoryProvider sourceRepositoryProvider,
            Configuration.ISettings settings)
        {
            _solutionManager = solutionManager;
            _sourceRepositoryProvider = sourceRepositoryProvider;
            _settings = settings;
        }

        public async Task<IVsPackageSpec> ReadAsync(Project project, CancellationToken token)
        {
            IVsPackageSpec spec = null;

            var nugetProject = await PackageManagementHelpers.GetProjectAsync(_solutionManager, project) as BuildIntegratedNuGetProject;

            if (nugetProject != null)
            {
                var json = JObject.Parse(File.ReadAllText(nugetProject.JsonConfigPath));

                spec = new VsPackageSpec(json);
            }

            return spec;
        }

        public async Task WriteAsync(IVsPackageSpec packageSpec, Project project, CancellationToken token)
        {
            var nugetProject = await PackageManagementHelpers.GetProjectAsync(_solutionManager, project) as BuildIntegratedNuGetProject;

            if (nugetProject != null)
            {
                await SaveJsonAsync(nugetProject.JsonConfigPath, packageSpec.Json);
            }
        }

        public async Task<IVsPackageRestoreResult> RestoreProjectAsync(Project project, IEnumerable<string> additionalSourcePaths, CancellationToken token)
        {
            IVsPackageRestoreResult restoreResult = null;

            var nugetProject = await PackageManagementHelpers.GetProjectAsync(_solutionManager, project) as BuildIntegratedNuGetProject;

            if (nugetProject != null)
            {
                // combine the additional sources with the enabled sources
                var sources = new HashSet<string>(
                    additionalSourcePaths,
                    StringComparer.OrdinalIgnoreCase);

                sources.UnionWith(_sourceRepositoryProvider.GetRepositories().Select(source => source.PackageSource.Source));

                var result = await BuildIntegratedRestoreUtility.RestoreAsync(nugetProject, new VSAPIProjectContext(), sources, _settings, token);
            }

            return restoreResult;
        }

        private async Task<JObject> GetJsonAsync(string path)
        {
            using (var streamReader = new StreamReader(File.OpenRead(path)))
            {
                return JObject.Parse(await streamReader.ReadToEndAsync());
            }
        }

        private async Task SaveJsonAsync(string path, string json)
        {
            using (var writer = new StreamWriter(Path.GetFullPath(path), false, Encoding.UTF8))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
