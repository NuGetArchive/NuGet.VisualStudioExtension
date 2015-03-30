using System;
using System.ComponentModel.Composition;
using System.Threading;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.PackageManagement.Interop.V2;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IPackageRestoreManager))]
    internal class VSPackageRestoreManager : PackageRestoreManager
    {
        public VSPackageRestoreManager()
            : this(
                ServiceLocator.GetInstance<ISourceRepositoryProvider>(),
                ServiceLocator.GetInstance<ISettings>(),
                ServiceLocator.GetInstance<ISolutionManager>(),
                ServiceLocator.GetInstance<ILegacyModeContextProvider>())
        {
        }

        public VSPackageRestoreManager(ISourceRepositoryProvider sourceRepositoryProvider, 
            ISettings settings, 
            ISolutionManager solutionManager,
            ILegacyModeContextProvider legacyProvider)
            : base(sourceRepositoryProvider, settings, solutionManager, legacyProvider)
        {
            SolutionManager = solutionManager;
            SolutionManager.NuGetProjectAdded += OnNuGetProjectAdded;
            SolutionManager.SolutionOpened += OnSolutionOpenedOrClosed;
            SolutionManager.SolutionClosed += OnSolutionOpenedOrClosed;
        }

        public override void EnableCurrentSolutionForRestore(bool fromActivation)
        {
            throw new NotImplementedException();
        }

        public override bool IsCurrentSolutionEnabledForRestore
        {
            get { return false; }
        }

        private async void OnSolutionOpenedOrClosed(object sender, EventArgs e)
        {
            // We need to do the check even on Solution Closed because, let's say if the yellow Update bar
            // is showing and the user closes the solution; in that case, we want to hide the Update bar.
            await base.RaisePackagesMissingEventForSolution(CancellationToken.None);
        }

        private async void OnNuGetProjectAdded(object sender, NuGetProjectEventArgs e)
        {
            if (IsCurrentSolutionEnabledForRestore)
            {
                throw new NotImplementedException();
                //EnablePackageRestore(e.Project, _packageManagerFactory.CreatePackageManager());
            }

            await base.RaisePackagesMissingEventForSolution(CancellationToken.None);
        }
    }
}
