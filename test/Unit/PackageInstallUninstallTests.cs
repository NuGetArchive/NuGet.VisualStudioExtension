using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using Xunit;

namespace PackageInstallUninstallTests
{
    class PackageInstallUninstallTests
    {
        [Fact]
        public void Test()
        {
            System.Diagnostics.Debugger.Launch();
            var x = new VsPackageUninstaller(Mock.Of<ISourceRepositoryProvider>(), Mock.Of<ISettings>(),
                Mock.Of<ISolutionManager>(), Mock.Of<IDeleteOnRestartManager>());

            x.UninstallPackage(Mock.Of<Project>(), "", false);
        }
    }
}
