extern alias Legacy;
using Legacy.NuGet;

using NuGet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.Interop.V2
{
    /// <summary>
    /// Install, uninstall options
    /// </summary>
    public class LegacyExecuteContext
    {
        public bool SkipAssemblyReferences { get; set; }

        public bool SkipBindingRedirects { get; set; }

        public DependencyBehavior DependencyBehavior { get; set; }

        public bool AllowFallbackRepositories { get; set; }

        public bool WhatIf { get; set; }

        public bool AllowPrerelease { get; set; }

        public IEnumerable<string> PrimarySources { get; set; }

        public IEnumerable<string> SecondarySources { get; set; }

        public string ProjectSafeName { get; set; }

        public bool Force { get; set; }

        public bool RemoveDependencies { get; set; }

        public ILogger Logger { get; set; }

        public LegacyExecuteContext()
        {
            // Defaults
            Logger = NullLogger.Instance;
            DependencyBehavior = DependencyBehavior.Lowest;
        }
    }
}
