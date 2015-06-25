// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// IVsPackageSpecManager provides support for parsing and writing <see cref="IVsPackageSpec" /> objects.
    /// </summary>
    [ComImport]
    [Guid("907DF976-503E-492D-98FF-FCC3AF08E14B")]
    public interface IVsPackageSpecManager
    {
        /// <summary>
        /// Parses a project.json file from a project.
        /// </summary>
        /// <param name="project">DTE project containing a project.json file.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Returns the parsed project.json file.</returns>
        Task<IVsPackageSpec> ReadAsync(Project project, CancellationToken token);

        /// <summary>
        /// Writes an <see cref= "IVsPackageSpec" /> to the project.json file in a project.
        /// </summary>
        /// <remarks>The project must already contain a project.json file.</remarks>
        /// <param name="packageSpec">project.json file data.</param>
        /// <param name="project">DTE project containg a project.json file.</param>
        /// <param name="token">Cancellation token</param>
        Task WriteAsync(IVsPackageSpec packageSpec, Project project, CancellationToken token);

        /// <summary>
        /// Generates a project.lock.json file for the project and restores all packages to the
        /// global packages folder.
        /// </summary>
        /// <remarks><paramref name="additionalSourcesPaths"/> will be used along with the existing sources
        /// when finding and restoring packages. If no additional sources are needed this may be empty.</remarks>
        /// <param name="project">DTE project containing a project.json file.</param>
        /// <param name="additionalSourcePaths">Additional sources that may be used to restore packages from.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Returns an <see cref="IVsPackageRestoreResult"/> containing the status of the restore operation.</returns>
        Task<IVsPackageRestoreResult> RestoreProjectAsync(Project project, IEnumerable<string> additionalSourcePaths, CancellationToken token);
    }
}
