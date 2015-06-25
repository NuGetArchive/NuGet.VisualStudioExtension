using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.Implementation
{
    public class VsPackageSpec : IVsPackageSpec
    {
        private readonly JObject _json;

        public VsPackageSpec(JObject json)
        {
            _json = json;
        }

        public IReadOnlyList<IVsPackageSpecDependency> Dependencies
        {
            get
            {
                var results = new List<VsPackageSpecDependency>();

                foreach (var dependency in JsonConfigUtility.GetDependencies(_json))
                {
                    results.Add(new VsPackageSpecDependency(dependency.Id, dependency.VersionRange.OriginalString));
                }

                return results;
            }
        }

        public IReadOnlyList<IVsPackageSpecEntry> Frameworks
        {
            get
            {
                var results = new List<VsPackageSpecEntry>();

                foreach (var framework in JsonConfigUtility.GetFrameworks(_json))
                {
                    results.Add(new VsPackageSpecEntry(framework.DotNetFrameworkName));
                }

                return results;
            }
        }

        public string Json
        {
            get
            {
                return _json.ToString();
            }
        }

        public IReadOnlyList<IVsPackageSpecEntry> Runtimes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyList<IVsPackageSpecEntry> Supports
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddDependency(IVsPackageSpecDependency entry)
        {
            throw new NotImplementedException();
        }

        public void AddFramework(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }

        public void AddRuntime(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }

        public void AddSupport(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }

        public void RemoveDependency(IVsPackageSpecDependency entry)
        {
            throw new NotImplementedException();
        }

        public void RemoveFramework(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }

        public void RemoveRuntime(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }

        public void RemoveSupport(IVsPackageSpecEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
