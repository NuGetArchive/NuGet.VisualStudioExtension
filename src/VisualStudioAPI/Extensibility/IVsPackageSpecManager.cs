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
        Task<IVsPackageSpec> ReadAsync(Project project, CancellationToken token);

        Task<IVsPackageSpec> ReadAsync(Stream stream, CancellationToken token);

        Task WriteAsync(IVsPackageSpec packageSpec, Stream stream, CancellationToken token);

        Task WriteAsync(IVsPackageSpec packageSpec, Project project, CancellationToken token);

        Task<IVsPackageRestoreResult> RestoreProjectAsync(Project project, CancellationToken token);
    }
}
