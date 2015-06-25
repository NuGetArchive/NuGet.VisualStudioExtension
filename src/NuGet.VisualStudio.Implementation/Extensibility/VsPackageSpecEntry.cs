using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Implementation
{
    public class VsPackageSpecEntry : IVsPackageSpecEntry
    {
        public VsPackageSpecEntry(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
