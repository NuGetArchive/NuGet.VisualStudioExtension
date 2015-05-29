using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol.Core.Types;
using System.Threading;

namespace NuGet.VisualStudio.Implementation
{
    public static class APITest
    {
        public static void InstallPackageApi(string id, string version)
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageInstaller services = ServiceLocator.GetInstance<IVsPackageInstaller>();
            ISourceRepositoryProvider sourceRepositoryProvider = ServiceLocator.GetInstance<ISourceRepositoryProvider>();
            IVsPackageSourceProvider packageSourceProvider = ServiceLocator.GetInstance<IVsPackageSourceProvider>();
            string activeSourceName = sourceRepositoryProvider.PackageSourceProvider.ActivePackageSourceName;
            var sources = packageSourceProvider.GetSources(true, false);
            string activeSource = null;
            foreach(var source in sources)
            {
                if(source.Key.Equals(activeSourceName))
                {
                    activeSource = source.Value;
                    break;
                }
            }

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                services.InstallPackage(activeSource, project, id, version, false);
                return;
            }
        }

        public static void InstallPackageAsyncApi(string id, string version)
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageInstaller2 services = ServiceLocator.GetInstance<IVsPackageInstaller>() as IVsPackageInstaller2;
            ISourceRepositoryProvider sourceRepositoryProvider = ServiceLocator.GetInstance<ISourceRepositoryProvider>();
            IVsPackageSourceProvider packageSourceProvider = ServiceLocator.GetInstance<IVsPackageSourceProvider>();
            string activeSourceName = sourceRepositoryProvider.PackageSourceProvider.ActivePackageSourceName;
            var sources = packageSourceProvider.GetSources(true, false);
            string activeSource = null;
            foreach (var source in sources)
            {
                if (source.Key.Equals(activeSourceName))
                {
                    activeSource = source.Value;
                    break;
                }
            }

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                services.InstallPackageAsync(project, new string[] { activeSource }, id, version, false, CancellationToken.None).Wait();
                return;
            }
        }

        public static void InstallPackageApiBadSource(string id, string version)
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageInstaller services = ServiceLocator.GetInstance<IVsPackageInstaller>();

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                services.InstallPackage("http://packagesource", project, id, version, false);
                return;
            }
        }

        public static void InstallPackageApiNoSource(string id, string version)
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageInstaller services = ServiceLocator.GetInstance<IVsPackageInstaller>();

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                // install the package from any available source
                services.InstallPackage(null, project, id, version, false);
                return;
            }
        }

        public static void UninstallPackageApi(string id, bool dependency)
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageUninstaller uninstaller = ServiceLocator.GetInstance<IVsPackageUninstaller>();

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                uninstaller.UninstallPackage(project, id, dependency);
                return;
            }
        }

        public static void RestorePackageApi()
        {
            EnvDTE.DTE dte = ServiceLocator.GetInstance<EnvDTE.DTE>();
            IVsPackageRestorer restorer = ServiceLocator.GetInstance<IVsPackageRestorer>();

            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                restorer.RestorePackages(project);
                return;
            }
        }
    }
}
