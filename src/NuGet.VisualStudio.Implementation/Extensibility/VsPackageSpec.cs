using System;
using System.Collections.Generic;
using System.Globalization;
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
                return GetDependencies(_json).ToList();
            }
        }

        public IReadOnlyList<IVsPackageSpecEntry> Frameworks
        {
            get
            {
                return GetEntries(_json, "frameworks").ToList();
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
                return GetEntries(_json, "runtimes").ToList();
            }
        }

        public IReadOnlyList<IVsPackageSpecEntry> Supports
        {
            get
            {
                return GetEntries(_json, "supports").ToList();
            }
        }

        public void AddDependency(IVsPackageSpecDependency entry)
        {
            AddDependency(entry);
        }

        public void AddFramework(IVsPackageSpecEntry entry)
        {
            AddEntry(_json, "frameworks", entry);
        }

        public void AddRuntime(IVsPackageSpecEntry entry)
        {
            AddEntry(_json, "runtimes", entry);
        }

        public void AddSupport(IVsPackageSpecEntry entry)
        {
            AddEntry(_json, "supports", entry);
        }

        public void RemoveDependency(IVsPackageSpecDependency entry)
        {
            RemoveDependency(entry);
        }

        public void RemoveFramework(IVsPackageSpecEntry entry)
        {
            RemoveEntry(_json, "frameworks", entry);
        }

        public void RemoveRuntime(IVsPackageSpecEntry entry)
        {
            RemoveEntry(_json, "runtimes", entry);
        }

        public void RemoveSupport(IVsPackageSpecEntry entry)
        {
            RemoveEntry(_json, "supports", entry);
        }

        /// <summary>
        /// Read dependencies from a project.json file
        /// </summary>
        private static IEnumerable<VsPackageSpecDependency> GetDependencies(JObject json)
        {
            JToken node = null;
            if (json.TryGetValue("dependencies", out node))
            {
                foreach (var dependency in node)
                {
                    var result = ParseDependency(dependency);

                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Convert a dependency entry into an id and version range
        /// </summary>
        private static VsPackageSpecDependency ParseDependency(JToken dependencyToken)
        {
            var property = dependencyToken as JProperty;

            var id = property.Name;

            string version = null;
            if (property.Value.Type == JTokenType.String)
            {
                version = (string)property.Value;
            }
            else if (property.Value.Type == JTokenType.Object)
            {
                version = (string)property.Value["version"];
            }

            if (!string.IsNullOrEmpty(version))
            {
                return new VsPackageSpecDependency(id, version);
            }

            return null;
        }

        private static VsPackageSpecEntry ParseEntry(JToken token)
        {
            var property = token as JProperty;

            if (property != null)
            {
                return new VsPackageSpecEntry(property.Name);
            }

            return null;
        }

        private static IEnumerable<VsPackageSpecEntry> GetEntries(JObject json, string parent)
        {
            JToken node = null;
            if (json.TryGetValue(parent, out node))
            {
                foreach (var entryToken in node)
                {
                    var result = ParseEntry(entryToken);

                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }

            yield break;
        }

        /// <summary>
        /// Add a dependency to a project.json file
        /// </summary>
        private static void AddDependency(JObject json, IVsPackageSpecDependency dependency)
        {
            JObject dependencySet = null;

            JToken node = null;
            if (json.TryGetValue("dependencies", out node))
            {
                dependencySet = node as JObject;
            }

            if (dependencySet == null)
            {
                dependencySet = new JObject();
            }

            var packageProperty = new JProperty(dependency.Id, dependency.VersionRange);
            dependencySet.Add(packageProperty);

            // order dependencies to reduce merge conflicts
            dependencySet = SortProperties(dependencySet);

            json["dependencies"] = dependencySet;
        }

        /// <summary>
        ///  Sort child properties
        /// </summary>
        private static JObject SortProperties(JObject parent)
        {
            var sortedParent = new JObject();

            var sortedChildren = parent.Children().OrderByDescending(child => GetChildKey(child), StringComparer.OrdinalIgnoreCase);

            foreach (var child in sortedChildren)
            {
                sortedParent.AddFirst(child);
            }

            return sortedParent;
        }

        private static string GetChildKey(JToken token)
        {
            var property = token as JProperty;

            if (property != null)
            {
                return property.Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// Remove a dependency from a project.json file
        /// </summary>
        private static void RemoveDependency(JObject json, string packageId)
        {
            JToken node = null;
            if (json.TryGetValue("dependencies", out node))
            {
                foreach (var dependency in node.ToArray())
                {
                    var dependencyProperty = dependency as JProperty;
                    if (StringComparer.OrdinalIgnoreCase.Equals(dependencyProperty.Name, packageId))
                    {
                        dependency.Remove();
                    }
                }
            }
        }

        private static void AddEntry(JObject json, string parent, IVsPackageSpecEntry entry)
        {
            JObject entrySet = null;

            JToken node = null;
            if (json.TryGetValue(parent, out node))
            {
                entrySet = node as JObject;
            }

            if (entrySet == null)
            {
                entrySet = new JObject();
            }

            var entryProperty = new JProperty(entry.Id, new JObject());
            entrySet.Add(entryProperty);

            // order dependencies to reduce merge conflicts
            entrySet = SortProperties(entrySet);

            json[parent] = entrySet;
        }

        private static void RemoveEntry(JObject json, string parent, IVsPackageSpecEntry entry)
        {
            JToken node = null;
            if (json.TryGetValue(parent, out node))
            {
                foreach (var entryNode in node.ToArray())
                {
                    var entryProperty = entryNode as JProperty;
                    if (StringComparer.OrdinalIgnoreCase.Equals(entryProperty.Name, entry.Id))
                    {
                        entryNode.Remove();
                    }
                }
            }
        }
    }
}
