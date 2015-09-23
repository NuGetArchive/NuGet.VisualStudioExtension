using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.JSON.Core.Schema;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Utilities;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.VisualStudio
{
    [Export(typeof(IJSONSchemaSelector))]
    [Name("NuGetProjectJSONSchemaSelector")]
    [Order(Before = "NonKProjectJSONSchemaSelector")]
    public class NuGetProjectJSONSchemaSelector : IJSONSchemaSelector
    {
        private static IEnumerable<string> _noSchemas = new string[0];
        private static Task<IEnumerable<string>> _noSchemasTask = System.Threading.Tasks.Task.FromResult(_noSchemas);
        private static string schemaUrl = "http://json.schemastore.org/nuget-project";

#pragma warning disable 0067
        public event EventHandler AvailableSchemasChanged;
#pragma warning disable 0067

        public Task<IEnumerable<string>> GetAvailableSchemasAsync()
        {
            return _noSchemasTask;
        }

        public string GetSchemaFor(string fileLocation)
        {
            // Return null for projects that do not have project.json
            if (!fileLocation.EndsWith("\\project.json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            IVsHierarchy project;
            uint itemId;

            //If we couldn't figure out the project or it's a Project K project, don't set the schema
            if (!VsHierarchyUtility.TryGetHierarchy(fileLocation, out project, out itemId)
                || VsHierarchyUtility.IsProjectKProject(project))
            {
                return null;
            }

            return schemaUrl;
        }
    }
}