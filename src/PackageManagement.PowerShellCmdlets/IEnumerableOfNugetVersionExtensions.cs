using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    internal static class IEnumerableOfNugetVersionExtensions
    {
        public static Lazy<Task<IEnumerable<NuGetVersion>>> ToLazy(this IEnumerable<NuGetVersion> nugetVersion)
        {
            return new Lazy<Task<IEnumerable<NuGetVersion>>>(() => Task.FromResult(nugetVersion));
        }
    }
}
