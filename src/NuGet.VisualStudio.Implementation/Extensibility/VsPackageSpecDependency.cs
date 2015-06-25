using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Implementation
{
    public class VsPackageSpecDependency : VsPackageSpecEntry, IVsPackageSpecDependency
    {
        public VsPackageSpecDependency(string id, string versionRange)
            : base(id)
        {
            VersionRange = versionRange;
        }

        public string VersionRange { get; }
    }
}
