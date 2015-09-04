using System.Runtime.Versioning;
using NuGet.Frameworks;

namespace NuGet.PackageManagement.VisualStudio
{
    public class PackageFile : IPackageFile
    {
        private readonly NuGetFramework _targetFramework;

        public PackageFile(string path, NuGetFramework targetFramework)
        {
            Path = path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            _targetFramework = targetFramework;
        }
        public string Path
        {
            get;
            private set;
        }

        public FrameworkName TargetFramework
        {
            get
            {
                return new FrameworkName(_targetFramework.DotNetFrameworkName);
            }
        }
    }
}
