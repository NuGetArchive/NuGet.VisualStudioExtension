// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.ProjectManagement;

namespace NuGet.VisualStudio
{
    internal sealed class VSAPIProjectContext : IMSBuildNuGetProjectContext
    {
        public VSAPIProjectContext()
            : this(false, false)
        {
        }

        public VSAPIProjectContext(bool skipAssemblyReferences, bool bindingRedirectsDisabled)
        {
            PackageExtractionContext = new PackageExtractionContext();

            SourceControlManagerProvider = ServiceLocator.GetInstanceSafe<ISourceControlManagerProvider>();
            SkipAssemblyReferences = skipAssemblyReferences;
            BindingRedirectsDisabled = bindingRedirectsDisabled;
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            // No logging needed when using the API
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ExecutionContext ExecutionContext
        {
            get { return null; }
        }

        public bool SkipAssemblyReferences { get; }

        public bool BindingRedirectsDisabled { get; }

        public void ReportError(string message)
        {
            // no-op
            Debug.Fail(message);
        }
    }
}
