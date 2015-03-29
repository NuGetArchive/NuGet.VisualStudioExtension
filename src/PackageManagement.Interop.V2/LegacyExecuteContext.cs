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

        public IEnumerable<string> Sources { get; set; }

        public string ProjectCustomName { get; set; }

        public LegacyExecuteContext()
        {

        }
    }
}
