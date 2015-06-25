// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents a project.json file.
    /// </summary>
    [ComImport]
    [Guid("66C1B338-EA97-4DBF-B921-9A6CC8595E21")]
    public interface IVsPackageSpec
    {
        /// <summary>
        /// Reads dependency entries from the project.json file.
        /// </summary>
        IReadOnlyList<IVsPackageSpecDependency> Dependencies { get; }

        /// <summary>
        /// Removes a package dependency.
        /// </summary>
        /// <param name="entry">Dependency entry to remove.</param>
        void RemoveDependency(IVsPackageSpecDependency entry);

        /// <summary>
        /// Adds a new package dependency.
        /// </summary>
        /// <param name="entry">Dependency entry to add.</param>
        void AddDependency(IVsPackageSpecDependency entry);

        /// <summary>
        /// Reads runtime entries from the project.json file.
        /// </summary>
        IReadOnlyList<IVsPackageSpecEntry> Runtimes { get; }

        /// <summary>
        /// Removes a runtime entry.
        /// </summary>
        /// <param name="entry">Runtime entry to remove.</param>
        void RemoveRuntime(IVsPackageSpecEntry entry);

        /// <summary>
        /// Adds a new runtime entry.
        /// </summary>
        /// <param name="entry">Runtime entry to add.</param>
        void AddRuntime(IVsPackageSpecEntry entry);

        /// <summary>
        /// Reads support entries from the project.json file.
        /// </summary>
        IReadOnlyList<IVsPackageSpecEntry> Supports { get; }

        /// <summary>
        /// Removes a support entry.
        /// </summary>
        /// <param name="entry">Support entry to remove.</param>
        void RemoveSupport(IVsPackageSpecEntry entry);

        /// <summary>
        /// Adds a new support entry.
        /// </summary>
        /// <param name="entry">Support entry to add.</param>
        void AddSupport(IVsPackageSpecEntry entry);

        /// <summary>
        /// Reads framework entries from the project.json file.
        /// </summary>
        IReadOnlyList<IVsPackageSpecEntry> Frameworks { get; }

        /// <summary>
        /// Removes a target framework entry.
        /// </summary>
        /// <param name="entry">Framework entry to remove.</param>
        void RemoveFramework(IVsPackageSpecEntry entry);

        /// <summary>
        /// Adds a new framework entry.
        /// </summary>
        /// <param name="entry">Framework entry to add.</param>
        void AddFramework(IVsPackageSpecEntry entry);
    }
}
