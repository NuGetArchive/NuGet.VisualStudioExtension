// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Creates <see cref="IVsPackageSourceContext"/> objects which contain NuGet package sources.
    /// </summary>
    [ComImport]
    [Guid("A308A86B-2884-4DC7-A293-86FF9D92B88F")]
    public interface IVsPackageSourceContextFactory
    {
        /// <summary>
        /// Creates a <see cref="IVsPackageSourceContext"/> with the given sources.
        /// </summary>
        /// <param name="includeEnabledSources">If true all default enabled sources will be added.</param>
        /// <param name="sources">Additional source urls or folder paths.</param>
        /// <param name="registryKeySources">Registry keys that point that package sources.</param>
        /// <param name="extensionIdSources">Extension ids to be used as repositories.</param>
        /// <returns>An <see cref="IVsPackageSourceContext"/> containing the given sources.</returns>
        IVsPackageSourceContext CreateContext(
            bool includeEnabledSources,
            IEnumerable<string> sources, 
            IEnumerable<string> registryKeySources, 
            IEnumerable<string> extensionIdSources);
    }
}
