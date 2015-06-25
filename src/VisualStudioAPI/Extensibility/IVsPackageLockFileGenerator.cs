// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Creates project.lock.json files.
    /// </summary>
    [ComImport]
    [Guid("147E9CAD-18A8-4A6D-BBDB-399C6E5AEA1F")]
    public interface IVsPackageLockFileGenerator
    {

    }
}
