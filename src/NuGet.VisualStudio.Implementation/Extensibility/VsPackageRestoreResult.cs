using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using NuGet.Commands;

namespace NuGet.VisualStudio.Implementation
{
    [Export(typeof(IVsPackageRestoreResult))]
    public class VsPackageRestoreResult : IVsPackageRestoreResult
    {
        public VsPackageRestoreResult(RestoreResult result)
        {
            Success = result.Success;
        }

        public bool Success { get; }
    }
}
