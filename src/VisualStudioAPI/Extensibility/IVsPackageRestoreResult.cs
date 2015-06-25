// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// The result of a package restore operation.
    /// </summary>
    [ComImport]
    [Guid("7369BAED-9557-4C49-B7F9-C96BA9B4D2BB")]
    public interface IVsPackageRestoreResult
    {
        /// <summary>
        /// True if the restore completed without errors.
        /// </summary>
        bool Success { get; }
    }
}