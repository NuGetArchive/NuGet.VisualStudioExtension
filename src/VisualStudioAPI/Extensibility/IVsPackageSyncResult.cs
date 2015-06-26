// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Package restore and update operation result for project.json based projects.
    /// </summary>
    [ComImport]
    [Guid("758AE2F5-A72D-4D4B-9FF7-8324B874BF89")]
    public interface IVsPackageSyncResult
    {
        /// <summary>
        /// True if the operation completed successfully.
        /// </summary>
        bool Success { get; }
    }
}
