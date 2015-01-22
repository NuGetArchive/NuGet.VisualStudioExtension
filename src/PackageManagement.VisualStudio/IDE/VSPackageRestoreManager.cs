using NuGet.ProjectManagement;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IPackageRestoreManager))]
    internal class VSPackageRestoreManager : IPackageRestoreManager
    {
        public void CheckForMissingPackages()
        {
            //throw new NotImplementedException();
        }

        public void EnableCurrentSolutionForRestore(bool fromActivation)
        {
            //throw new NotImplementedException();
        }

        public bool IsCurrentSolutionEnabledForRestore
        {
            get { return false; }
        }

        public event EventHandler<PackagesMissingStatusEventArgs> PackagesMissingStatusChanged;

        public Task RestoreMissingPackages()
        {
            throw new NotImplementedException();
        }


        public Task RestoreMissingPackages(NuGetProject nuGetProject)
        {
            throw new NotImplementedException();
        }

        void IPackageRestoreManager.EnableCurrentSolutionForRestore(bool fromActivation)
        {
            //throw new NotImplementedException();
        }

        bool IPackageRestoreManager.IsCurrentSolutionEnabledForRestore
        {
            get { return true; }
        }

        event EventHandler<PackagesMissingStatusEventArgs> IPackageRestoreManager.PackagesMissingStatusChanged
        {
            add {  }
            remove { }
        }

        void IPackageRestoreManager.RaisePackagesMissingEventForSolution()
        {
            //throw new NotImplementedException();
        }

        Task<bool> IPackageRestoreManager.RestoreMissingPackages(System.Collections.Generic.IEnumerable<Packaging.PackageReference> packageReferences)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPackageRestoreManager.RestoreMissingPackages(NuGetProject nuGetProject)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPackageRestoreManager.RestoreMissingPackagesInSolution()
        {
            throw new NotImplementedException();
        }
    }
}
