﻿// Copyright (c) .NET Foundation. All rights reserved.
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
    public interface IVsPackageRestorer2
    {
        /// <summary>
        /// Restore packages for a project using the source context.
        /// </summary>
        /// <param name="project">Project to restore packages for.</param>
        /// <param name="sources">Sources to use during package restore.</param>
        /// <param name="token">Cancel token to allow cancelling the restore.</param>
        Task RestorePackages(Project project, IVsPackageSourceContext sources, CancellationToken token);
    }
}
