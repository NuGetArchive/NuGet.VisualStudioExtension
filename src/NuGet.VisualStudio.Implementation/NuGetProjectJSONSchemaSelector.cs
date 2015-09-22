using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.JSON.Core.Schema;
using Microsoft.VisualStudio.Utilities;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement.Projects;

namespace NuGet.VisualStudio
{
    [Export(typeof(IJSONSchemaSelector))]
    [Name("NuGetProjectJSONSchemaSelector")]
    [Order(Before = "NonKProjectJSONSchemaSelector")]
    public class NuGetProjectJSONSchemaSelector : IJSONSchemaSelector
    {
        private static IEnumerable<string> _noSchemas = new string[0];
        private static Task<IEnumerable<string>> _noSchemasTask = Task.FromResult(_noSchemas);

#pragma warning disable 0067
        public event EventHandler AvailableSchemasChanged;
#pragma warning disable 0067

        public Task<IEnumerable<string>> GetAvailableSchemasAsync()
        {
            return _noSchemasTask;
        }

        public string GetSchemaFor(string fileLocation)
        {
            if (!fileLocation.EndsWith("\\project.json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            //If we couldn't figure out the project or it's a Project K project, don't set the schema
            ISolutionManager solutionManager = ServiceLocator.GetInstance<ISolutionManager>();
            if (solutionManager != null)
            {
                var project = solutionManager.DefaultNuGetProject;

                if (!(project is INuGetIntegratedProject))
                {
                    return null;
                }
                else if (project is ProjectKNuGetProject)
                {
                    return null;
                }
            }

            return "http://json.schemastore.org/nuget-project";
        }
    }
}