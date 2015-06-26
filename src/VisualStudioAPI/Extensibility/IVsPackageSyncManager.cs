// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Provides support for updating project.json based projects after modifying the project.json file.
    /// </summary>
    [ComImport]
    [Guid("5006BAC9-CCC7-4AD1-85C2-BE0AF6463667")]
    public interface IVsPackageSyncManager
    {
        /// <summary>
        /// Restore packages for a project.json based project and generate a project.lock.json file.
        /// </summary>
        /// <param name="project">DTE project to update.</param>
        /// <param name="sources">Source information used to find packages.</param>
        /// <param name="token">Cancel token</param>
        /// <returns>Returns the operation status.</returns>
        Task<IVsPackageSyncResult> UpdateProjectAync(Project project, IVsPackageSourceContext sources, CancellationToken token);
    }
}
