using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.UI;
using NuGet.Protocol.Core.Types;
using NuGet.PackageManagement.Interop.V2;

namespace NuGetVSExtension
{
    [Export(typeof(INuGetUIContextFactory))]
    class VisualStudioUIContextFactory : INuGetUIContextFactory
    {
        private readonly ISourceRepositoryProvider _repositoryProvider;
        private readonly ISolutionManager _solutionManager;
        private readonly IPackageRestoreManager _restoreManager;
        private readonly IOptionsPageActivator _optionsPage;
        private readonly ISettings _settings;
        private readonly ILegacyModeContextProvider _legacyContextProvider;

        [ImportingConstructor]
        public VisualStudioUIContextFactory([Import]ISourceRepositoryProvider repositoryProvider,
            [Import]ISolutionManager solutionManager,
            [Import]ISettings settings,
            [Import]IPackageRestoreManager packageRestoreManager,
            [Import]IOptionsPageActivator optionsPage,
            [Import]ILegacyModeContextProvider legacyContextProvider)
        {
            _repositoryProvider = repositoryProvider;
            _solutionManager = solutionManager;
            _restoreManager = packageRestoreManager;
            _optionsPage = optionsPage;
            _settings = settings;
            _legacyContextProvider = legacyContextProvider;
        }

        public INuGetUIContext Create(NuGetPackage package, IEnumerable<NuGet.ProjectManagement.NuGetProject> projects)
        {
            if (projects == null || !projects.Any())
            {
                throw new ArgumentNullException("projects");
            }

            NuGetPackageManager packageManager = new NuGetPackageManager(_repositoryProvider, 
                _settings, 
                _solutionManager,
                _legacyContextProvider.GetContext());

            UIActionEngine actionEngine = new UIActionEngine(_repositoryProvider, packageManager);

            return new VisualStudioUIContext(
                package,
                _repositoryProvider, 
                _solutionManager, 
                packageManager, 
                actionEngine, 
                _restoreManager, 
                _optionsPage, 
                projects);
        }
    }
}
