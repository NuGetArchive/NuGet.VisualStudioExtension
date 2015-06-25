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
    /// Represents a project.json file.
    /// </summary>
    [ComImport]
    [Guid("66C1B338-EA97-4DBF-B921-9A6CC8595E21")]
    public interface IVsPackageSpec
    {
        string FilePath { get; }

        IReadOnlyList<IVsPackageSpecDependency> GetDependencies();

        void RemoveDependency(IVsPackageSpecDependency entry);

        void AddDependency(IVsPackageSpecDependency entry);

        IReadOnlyList<IVsPackageSpecEntry> GetRuntimes();

        void RemoveRuntime(IVsPackageSpecEntry entry);

        void AddRuntime(IVsPackageSpecEntry entry);

        IReadOnlyList<IVsPackageSpecEntry> GetSupports();

        void RemoveSupport(IVsPackageSpecEntry entry);

        void AddSupport(IVsPackageSpecEntry entry);

        IReadOnlyList<IVsPackageSpecEntry> GetFrameworks();

        void RemoveFramework(IVsPackageSpecEntry entry);

        void AddFramework(IVsPackageSpecEntry entry);
    }
}
