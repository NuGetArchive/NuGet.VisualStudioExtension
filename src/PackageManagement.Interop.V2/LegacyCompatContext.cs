extern alias Legacy;
using Legacy.NuGet;

using NuGet.VisualStudio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.Interop.V2
{
    /// <summary>
    /// Contains NuGet.Core 2.* components for legacy mode operations.
    /// </summary>
    public class LegacyModeContext
    {
        private readonly IPackageRepositoryFactory _repositoryFactory;

        private readonly IPackageSourceProvider _sourceProvider;

        private readonly ISolutionManager _solutionManager;

        private readonly IVsPackageManagerFactory _packageManagerFactory;

        public LegacyModeContext(IVsPackageManagerFactory packageManagerFactory, IPackageRepositoryFactory repositoryFactory, IPackageSourceProvider sourceProvider, ISolutionManager solutionManager)
        {
            _repositoryFactory = repositoryFactory;
            _sourceProvider = sourceProvider;
            _solutionManager = solutionManager;
            _packageManagerFactory = packageManagerFactory;
        }

        public IPackageRepositoryFactory RepositoryFactory
        {
            get
            {
                return _repositoryFactory;
            }
        }

        public IPackageSourceProvider SourceProvider
        {
            get
            {
                return _sourceProvider;
            }
        }

        public ISolutionManager SolutionManager
        {
            get
            {
                return _solutionManager;
            }
        }

        public IVsPackageManagerFactory PackageManagerFactory
        {
            get
            {
                return _packageManagerFactory;
            }
        }
    }
}
